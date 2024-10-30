namespace TaskHub.Entities;

public sealed class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<Task> Tasks { get; set; } = [];
    public List<UserProject> UserProjects { get; set; } = [];
}
