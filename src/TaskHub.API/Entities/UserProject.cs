using TaskHub.Common.Enums;

namespace TaskHub.Entities;

public sealed class UserProject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public Guid ProjectId { get; set; } 
    public Project Project { get; set; } = default!;
    public Role Role { get; set; }
}
