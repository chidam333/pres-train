using ELearnApp.Services;
using ELearnApp.Repositories;
using ELearnApp.Models;
using ELearnApp.Dtos;
using ELearnApp.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ELearnAppTest;

public class AuthServiceTests : IDisposable
{
    private readonly ElearnContext _context;
    private readonly UserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ElearnContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ElearnContext(options);

        SeedDatabase();

        _userRepository = new UserRepository(_context);

        var mockConfig = new Mock<IConfiguration>();
        mockConfig.Setup(x => x["Keys:JwtTokenKey"])
            .Returns("this-is-a-very-long-secret-key-for-testing-purposes-that-is-at-least-32-characters");

        _tokenService = new TokenService(mockConfig.Object);
        _authService = new AuthService(_userRepository, _tokenService);
    }

    private void SeedDatabase()
    {
        _context.Roles.AddRange(
            new Role { Id = 3, RoleName = "student" },
            new Role { Id = 2, RoleName = "instructor" },
            new Role { Id = 1, RoleName = "admin" }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
    {
        var user = new User
        {
            Email = "test@yahoo.com",
            Name = "Test User",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        var credentials = new CredentialsDTO { Email = "test@yahoo.com", Password = "password123" };

        var result = await _authService.LoginAsync(credentials);

        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.Equal("Login Successful ! ", result.Message);
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsFailure()
    {
        var credentials = new CredentialsDTO { Email = "", Password = "" };

        var result = await _authService.LoginAsync(credentials);

        Assert.False(result.Success);
        Assert.Null(result.Token);
        Assert.Equal("Invalid credentials.", result.Message);
    }

    [Fact]
    public async Task LoginAsync_UserNotFound_ReturnsFailure()
    {
        var credentials = new CredentialsDTO { Email = "notfound@yahoo.com", Password = "password123" };

        var result = await _authService.LoginAsync(credentials);

        Assert.False(result.Success);
        Assert.Null(result.Token);
        Assert.Equal("User not found.", result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ValidUser_ReturnsSuccess()
    {
        var userDto = new UserDto
        {
            Email = "newuser@yahoo.com",
            Password = "password123",
            Name = "New User",
            Role = "student"
        };

        var result = await _authService.RegisterAsync(userDto);

        Assert.True(result.Success);
        Assert.NotNull(result.User);
        Assert.Equal(userDto.Email, result.User.Email);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
