namespace FaqAiApi.Models;

public class BotRequest
{
    public int RequestId { get; set; }
    public required string UserMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required int SessionId { get; set; }
    public Session? Session { get; set; }
}