using FaqAiApi.Models;

namespace FaqAiApi.Interfaces;

public interface IBotRequestRepository
{
    Task<BotRequest> CreateAsync(BotRequest request);
    Task<BotRequest> UpdateAsync(BotRequest request);
    Task<BotRequest?> GetByIdAsync(int requestId);
}
