using FaqAiApi.Models;
using FaqAiApi.Contexts;

namespace FaqAiApi.Repositories;

public class BotResponseRepository  
{
    private readonly FaqContext _context;

    public BotResponseRepository(FaqContext context)
    {
        _context = context;
    }

    public async Task<BotResponse> CreateAsync(BotResponse response)
    {
        _context.BotResponses.Add(response);
        await _context.SaveChangesAsync();
        return response;
    }

}
