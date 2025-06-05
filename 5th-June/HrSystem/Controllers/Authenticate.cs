using HrSystem.Context;
using HrSystem.Models;
using System.Text;
using HrSystem.Dtos;
using HrSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HrSystem.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
    public TokenService _tokenService;
    public HrContext hrContext;
    public HMACSHA256 hMACSHA256;

    public AuthenticateController(TokenService tokenService, HrContext context)
    {
        _tokenService = tokenService;
        hrContext = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] CredentialsDTO cred)
    {
        if (cred == null || cred.Id <= 0 || string.IsNullOrEmpty(cred.Password))
        {
            return BadRequest("Invalid credentials.");
        }
        var user = await hrContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).SingleOrDefaultAsync(u => u.Id == cred.Id);
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var passwordParts = user.PasswordHash.Split(' ');
        string Password = passwordParts[0];
        string Hash = passwordParts[1];
        hMACSHA256 = new HMACSHA256(Convert.FromBase64String(Hash));
        var computedHash = hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(cred.Password));
        if (!computedHash.SequenceEqual(Convert.FromBase64String(Password)))
        {
            return Unauthorized("Invalid password.");
        }
        var token = await _tokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto user)
    {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Invalid user data.");
        }
        var existingUser = await hrContext.Users.SingleOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
        {
            return Conflict("User with this email already exists.");
        }
        hMACSHA256 = new HMACSHA256();
        var passwordHash = Convert.ToBase64String(hMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
        User entryRecord = new User
        {
            Email = user.Email,
            Name = user.Name,
            PasswordHash = $"{passwordHash} {Convert.ToBase64String(hMACSHA256.Key)}"
        };
        hrContext.Users.Add(entryRecord);
        if (user.Role == "HR")
        {
            var role = hrContext.Roles.FirstOrDefault(r => r.RoleName == "HR");
            hrContext.UserRoles.Add(new UserRole
            {
                User = entryRecord,
                Role = role
            });
        }
        else
        {
            var role = hrContext.Roles.FirstOrDefault(r => r.RoleName == "ROOKIE");
            hrContext.UserRoles.Add(new UserRole
            {
                User = entryRecord,
                Role = role
            });

        }
        await hrContext.SaveChangesAsync();
        return Ok(user);
    }
}