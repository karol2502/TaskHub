using TaskHub.Common.Enums;

namespace TaskHub.Entities;

public sealed class Task
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Status Status { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public List<Comment> Comments { get; set; } = [];
}
