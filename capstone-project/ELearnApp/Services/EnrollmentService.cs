using ELearnApp.Dtos;
using ELearnApp.Models;
using ELearnApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ELearnApp.Services;

public class EnrollmentService
{
    private readonly GenericRepository<UserCourse> _enrollmentRepository;
    private readonly UserRepository _userRepository;
    private readonly GenericRepository<Course> _courseRepository;

    public EnrollmentService(GenericRepository<UserCourse> enrollmentRepository, UserRepository userRepository, GenericRepository<Course> courseRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
    }

    public async Task<(bool Success, UserCourse? Enrollment, string? Message)> EnrollInCourseAsync(EnrollmentDto enrollmentDto, string userEmail)
    {
        if (enrollmentDto == null || enrollmentDto.CourseId <= 0)
        {
            return (false, null, "Invalid enrollment data.");
        }

        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return (false, null, "User not found.");
        }

        var course = await _courseRepository.GetByIdAsync(enrollmentDto.CourseId);
        if (course == null)
        {
            return (false, null, "Course not found.");
        }

        if (await _enrollmentRepository.AnyAsync(e => e.UserId == user.Id && e.CourseId == enrollmentDto.CourseId))
        {
            return (false, null, "User is already enrolled in this course.");
        }

        var enrollment = new UserCourse
        {
            UserId = user.Id,
            CourseId = enrollmentDto.CourseId,
        };

        await _enrollmentRepository.AddAsync(enrollment);
        await _enrollmentRepository.SaveChangesAsync();

        return (true, enrollment, null);
    }

    public async Task<(bool Success, string? Message)> UnenrollFromCourseAsync(EnrollmentDto enrollmentDto, string userEmail)
    {
        if (enrollmentDto == null || enrollmentDto.CourseId <= 0)
        {
            return (false, "Invalid unenrollment data.");
        }

        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return (false, "User not found.");
        }

        var enrollment = await _enrollmentRepository.GetSingleOrDefaultAsync(e => e.UserId == user.Id && e.CourseId == enrollmentDto.CourseId);
        if (enrollment == null)
        {
            return (false, "Enrollment not found.");
        }

        _enrollmentRepository.Remove(enrollment);
        await _enrollmentRepository.SaveChangesAsync();

        return (true, "Successfully unenrolled from the course.");
    }

    public async Task<List<Course>> GetUserEnrollmentsAsync(string userEmail)
    {
        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return new List<Course>();
        }

        return await _courseRepository.Query()
            .Where(c => _enrollmentRepository.Query().Any(uc => uc.UserId == user.Id && uc.CourseId == c.Id))
            .ToListAsync();
    }

    public async Task<List<Course>> GetOthersCoursesAsync(string userEmail)
    {
        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return new List<Course>();
        }

        return await _courseRepository.Query()
            .Where(c => !_enrollmentRepository.Query().Any(uc => uc.UserId == user.Id && uc.CourseId == c.Id))
            .ToListAsync();
    }
}
