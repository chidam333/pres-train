namespace ELearnApp.Dtos;

public class UserDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public string Role { get; set; } = "student";
}