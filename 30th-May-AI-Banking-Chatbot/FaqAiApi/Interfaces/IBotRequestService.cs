using FaqAiApi.Dtos;
using FaqAiApi.Models;

namespace FaqAiApi.Interfaces;

public interface IBotRequestService
{
    Task<BotRequest> CreateRequestAsync(BotRequestDto createRequestDto);
    Task<BotRequest> UpdateRequestAsync(int requestId, BotRequestDto requestDto);
}
