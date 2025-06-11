using ELearnApp.Services;
using ELearnApp.Repositories;
using ELearnApp.Models;
using ELearnApp.Dtos;
using ELearnApp.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ELearnAppTest;

public class EnrollmentServiceTests : IDisposable
{
    private readonly ElearnContext _context;
    private readonly GenericRepository<UserCourse> _enrollmentRepository;
    private readonly UserRepository _userRepository;
    private readonly GenericRepository<Course> _courseRepository;
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentServiceTests()
    {
        var options = new DbContextOptionsBuilder<ElearnContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ElearnContext(options);
        _enrollmentRepository = new GenericRepository<UserCourse>(_context);
        _userRepository = new UserRepository(_context);
        _courseRepository = new GenericRepository<Course>(_context);
        _enrollmentService = new EnrollmentService(
            _enrollmentRepository, 
            _userRepository, 
            _courseRepository);
    }

    [Fact]
    public async Task EnrollInCourseAsync_ValidData_ReturnsSuccess()
    {
        var user = new User { Id = 1, Email = "student@yahoo.com", Name = "Student" };
        var course = new Course { Id = 1, Title = "Test Course", Description = "Test Description" };
        
        await _userRepository.AddAsync(user);
        await _courseRepository.AddAsync(course);
        await _context.SaveChangesAsync();
        
        var enrollmentDto = new EnrollmentDto { CourseId = 1 };
        var userEmail = "student@yahoo.com";

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail);

        Assert.True(result.Success);
        Assert.NotNull(result.Enrollment);
        Assert.Equal(user.Id, result.Enrollment.UserId);
        Assert.Equal(enrollmentDto.CourseId, result.Enrollment.CourseId);
    }

    [Fact]
    public async Task EnrollInCourseAsync_InvalidData_ReturnsFailure()
    {
        var enrollmentDto = new EnrollmentDto { CourseId = 0 };
        var userEmail = "student@yahoo.com";

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Enrollment);
        Assert.Equal("Invalid enrollment data.", result.Message);
    }

    [Fact]
    public async Task EnrollInCourseAsync_UserNotFound_ReturnsFailure()
    {
        var enrollmentDto = new EnrollmentDto { CourseId = 1 };
        var userEmail = "notfound@yahoo.com";

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Enrollment);
        Assert.Equal("User not found.", result.Message);
    }

    [Fact]
    public async Task EnrollInCourseAsync_CourseNotFound_ReturnsFailure()
    {
        var user = new User { Id = 1, Email = "student@yahoo.com", Name = "Student" };
        await _userRepository.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var enrollmentDto = new EnrollmentDto { CourseId = 999 };
        var userEmail = "student@yahoo.com";

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Enrollment);
        Assert.Equal("Course not found.", result.Message);
    }

    [Fact]
    public async Task EnrollInCourseAsync_AlreadyEnrolled_ReturnsFailure()
    {
        var user = new User { Id = 1, Email = "student@yahoo.com", Name = "Student" };
        var course = new Course { Id = 1, Title = "Test Course", Description = "Test Description" };
        var existingEnrollment = new UserCourse { UserId = 1, CourseId = 1 };
        
        await _userRepository.AddAsync(user);
        await _courseRepository.AddAsync(course);
        await _enrollmentRepository.AddAsync(existingEnrollment);
        await _context.SaveChangesAsync();
        
        var enrollmentDto = new EnrollmentDto { CourseId = 1 };
        var userEmail = "student@yahoo.com";

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Enrollment);
        Assert.Equal("User is already enrolled in this course.", result.Message);
    }

    [Fact]
    public async Task UnenrollFromCourseAsync_ValidData_ReturnsSuccess()
    {
        // Arrange
        var user = new User { Id = 1, Email = "student@yahoo.com", Name = "Student" };
        var enrollment = new UserCourse { UserId = 1, CourseId = 1 };
        
        await _userRepository.AddAsync(user);
        await _enrollmentRepository.AddAsync(enrollment);
        await _context.SaveChangesAsync();
        
        var enrollmentDto = new EnrollmentDto { CourseId = 1 };
        var userEmail = "student@yahoo.com";

        // Act
        var result = await _enrollmentService.UnenrollFromCourseAsync(enrollmentDto, userEmail);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Successfully unenrolled from the course.", result.Message);
    }

    [Fact]
    public async Task UnenrollFromCourseAsync_EnrollmentNotFound_ReturnsFailure()
    {
        // Arrange
        var user = new User { Id = 1, Email = "student@yahoo.com", Name = "Student" };
        await _userRepository.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var enrollmentDto = new EnrollmentDto { CourseId = 1 };
        var userEmail = "student@yahoo.com";

        // Act
        var result = await _enrollmentService.UnenrollFromCourseAsync(enrollmentDto, userEmail);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Enrollment not found.", result.Message);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
