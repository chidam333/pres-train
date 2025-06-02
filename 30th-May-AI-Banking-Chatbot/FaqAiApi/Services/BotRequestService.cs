using FaqAiApi.Dtos;
using FaqAiApi.Interfaces;
using FaqAiApi.Mappings;
using FaqAiApi.Models;

namespace FaqAiApi.Services;

public class BotRequestService : IBotRequestService
{
    private readonly IBotRequestRepository _botRequestRepository;
    private readonly RequestMapper _mapper;

    public BotRequestService(IBotRequestRepository botRequestRepository)
    {
        _botRequestRepository = botRequestRepository;
        _mapper = new RequestMapper();
    }

    public async Task<BotRequest> CreateRequestAsync(BotRequestDto createRequestDto)
    {
        var request = _mapper.Map(createRequestDto);
        var createdRequest = await _botRequestRepository.CreateAsync(request);
        return createdRequest;
    }

    public async Task<BotRequest> UpdateRequestAsync(int requestId, BotRequestDto requestDto)
    {
        var existingRequest = await _botRequestRepository.GetByIdAsync(requestId);
        if (existingRequest == null)
            throw new ArgumentException($"Request with ID {requestId} not found");
        _mapper.Map(requestDto);
        var updatedRequest = await _botRequestRepository.UpdateAsync(existingRequest);
        return updatedRequest;
    }
}
