using Microsoft.AspNetCore.Mvc;
using ELearnApp.Contexts;
using ELearnApp.Dtos;
using ELearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ELearnApp.Hubs;
using Serilog;


[ApiController]
[Route("/api/v1/material")]
public class MaterialController : ControllerBase
{
    private readonly ElearnContext _elearnContext;
    private readonly IHubContext<NotifyHub> _hubContext;

    public MaterialController(ElearnContext elearnContext, IHubContext<NotifyHub> hubContext)
    {
        _elearnContext = elearnContext;
        _hubContext = hubContext;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> UploadMaterial(MaterialDto materialDto)
    {
        Console.WriteLine("Uploading material...");
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
        var materialsWithSameCourseId = _elearnContext.Materials.Include(m => m.Lesson).Where(m => m.Lesson != null && m.Lesson.CourseId == lesson.CourseId).OrderBy(m => m.LessonId).ThenBy(m => m.SequenceNo).ToList();
        var curSequenceNo = 1;
        int? fixedSequenceNo = null;
        foreach (var existingMaterial in materialsWithSameCourseId)
        {
            if (fixedSequenceNo == null && existingMaterial.LessonId > materialDto.LessonId)
            {
                fixedSequenceNo = curSequenceNo++;
            }
            existingMaterial.SequenceNo = curSequenceNo++;
        }
        var material = new Material
        {
            Title = materialDto.Title,
            LessonId = materialDto.LessonId,
            FileType = Path.GetExtension(materialDto.File.FileName),
            SequenceNo = fixedSequenceNo ?? curSequenceNo,
        };
        var filePath = Path.Combine(Environment.CurrentDirectory, "UploadedFiles", materialDto.Title + Path.GetExtension(materialDto.File.FileName));
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await materialDto.File.CopyToAsync(stream);
        }
        material.FilePath = filePath;
        try
        {
            Console.WriteLine(material);
            _elearnContext.Materials.Add(material);
            await _elearnContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        try
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New material uploaded: {material.Title} for lesson {lesson.Title} in course {lesson.Course.Title}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending notification: {ex.Message}");
        }

        return Ok(material);
    }

    [HttpGet("download/{filename}")]
    [Authorize]
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

    [HttpGet("lesson/{lessonId}")]
    [Authorize]
    public async Task<IActionResult> GetMaterialsByLessonId(int lessonId)
    {
        var materials = await _elearnContext.Materials
            .Where(m => m.LessonId == lessonId)
            .ToListAsync();

        if (materials == null)
        {
            return NotFound("No materials found for this lesson.");
        }

        return Ok(materials);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var material = await _elearnContext.Materials.Include(m => m.Lesson).ThenInclude(l => l.Course).ThenInclude(c => c.CreatedBy).FirstOrDefaultAsync(m => m.Id == id);
        if (material == null)
        {
            return NotFound("Material not found.");
        }
        if (material.Lesson == null || material.Lesson.Course == null || material.Lesson.Course.CreatedBy == null)
        {
            return NotFound("Related data not found.");
        }
        if (material.Lesson.Course.CreatedBy.Email != userEmail)
        {
            return Forbid("You are not authorized to delete this material.");
        }

        _elearnContext.Materials.Remove(material);
        
        var otherMaterialsOfCourse = await _elearnContext.Materials
            .Include(m => m.Lesson)
            .Where(m => m.Lesson != null && m.Lesson.CourseId == material.Lesson.CourseId && m.Id != id)
            .OrderBy(m => m.SequenceNo)
            .ToListAsync();
        int sequenceNo = 1;
        foreach (var otherMaterial in otherMaterialsOfCourse)
        {
            otherMaterial.SequenceNo = sequenceNo++;
        }
        await _elearnContext.SaveChangesAsync();

        return Ok(new { message = "Material deleted successfully.", material = material });
    }
}