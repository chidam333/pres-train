using System.Security.Claims;
using ELearnApp.Contexts;
using ELearnApp.Dtos;
using ELearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearnApp.Controllers;

[ApiController]
[Route("/api/course")]
public class CourseController : ControllerBase
{
    public ElearnContext _elearnContext;
    public CourseController(ElearnContext elearnContext)
    {
        _elearnContext = elearnContext;
    }
    [HttpPost]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> CreateCourse(CourseDtos courseDto)
    {
        Console.WriteLine("Creating course...");
        if (courseDto == null || string.IsNullOrEmpty(courseDto.Title) || string.IsNullOrEmpty(courseDto.Description))
        {
            return BadRequest("Invalid course data.");
        }
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        var userdb = _elearnContext.Users.SingleOrDefault(u => u.Email == userEmail);
        if (userdb == null)
        {
            return Unauthorized("User not found.");
        }
        var course = new Course
        {
            Title = courseDto.Title,
            Description = courseDto.Description,
            CreatedById = userdb.Id,
            Thumbnail = courseDto.Thumbnail,
        };

        await _elearnContext.Courses.AddAsync(course);
        await _elearnContext.SaveChangesAsync();
        return Ok(course);
    }
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetCourse(int id)
    {
        var course = await _elearnContext.Courses.FindAsync(id);
        if (course == null)
        {
            return NotFound();
        }
        return Ok(course);
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _elearnContext.Courses.ToListAsync();
        return Ok(courses);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = "admin, instructor")]
    public async Task<IActionResult> UpdateCourse(int id, CourseDtos courseDto)
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (courseDto == null || string.IsNullOrEmpty(courseDto.Title) || string.IsNullOrEmpty(courseDto.Description))
        {
            return BadRequest("Invalid course data.");
        }
        var course = await _elearnContext.Courses.Include(c => c.CreatedBy).FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
        {
            return NotFound();
        }
        if (course.CreatedBy == null)
        {
            return NotFound("Course author not found.");
        }
        var courseAuthor = course.CreatedBy;
        if (courseAuthor.Email != userEmail)
        {
            return Unauthorized("You are not authorized to update this course.");
        }
        course.Title = courseDto.Title;
        course.Description = courseDto.Description;
        course.Thumbnail = courseDto.Thumbnail;
        course.UpdatedAt = DateTime.UtcNow;
        _elearnContext.Courses.Update(course);
        await _elearnContext.SaveChangesAsync();
        return Ok(course);
    }
}