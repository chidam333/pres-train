namespace TrainingVideos.Dtos;

public class VideoDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
       public IFormFile? File { get; set; } = null!;
}
