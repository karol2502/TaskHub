namespace TaskHub.Entities;

public sealed class Comment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Content { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public Guid TaskId { get; set; }
    public Task Task { get; set; } = default!;
}
