namespace ELearnApp.Models;

public class Course
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    public int CreatedById { get; set; }
    public User? CreatedBy { get; set; }
    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}