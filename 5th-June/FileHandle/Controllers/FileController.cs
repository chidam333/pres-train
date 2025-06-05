using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class fileController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        var filePath = Path.Combine("/Users/presidio/Documents/code-mac/pres-train/5th-June/FileHandle", "SavedFiles", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok(new { FilePath = filePath });
    }

    [HttpGet("download/{fileName}")]
    public ActionResult DownloadFile(string fileName)
    {
        var filePath = Path.Combine("/Users/presidio/Documents/code-mac/pres-train/5th-June/FileHandle", "SavedFiles", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", fileName);
    }
}