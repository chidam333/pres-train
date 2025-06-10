namespace ELearnApp.Dtos;

public class MaterialDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int LessonId { get; set; }
    public string? FileType { get; set; } = "text/plain";
    public IFormFile File { get; set; } = null!;
}