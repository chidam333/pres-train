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
        Log.Information("Updating lesson with ID: {LessonId} for course ID: {CourseId}", id, lessonDto.CourseId);

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
        using var transaction = await _elearnContext.Database.BeginTransactionAsync();
        try
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
            
            var courseId = lesson.CourseId;
            var deletedSequenceNo = lesson.SequenceNo;
            
            _elearnContext.Lessons.Remove(lesson);
            await _elearnContext.SaveChangesAsync();
            
            var remainingLessons = await _elearnContext.Lessons
                .Where(l => l.CourseId == courseId && l.SequenceNo > deletedSequenceNo)
                .OrderBy(l => l.SequenceNo)
                .ToListAsync();

            Log.Information("remainingLessons count: {Count}", remainingLessons.Count);

            for (int i = 0; i < remainingLessons.Count; i++)
            {
                remainingLessons[i].SequenceNo -= 1;
            }

            if (remainingLessons.Count > 0)
            {
                await _elearnContext.SaveChangesAsync();
            }
            
            await transaction.CommitAsync();
            return Ok(new { Message = "Lesson deleted successfully." });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await transaction.RollbackAsync();
            Log.Error(ex, "Concurrency conflict while deleting lesson with ID: {LessonId}", id);
            return Conflict("The lesson has been modified by another user. Please refresh and try again.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Log.Error(ex, "Error deleting lesson with ID: {LessonId}", id);
            return StatusCode(500, "Internal server error while deleting lesson.");
        }
    }
}