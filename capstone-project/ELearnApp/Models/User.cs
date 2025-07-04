namespace ELearnApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Course> CreatedCourses { get; set; } = new List<Course>();
    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
}