using Riok.Mapperly.Abstractions;
using TaskHub.Entities;

namespace TaskHub.Models;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public UserDto User { get; set; } = default!;
    public TaskDto Task { get; set; } = default!;
}

[Mapper]
public static partial class CommentMapper
{
    public static partial CommentDto AsDto(this Comment comment);
}
