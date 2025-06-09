using Microsoft.AspNetCore.Mvc;
using ELearnApp.Contexts;
using ELearnApp.Models;
using ELearnApp.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("/api/enrollment")]
public class EnrollmentController : ControllerBase
{
    private readonly ElearnContext _elearnContext;

    public EnrollmentController(ElearnContext elearnContext)
    {
        _elearnContext = elearnContext;
    }

    [HttpPost]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> EnrollInCourse([FromBody] EnrollmentDto enrollmentDto)
    {
        if (enrollmentDto == null || enrollmentDto.CourseId <= 0)
        {
            return BadRequest("Invalid enrollment data.");
        }
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _elearnContext.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }
        var course = await _elearnContext.Courses.FindAsync(enrollmentDto.CourseId);
        if (course == null)
        {
            return NotFound("Course not found.");
        }

        var existingEnrollment = await _elearnContext.UserCourses
            .AnyAsync(e => e.UserId == user.Id && e.CourseId == enrollmentDto.CourseId);

        if (existingEnrollment)
        {
            return Conflict("User is already enrolled in this course.");
        }

        var enrollment = new UserCourse
        {
            UserId = user.Id,
            CourseId = enrollmentDto.CourseId,
        };

        await _elearnContext.UserCourses.AddAsync(enrollment);
        await _elearnContext.SaveChangesAsync();
        return Ok(enrollment);
    }

    [HttpDelete]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> UnenrollFromCourse([FromBody] EnrollmentDto enrollmentDto)
    {
        if (enrollmentDto == null || enrollmentDto.CourseId <= 0)
        {
            return BadRequest("Invalid unenrollment data.");
        }
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _elearnContext.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        var enrollment = await _elearnContext.UserCourses
            .SingleOrDefaultAsync(e => e.UserId == user.Id && e.CourseId == enrollmentDto.CourseId);

        if (enrollment == null)
        {
            return NotFound("Enrollment not found.");
        }

        _elearnContext.UserCourses.Remove(enrollment);
        await _elearnContext.SaveChangesAsync();
        return Ok("Successfully unenrolled from the course.");
    }
    [HttpGet("my-courses")]
    [Authorize(Roles = "student")]
    public async Task<IActionResult> GetMyCourses()
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _elearnContext.Users.SingleOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        var enrolledCourses = await _elearnContext.UserCourses
            .Where(uc => uc.UserId == user.Id)
            .Include(uc => uc.Course)
            .ToListAsync();

        return Ok(enrolledCourses);
    }
}