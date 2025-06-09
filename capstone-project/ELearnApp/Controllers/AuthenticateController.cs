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
    public TokenService _tokenService;
    public ElearnContext elearnContext;
    public AuthenticateController(TokenService tokenService, ElearnContext context)
    {
        _tokenService = tokenService;
        elearnContext = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredentialsDTO cred)
    {
        if (cred == null || string.IsNullOrEmpty(cred.Email) || string.IsNullOrEmpty(cred.Password))
        {
            return BadRequest("Invalid credentials.");
        }
        var user = await elearnContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefaultAsync(u => u.Email == cred.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        if (!BCrypt.Net.BCrypt.Verify(cred.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid password.");
        }
        var token = await _tokenService.GenerateToken(user);
        return Ok(new { Token = token, Message = "Login Successful ! " });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Invalid user data.");
        }
        var existingUser = await elearnContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return Conflict("User with this email already exists.");
        }
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        User entryRecord = new User
        {
            Email = user.Email,
            Name = user.Name,
            PasswordHash = passwordHash
        };
        elearnContext.Users.Add(entryRecord);
        switch (user.Role?.ToLower())
        {
            case "admin":
                var adminRole = elearnContext.Roles.FirstOrDefault(r => r.RoleName == "admin");
                if (adminRole != null)
                {
                    elearnContext.UserRoles.Add(new UserRole
                    {
                        User = entryRecord,
                        Role = adminRole
                    });
                }
                break;
            case "instructor":
                var instructorRole = elearnContext.Roles.FirstOrDefault(r => r.RoleName == "instructor");
                if (instructorRole != null)
                {
                    elearnContext.UserRoles.Add(new UserRole
                    {
                        User = entryRecord,
                        Role = instructorRole
                    });
                }
                break;
            case "student":
                var studentRole = elearnContext.Roles.FirstOrDefault(r => r.RoleName == "student");
                if (studentRole != null)
                {
                    elearnContext.UserRoles.Add(new UserRole
                    {
                        User = entryRecord,
                        Role = studentRole
                    });
                }
                break;
            default:
                var defaultRole = elearnContext.Roles.FirstOrDefault(r => r.RoleName == "student");
                if (defaultRole != null)
                {
                    elearnContext.UserRoles.Add(new UserRole
                    {
                        User = entryRecord,
                        Role = defaultRole
                    });
                }
                break;
        }
        await elearnContext.SaveChangesAsync();
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