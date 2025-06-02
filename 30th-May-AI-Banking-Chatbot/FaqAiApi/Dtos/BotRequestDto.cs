namespace FaqAiApi.Dtos;

public class BotRequestDto
{
    public required string UserMessage { get; set; }
    public required int SessionId { get; set; }
}