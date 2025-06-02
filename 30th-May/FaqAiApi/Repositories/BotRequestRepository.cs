using FaqAiApi.Interfaces;
using FaqAiApi.Models;
using FaqAiApi.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FaqAiApi.Repositories;

public class BotRequestRepository : IBotRequestRepository
{
    private readonly FaqContext _context;

    public BotRequestRepository(FaqContext context)
    {
        _context = context;
    }
    public async Task<BotRequest> CreateAsync(BotRequest request)
    {
        _context.BotRequests.Add(request);
        await _context.SaveChangesAsync();
        return request;
    }

    public async Task<BotRequest> UpdateAsync(BotRequest request)
    {
        _context.BotRequests.Update(request);
        await _context.SaveChangesAsync();
        return request;
    }
    public async Task<BotRequest?> GetByIdAsync(int requestId)
    {
        return await _context.BotRequests
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
    }
}
