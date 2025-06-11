using System.Security.Claims;
using ELearnApp.Contexts;
using ELearnApp.Dtos;
using ELearnApp.Models;
using ELearnApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ELearnApp.Controllers;

[ApiController]
[Route("/api/course")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;
    
    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
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
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
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