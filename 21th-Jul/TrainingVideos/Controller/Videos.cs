namespace TrainingVideos.Controllers;

using Microsoft.AspNetCore.Mvc;
using TrainingVideos.Models;
using TrainingVideos.Context;
using Microsoft.EntityFrameworkCore;
using TrainingVideos.Dtos;
using Azure.Storage.Blobs;

[ApiController]
[Route("api/videos")]
public class VideosController : ControllerBase
{
    private readonly TrainingDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;

    public VideosController(TrainingDbContext context, BlobServiceClient blobServiceClient)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
    {
        return await _context.Videos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Video>> GetVideo(int id)
    {
        var video = await _context.Videos.FindAsync(id);
        if (video == null)
        {
            return NotFound();
        }
        return video;
    }

    [HttpPost]
    public async Task<ActionResult<Video>> CreateVideo(VideoDto video)
    {
        if (video.File == null || video.File.Length == 0)
        {
            return BadRequest("File is required.");
        }
        var containerClient = _blobServiceClient.GetBlobContainerClient("training-videos");
        BlobClient blobClient = containerClient.GetBlobClient(video.File.FileName);
        await blobClient.UploadAsync(video.File.OpenReadStream(), true);
        var newVideo = new Video
        {
            Title = video.Title,
            Description = video.Description,
            BlobUrl = blobClient.Uri.ToString()
        };
        _context.Videos.Add(newVideo);
        await _context.SaveChangesAsync();
        return Ok(newVideo);
    }
}
