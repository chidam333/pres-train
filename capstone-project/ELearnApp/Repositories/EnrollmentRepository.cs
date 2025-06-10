using ELearnApp.Contexts;
using ELearnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearnApp.Repositories;

public class EnrollmentRepository : GenericRepository<UserCourse>
{
    public EnrollmentRepository(ElearnContext context) : base(context)
    {
    }

    public async Task<UserCourse> CreateEnrollmentAsync(UserCourse enrollment)
    {
        return await AddAsync(enrollment);
    }

    public async Task<bool> IsUserEnrolledAsync(int userId, int courseId)
    {
        return await AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
    }

    public async Task<UserCourse?> GetEnrollmentAsync(int userId, int courseId)
    {
        return await GetSingleOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);
    }

    public async Task DeleteEnrollmentAsync(UserCourse enrollment)
    {
        Remove(enrollment);
        await Task.CompletedTask;
    }

    public async Task<List<UserCourse>> GetUserEnrollmentsAsync(int userId)
    {
        return await Query()
            .Where(uc => uc.UserId == userId)
            .Include(uc => uc.Course)
            .ToListAsync();
    }
}
