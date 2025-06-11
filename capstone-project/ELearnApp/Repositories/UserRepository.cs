using ELearnApp.Contexts;
using ELearnApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearnApp.Repositories;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(ElearnContext context) : base(context)
    {
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
    }

    public async Task<UserRole> CreateUserRoleAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
        return userRole;
    }
}
