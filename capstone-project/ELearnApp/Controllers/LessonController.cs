using Microsoft.AspNetCore.Mvc;
using ELearnApp.Contexts;
using Microsoft.EntityFrameworkCore;
using ELearnApp.Dtos;
using ELearnApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using ELearnApp.Hubs;
using Serilog;

[ApiController]
[Route("/api/v1/lesson")]
public class LessonController : ControllerBase
{
    private readonly ElearnContext _elearnContext;
    private readonly IHubContext<NotifyHub> _hubContext;

    public LessonController(ElearnContext elearnContext, IHubContext<NotifyHub> hubContext)
    {
        _elearnContext = elearnContext;
        _hubContext = hubContext;
    }

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetLessonsByCourseId(int courseId)
    {
        if (courseId <= 0)
        {
            return BadRequest("Invalid course ID.");
        }

        var lessons = await _elearnContext.Lessons
            .Where(l => l.CourseId == courseId).ToListAsync();
        if (lessons == null)
        {
            return NotFound("No lessons found for this course.");
        }

        return Ok(lessons);
    }

    [HttpPost]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> CreateLesson([FromBody] LessonDto lesson)
    {
        if (lesson == null || string.IsNullOrWhiteSpace(lesson.Title) || lesson.CourseId <= 0)
        {
            return BadRequest("Invalid lesson data.");
        }
        var course = await _elearnContext.Courses.Include(e => e.CreatedBy).FirstOrDefaultAsync(c => c.Id == lesson.CourseId);
        if (course == null)
        {
            return NotFound("Course not found.");
        }
        if (course.CreatedBy == null)
        {
            return NotFound("Course creator not found.");
        }
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (course.CreatedBy.Email != userEmail)
        {
            return Forbid("You are not authorized to create lessons for this course.");
        }
        var newLesson = new Lesson
        {
            Title = lesson.Title,
            CourseId = lesson.CourseId,
            Description = lesson.Description,
            SequenceNo = lesson.SequenceNo,
        };
        try
        {
            _elearnContext.Lessons.Add(newLesson);
            await _elearnContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error creating new lesson.");
            return StatusCode(500, "Internal server error while creating lesson.");
        }
        try
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"New lesson created: {newLesson.Title} in course {course.Title} by {userEmail}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending notification for new lesson creation.");
        }
        return Ok(newLesson);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> UpdateLesson(int id, [FromBody] LessonDto lessonDto)
    {
        if (lessonDto == null || string.IsNullOrWhiteSpace(lessonDto.Title) || lessonDto.CourseId <= 0)
        {
            return BadRequest("Invalid lesson data.");
        }
        var course = await _elearnContext.Courses.Include(e => e.CreatedBy).FirstOrDefaultAsync(c => c.Id == lessonDto.CourseId);
        if (course == null)
        {
            return NotFound("Course not found.");
        }
        if (course.CreatedBy == null)
        {
            return NotFound("Course creator not found.");
        }
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (course.CreatedBy.Email != userEmail)
        {
            return Forbid("You are not authorized to update lessons for this course.");
        }
        var lesson = await _elearnContext.Lessons.FindAsync(id);
        if (lesson == null)
        {
            return NotFound("Lesson not found.");
        }
        lesson.Title = lessonDto.Title;
        lesson.Description = lessonDto.Description;
        lesson.SequenceNo = lessonDto.SequenceNo;
        lesson.CourseId = lessonDto.CourseId;
        try
        {
            _elearnContext.Lessons.Update(lesson);
            await _elearnContext.SaveChangesAsync();
        }catch(Exception ex)
        {
            Log.Error(ex, "Error updating lesson.");
            return StatusCode(500, "Internal server error while updating lesson.");
        }
        return Ok(lesson);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "instructor")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        var lesson = await _elearnContext.Lessons.Include(l => l.Course).ThenInclude(c => c!.CreatedBy).FirstOrDefaultAsync(l => l.Id == id);
        if (lesson == null)
        {
            return NotFound("Lesson not found.");
        }
        if (lesson.Course == null || lesson.Course.CreatedBy == null)
        {
            return NotFound("Course or course creator not found.");
        }
        if (lesson.Course.CreatedBy.Email == null)
        {
            return NotFound("Course creator email not found.");
        }
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (lesson.Course.CreatedBy.Email != userEmail)
        {
            return Forbid("You are not authorized to delete this lesson.");
        }
        _elearnContext.Lessons.Remove(lesson);
        await _elearnContext.SaveChangesAsync();
        return Ok(new { Message = "Lesson deleted successfully." });
    }
}