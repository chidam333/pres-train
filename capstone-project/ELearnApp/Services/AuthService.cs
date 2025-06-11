using ELearnApp.Dtos;
using ELearnApp.Models;
using ELearnApp.Repositories;

namespace ELearnApp.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthService(UserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<(bool Success, string? Token, string? Message)> LoginAsync(CredentialsDTO credentials)
    {
        if (credentials == null || string.IsNullOrEmpty(credentials.Email) || string.IsNullOrEmpty(credentials.Password))
        {
            return (false, null, "Invalid credentials.");
        }

        var user = await _userRepository.GetUserByEmailAsync(credentials.Email);
        if (user == null)
        {
            return (false, null, "User not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(credentials.Password, user.PasswordHash))
        {
            return (false, null, "Invalid password.");
        }

        var token = await _tokenService.GenerateToken(user);
        return (true, token, "Login Successful ! ");
    }

    public async Task<(bool Success, User? User, string? Message)> RegisterAsync(UserDto userDto)
    {
        if (userDto == null || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
        {
            return (false, null, "Invalid user data.");
        }

        if (await _userRepository.AnyAsync(u => u.Email == userDto.Email))
        {
            return (false, null, "User with this email already exists.");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
        var user = new User
        {
            Email = userDto.Email,
            Name = userDto.Name,
            PasswordHash = passwordHash
        };

        await _userRepository.AddAsync(user);

        var roleName = string.IsNullOrEmpty(userDto.Role) ? "student" : userDto.Role.ToLower();
        var role = await _userRepository.GetRoleByNameAsync(roleName);
        if (role == null)
        {
            return (false, null, $"Role '{roleName}' does not exist.");
        }
        await _userRepository.CreateUserRoleAsync(new UserRole
        {
            User = user,
            Role = role
        });
        await _userRepository.SaveChangesAsync();
        return (true, user, null);
    }
}
