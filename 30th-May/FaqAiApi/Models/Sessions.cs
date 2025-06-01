public class Session
{
    public required string SessionId { get; set; }
    public string? UserId { get; set; }
    public string? LastIpAddress { get; set; }
    public required DateTime CreatedAt { get; set; }
    public List<BotRequest> BotRequests { get; set; } = new();
    public List<BotResponse> BotResponses { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
}
