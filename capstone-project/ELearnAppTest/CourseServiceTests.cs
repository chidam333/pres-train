using ELearnApp.Services;
using ELearnApp.Repositories;
using ELearnApp.Models;
using ELearnApp.Dtos;
using ELearnApp.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ELearnAppTest;

public class CourseServiceTests : IDisposable
{
    private readonly ElearnContext _context;
    private readonly GenericRepository<Course> _courseRepository;
    private readonly UserRepository _userRepository;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        var options = new DbContextOptionsBuilder<ElearnContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ElearnContext(options);
        _courseRepository = new GenericRepository<Course>(_context);
        _userRepository = new UserRepository(_context);
        _courseService = new CourseService(_courseRepository, _userRepository);
        var user = new User { Id = 1, Email = "instructor@yahoo.com", Name = "Instructor" };
        _userRepository.AddAsync(user);
        _userRepository.SaveChangesAsync();
    }

    [Fact]
    public async Task CreateCourseAsync_ValidData_ReturnsSuccess()
    {

        var courseDto = new CourseDtos
        {
            Title = "Test Course",
            Description = "Test Description",
            Thumbnail = "test.jpg"
        };
        var userEmail = "instructor@yahoo.com";

        var result = await _courseService.CreateCourseAsync(courseDto, userEmail);

        Assert.True(result.Success);
        Assert.NotNull(result.Course);
        Assert.Equal(courseDto.Title, result.Course.Title);
        Assert.Equal(courseDto.Description, result.Course.Description);
    }

    [Fact]
    public async Task CreateCourseAsync_InvalidData_ReturnsFailure()
    {
        var courseDto = new CourseDtos { Title = "", Description = "" };
        var userEmail = "instructor@yahoo.com";

        var result = await _courseService.CreateCourseAsync(courseDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Course);
        Assert.Equal("Invalid course data.", result.Message);
    }

    [Fact]
    public async Task CreateCourseAsync_UserNotFound_ReturnsFailure()
    {
        var courseDto = new CourseDtos
        {
            Title = "Test Course",
            Description = "Test Description"
        };
        var userEmail = "notfound@yahoo.com";

        var result = await _courseService.CreateCourseAsync(courseDto, userEmail);

        Assert.False(result.Success);
        Assert.Null(result.Course);
        Assert.Equal("User not found.", result.Message);
    }

    [Fact]
    public async Task GetCourseAsync_ExistingCourse_ReturnsSuccess()
    {
        var course = new Course { Title = "Test Course", Description = "Test Description" };
        await _courseRepository.AddAsync(course);
        await _courseRepository.SaveChangesAsync();

        var result = await _courseService.GetCourseAsync(1);

        Assert.True(result.Success);
        Assert.NotNull(result.Course);
        Assert.Equal(1, result.Course.Id);
    }

    [Fact]
    public async Task GetCourseAsync_NonExistingCourse_ReturnsFailure()
    {
        var courseId = 999;

        var result = await _courseService.GetCourseAsync(courseId);

        Assert.False(result.Success);
        Assert.Null(result.Course);
        Assert.Equal("Course not found.", result.Message);
    }

    [Fact]
    public async Task GetAllCoursesAsync_ReturnsAllCourses()
    {
        var courses = new List<Course>
        {
            new Course { Id = 1, Title = "Course 1", Description = "Description 1" },
            new Course { Id = 2, Title = "Course 2", Description = "Description 2" }
        };

        foreach (var course in courses)
        {
            await _courseRepository.AddAsync(course);
        }
        await _courseRepository.SaveChangesAsync();

        var result = await _courseService.GetAllCoursesAsync();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Title == "Course 1");
        Assert.Contains(result, c => c.Title == "Course 2");
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
