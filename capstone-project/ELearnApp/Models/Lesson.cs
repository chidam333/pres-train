namespace ELearnApp.Models;

public class Lesson
{
    public int Id { get; set; }
    public int? SequenceNo { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CourseId { get; set; }
    public Course? Course { get; set; }
    public ICollection<Material>? Materials { get; set; } = new List<Material>();
}