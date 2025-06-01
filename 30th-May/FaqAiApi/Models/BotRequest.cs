public class BotRequest
{
    public required int RequestId { get; set; }
    public required string UserMessage { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required string SessionId { get; set; }
    public Session? Session { get; set; } 
}