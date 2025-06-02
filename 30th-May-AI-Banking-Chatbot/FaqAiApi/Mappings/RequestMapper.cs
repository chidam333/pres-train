using FaqAiApi.Models;
using FaqAiApi.Dtos;
namespace FaqAiApi.Mappings;

public class RequestMapper
{
    public BotRequest Map(BotRequestDto dto)
    {
        return new BotRequest
        {
            UserMessage = dto.UserMessage,
            SessionId = dto.SessionId
        };
    }
}