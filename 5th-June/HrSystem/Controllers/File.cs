using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HrSystem.Hubs;
using System;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class fileController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public fileController(IWebHostEnvironment environment, IHubContext<NotificationHub> hubContext)
    {
        _environment = environment;
        _hubContext = hubContext;
    }
    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        var filePath = Path.Combine(_environment.ContentRootPath, "SavedFiles", file.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        await _hubContext.Clients.All.SendAsync("FileUploaded", "File is uploaded");
        
        return Ok(new { FilePath = filePath });
    }

    [HttpGet("download/{fileName}")]
    [Authorize]
    public IActionResult DownloadFile(string fileName)
    {
        var filePath = Path.Combine(_environment.ContentRootPath, "SavedFiles", fileName);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }
        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", fileName);
    }
}