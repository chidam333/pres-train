namespace FirstAPI.Models.DTOs
{
    public class PatientAddDto
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}