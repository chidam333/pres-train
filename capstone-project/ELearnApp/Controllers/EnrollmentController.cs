using Microsoft.AspNetCore.Mvc;
using ELearnApp.Dtos;
using ELearnApp.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


[ApiController]
[Route("/api/v1/enrollment")]
public class EnrollmentController : ControllerBase
{
    private readonly EnrollmentService _enrollmentService;

    public EnrollmentController(EnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> EnrollInCourse([FromBody] EnrollmentDto enrollmentDto)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var result = await _enrollmentService.EnrollInCourseAsync(enrollmentDto, userEmail ?? string.Empty);

        if (!result.Success)
        {
            if (result.Message == "User not found.")
                return Unauthorized(result.Message);
            if (result.Message == "Course not found.")
                return NotFound(result.Message);
            if (result.Message == "User is already enrolled in this course.")
                return Conflict(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(result.Enrollment);
    }

    [HttpDelete]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> UnenrollFromCourse([FromBody] EnrollmentDto enrollmentDto)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var result = await _enrollmentService.UnenrollFromCourseAsync(enrollmentDto, userEmail ?? string.Empty);

        if (!result.Success)
        {
            if (result.Message == "User not found.")
                return Unauthorized(result.Message);
            if (result.Message == "Enrollment not found.")
                return NotFound(result.Message);
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
    }

    [HttpGet("my-courses")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetMyCourses()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        var enrolledCourses = await _enrollmentService.GetUserEnrollmentsAsync(userEmail ?? string.Empty);

        return Ok(enrolledCourses);
    }
}