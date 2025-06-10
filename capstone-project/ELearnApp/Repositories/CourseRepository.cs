using ELearnApp.Contexts;
using ELearnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearnApp.Repositories;

public class CourseRepository : GenericRepository<Course>
{
    public CourseRepository(ElearnContext context) : base(context)
    {
    }

    public async Task<Course> CreateCourseAsync(Course course)
    {
        return await AddAsync(course);
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        return await GetByIdAsync(id);
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return (await GetAllAsync()).ToList();
    }

    public async Task<Course?> GetCourseWithCreatorAsync(int id)
    {
        return await Query()
            .Include(c => c.CreatedBy)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateCourseAsync(Course course)
    {
        Update(course);
        await Task.CompletedTask;
    }

    public async Task DeleteCourseAsync(Course course)
    {
        Remove(course);
        await Task.CompletedTask;
    }
}
