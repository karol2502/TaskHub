using Microsoft.AspNetCore.Identity;

namespace TaskHub.Entities;

public sealed class User : IdentityUser
{
    public List<UserProject> UserProjects { get; set; } = [];
}
