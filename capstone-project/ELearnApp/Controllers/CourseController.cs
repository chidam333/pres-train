using System.Security.Claims;
using ELearnApp.Dtos;
using ELearnApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.AspNetCore.SignalR;
using ELearnApp.Hubs;

namespace ELearnApp.Controllers;

[ApiController]
[Route("/api/v1/course")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    private readonly EnrollmentService _enrollmentService;
    private readonly IHubContext<NotifyHub> _hubContext;

    public CourseController(CourseService courseService, EnrollmentService enrollmentService, IHubContext<NotifyHub> hubContext)
    {
        _courseService = courseService;
        _enrollmentService = enrollmentService;
        _hubContext = hubContext;
        Log.Information("CourseController initialized.");
    }
    [HttpPost]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> CreateCourse(CourseDtos courseDto)
    {
        Log.Information("Creating course with title: {Title}", courseDto.Title);
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User email not found in token.");
        }

        var result = await _courseService.CreateCourseAsync(courseDto, userEmail);

        if (!result.Success)
        {
            if (result.Message == "User not found.")
                return Unauthorized(result.Message);
            return BadRequest(result.Message);
        }
        try
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New course created: {courseDto.Title} by {userEmail}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending notification for new course creation.");
        }
        return Ok(result.Course);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetCourse(int id)
    {
        var result = await _courseService.GetCourseAsync(id);

        if (!result.Success)
        {
            return NotFound();
        }

        return Ok(result.Course);
    }

    [HttpGet("details/{id}")]
    [Authorize]
    public async Task<IActionResult> GetCourseDetails(int id)
    {
        var result = await _courseService.GetCourseDetailsAsync(id);
        
        if (result.Course == null)
        {
            return NotFound(result.Message ?? "Course not found.");
        }

        var course = result.Course;
        // Console.WriteLine($"Course: {course.Id} - {course.Title}");

        // var filteredResult = new
        // {
        //     course.Id,
        //     course.Title,
        //     course.Description,
        //     course.CreatedAt,
        //     course.Lessons,
        //     course.Thumbnail,
        //     course.CreatedById,
        //     course.CreatedBy
        // };
        
        return Ok(course);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }


    [HttpGet("instructor")]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> GetInstructorCourses()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User email not found in token.");
        }
        var result = await _courseService.GetCoursesByInstructorAsync(userEmail);
        if (result == null || !result.Any())
        {
            return NotFound("No courses found for this instructor.");
        }
        return Ok(result);
    }

    [HttpGet("others")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetOtherCourses()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
        {
            return Unauthorized("User email not found in token.");
        }
        var result = await _enrollmentService.GetOthersCoursesAsync(userEmail);
        if (result == null || !result.Any())
        {
            return NotFound("You are already enrolled in all available courses.");
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> UpdateCourse(int id, CourseDtos courseDto)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var result = await _courseService.UpdateCourseAsync(id, courseDto, userEmail ?? string.Empty);

        if (!result.Success)
        {
            if (result.Message == "Course not found." || result.Message == "Course author not found.")
                return NotFound(result.Message);
            if (result.Message == "You are not authorized to update this course.")
                return Unauthorized(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(result.Course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var result = await _courseService.DeleteCourseAsync(id, userEmail ?? string.Empty);

        if (!result.Success)
        {
            if (result.Message == "Course not found." || result.Message == "Course author not found.")
                return NotFound(result.Message);
            if (result.Message == "You are not authorized to delete this course.")
                return Unauthorized(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(new { Message = "Course deleted successfully." });
    }
}