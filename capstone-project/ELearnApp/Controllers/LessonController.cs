using Microsoft.AspNetCore.Mvc;
using ELearnApp.Contexts;
using Microsoft.EntityFrameworkCore;
using ELearnApp.Dtos;
using ELearnApp.Models;

[ApiController]
[Route("/api/lesson")]
public class LessonController : ControllerBase
{
    private readonly ElearnContext _elearnContext;

    public LessonController(ElearnContext elearnContext)
    {
        _elearnContext = elearnContext;
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
    public async Task<IActionResult> CreateLesson([FromBody] LessonDto lesson)
    {
        if (lesson == null || string.IsNullOrWhiteSpace(lesson.Title) || lesson.CourseId <= 0)
        {
            return BadRequest("Invalid lesson data.");
        }

        var newLesson = new Lesson
        {
            Title = lesson.Title,
            CourseId = lesson.CourseId,
            Description = lesson.Description,
            SequenceNo = lesson.SequenceNo,
        };

        _elearnContext.Lessons.Add(newLesson);
        await _elearnContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLessonsByCourseId), new { courseId = lesson.CourseId }, newLesson);
    }