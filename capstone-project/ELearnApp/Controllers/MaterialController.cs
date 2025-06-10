using Microsoft.AspNetCore.Mvc;
using ELearnApp.Contexts;
using ELearnApp.Dtos;
using ELearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


[ApiController]
[Route("/api/material")]
public class MaterialController : ControllerBase
{
    private readonly ElearnContext _elearnContext;

    public MaterialController(ElearnContext elearnContext)
    {
        _elearnContext = elearnContext;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> UploadMaterial([FromForm] MaterialDto materialDto)
    {
        if (materialDto == null || string.IsNullOrWhiteSpace(materialDto.Title) || materialDto.LessonId <= 0 || materialDto.File == null)
        {
            return BadRequest("Invalid material data.");
        }
        var lesson = await _elearnContext.Lessons.Include(e => e.Course).ThenInclude(c => c.CreatedBy).FirstOrDefaultAsync(e => e.Id == materialDto.LessonId);
        if (lesson == null)
        {
            return NotFound("Lesson not found.");
        }
        if (lesson.Course == null)
        {
            return NotFound("Course not found");
        }
        if (lesson.Course.CreatedBy == null)
        {
            return NotFound("Creator not found");
        }
        if (lesson.Course.CreatedBy.Email != User.FindFirstValue(ClaimTypes.Email))
        {
            return Forbid("You are not authorized to upload materials for this lesson.");
        }
        var material = new Material
        {
            Title = materialDto.Title,
            Description = materialDto.Description,
            LessonId = materialDto.LessonId,
            FileType = materialDto.FileType
        };
        var filePath = Path.Combine(Environment.CurrentDirectory, "UploadedFiles", materialDto.File.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await materialDto.File.CopyToAsync(stream);
        }
        material.FilePath = filePath;
        _elearnContext.Materials.Add(material);
        await _elearnContext.SaveChangesAsync();

        return Ok(material);
    }

    [HttpGet("download/{filename}")]
    public ActionResult GetMaterialByFilename(string filename)
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, "UploadedFiles", filename);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found");
        }
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", filename);
    }
}