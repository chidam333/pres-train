namespace ELearnApp.Models;

public class Material
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? FilePath { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public int LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public string? FileType { get; set; } = "text/plain";
}