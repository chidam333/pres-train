using Microsoft.AspNetCore.Mvc;
using FaqAiApi.Dtos;
using FaqAiApi.Interfaces;
using FaqAiApi.Models;
using FaqAiApi.Services;

namespace FaqAiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BotRequestsController : ControllerBase
{
    private readonly IBotRequestService _botRequestService;
    private readonly BotResponseService _botResponseService;

    public BotRequestsController(IBotRequestService botRequestService, BotResponseService botResponseService)
    {
        _botRequestService = botRequestService;
        _botResponseService = botResponseService;
    }

    [HttpPost]
    public async Task<ActionResult<BotResponse>> CreateRequest([FromBody] BotRequestDto createRequestDto)
    {
        try
        {
            var request = await _botRequestService.CreateRequestAsync(createRequestDto);
            if (request == null)
            {
                return BadRequest("Failed to create request.");
            }
            var response = await _botResponseService.CreateResponseAsync(request);
            Console.WriteLine($"Response created (in br controller): {response.ResponseId}");
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{requestId:int}")]
    public async Task<ActionResult<BotRequest>> UpdateRequest(int requestId, [FromBody] BotRequestDto requestDto)
    {
        try
        {
            var updatedRequest = await _botRequestService.UpdateRequestAsync(requestId, requestDto);
            return Ok(updatedRequest);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
