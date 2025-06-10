using ELearnApp.Dtos;
using ELearnApp.Models;
using ELearnApp.Repositories;

namespace ELearnApp.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _enrollmentRepository;
    private readonly UserRepository _userRepository;
    private readonly CourseRepository _courseRepository;

    public EnrollmentService(EnrollmentRepository enrollmentRepository, UserRepository userRepository, CourseRepository courseRepository)
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

        var course = await _courseRepository.GetCourseByIdAsync(enrollmentDto.CourseId);
        if (course == null)
        {
            return (false, null, "Course not found.");
        }

        if (await _enrollmentRepository.IsUserEnrolledAsync(user.Id, enrollmentDto.CourseId))
        {
            return (false, null, "User is already enrolled in this course.");
        }

        var enrollment = new UserCourse
        {
            UserId = user.Id,
            CourseId = enrollmentDto.CourseId,
        };

        await _enrollmentRepository.CreateEnrollmentAsync(enrollment);
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

        var enrollment = await _enrollmentRepository.GetEnrollmentAsync(user.Id, enrollmentDto.CourseId);
        if (enrollment == null)
        {
            return (false, "Enrollment not found.");
        }

        await _enrollmentRepository.DeleteEnrollmentAsync(enrollment);
        await _enrollmentRepository.SaveChangesAsync();

        return (true, "Successfully unenrolled from the course.");
    }

    public async Task<List<UserCourse>> GetUserEnrollmentsAsync(string userEmail)
    {
        var user = await _userRepository.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return new List<UserCourse>();
        }

        return await _enrollmentRepository.GetUserEnrollmentsAsync(user.Id);
    }
}
