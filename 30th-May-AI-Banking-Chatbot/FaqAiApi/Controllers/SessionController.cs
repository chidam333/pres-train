
using Microsoft.AspNetCore.Mvc;
using FaqAiApi.Models;
using FaqAiApi.Contexts;

namespace FaqAiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
   private readonly FaqContext _context;
    public SessionController(FaqContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSession([FromBody] Session session)
    {
        if (session == null)
        {
            return BadRequest("Invalid request");
        }

        var createdSession = await _context.Sessions.AddAsync(session);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        return Ok(createdSession);
    }
}