using ELearnApp.Dtos;
using ELearnApp.Models;
using ELearnApp.Repositories;

namespace ELearnApp.Services;

public class CourseService
{
    private readonly CourseRepository _courseRepository;
    private readonly UserRepository _userRepository;

    public CourseService(CourseRepository courseRepository, UserRepository userRepository)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
    }

    public async Task<(bool Success, Course? Course, string? Message)> CreateCourseAsync(CourseDtos courseDto, string userEmail)
    {
        if (courseDto == null || string.IsNullOrEmpty(courseDto.Title) || string.IsNullOrEmpty(courseDto.Description))
        {
            return (false, null, "Invalid course data.");
        }

        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return (false, null, "User not found.");
        }

        var course = new Course
        {
            Title = courseDto.Title,
            Description = courseDto.Description,
            CreatedById = user.Id,
            Thumbnail = courseDto.Thumbnail,
        };

        await _courseRepository.CreateCourseAsync(course);
        await _courseRepository.SaveChangesAsync();

        return (true, course, null);
    }

    public async Task<(bool Success, Course? Course, string? Message)> GetCourseAsync(int id)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id);
        if (course == null)
        {
            return (false, null, "Course not found.");
        }

        return (true, course, null);
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return await _courseRepository.GetAllCoursesAsync();
    }

    public async Task<(bool Success, Course? Course, string? Message)> UpdateCourseAsync(int id, CourseDtos courseDto, string userEmail)
    {
        if (courseDto == null || string.IsNullOrEmpty(courseDto.Title) || string.IsNullOrEmpty(courseDto.Description))
        {
            return (false, null, "Invalid course data.");
        }

        var course = await _courseRepository.GetCourseWithCreatorAsync(id);
        if (course == null)
        {
            return (false, null, "Course not found.");
        }

        if (course.CreatedBy == null)
        {
            return (false, null, "Course author not found.");
        }

        if (course.CreatedBy.Email != userEmail)
        {
            return (false, null, "You are not authorized to update this course.");
        }

        course.Title = courseDto.Title;
        course.Description = courseDto.Description;
        course.Thumbnail = courseDto.Thumbnail;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateCourseAsync(course);
        await _courseRepository.SaveChangesAsync();

        return (true, course, null);
    }

    public async Task<(bool Success, string? Message)> DeleteCourseAsync(int id, string userEmail)
    {
        var course = await _courseRepository.GetCourseWithCreatorAsync(id);
        if (course == null)
        {
            return (false, "Course not found.");
        }

        if (course.CreatedBy == null)
        {
            return (false, "Course author not found.");
        }

        if (course.CreatedBy.Email != userEmail)
        {
            return (false, "You are not authorized to delete this course.");
        }

        await _courseRepository.DeleteCourseAsync(course);
        await _courseRepository.SaveChangesAsync();

        return (true, null);
    }
}
