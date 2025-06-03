using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Auth;

class MinExpRequirement : IAuthorizationRequirement
{
    public int Experience { get; set; }

    public MinExpRequirement(int experience) { Experience = experience; }
}