namespace ELearnApp.Dtos;

public class MaterialDto
{
    public string? Title { get; set; }
    public int LessonId { get; set; }
    public int? SequenceNo { get; set; }
    public IFormFile? File { get; set; } = null!;
}