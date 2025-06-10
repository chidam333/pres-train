using ELearnApp.Contexts;
using ELearnApp.Models;
using ELearnApp.Dtos;
using ELearnApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Serilog;

namespace ELearnApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
    private readonly AuthService _authService;
    
    public AuthenticateController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredentialsDTO cred)
    {
        var result = await _authService.LoginAsync(cred);
        
        if (!result.Success)
        {
            if (result.Message == "User not found.")
                return NotFound(result.Message);
            if (result.Message == "Invalid password.")
                return Unauthorized(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(new { Token = result.Token, Message = result.Message });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        var result = await _authService.RegisterAsync(user);
        
        if (!result.Success)
        {
            if (result.Message == "User with this email already exists.")
                return Conflict(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(user);
    }
    [HttpPost("aboutme")]
    [Authorize]
    public IActionResult AboutMe()
    {
        Console.WriteLine("User is authenticated.");
        var claims = User.Claims.ToList();
        var name = User.Identity?.Name;
        Console.WriteLine($"{name}");
        Log.Information($"User claims: {string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}"))}");
        return Ok(new { Message = $"Hello, {claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value}!" });
    }
}