namespace FaqAiApi.Models;

public class Session
{
    public int SessionId { get; set; }
    public string? UserId { get; set; }
    public string? LastIpAddress { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<BotRequest> BotRequests { get; set; } = new();
    public List<BotResponse> BotResponses { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
}
