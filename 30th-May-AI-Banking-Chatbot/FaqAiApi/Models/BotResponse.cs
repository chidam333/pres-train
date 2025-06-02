namespace FaqAiApi.Models;

public class BotResponse
{
    public int ResponseId { get; set; }
    public string? BotMessage { get; set; }
    public bool IsError { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required int SessionId { get; set; }
    public Session? Session { get; set; }
    public int? FeedbackId { get; set; }
    public Feedback? Feedback { get; set; }
}