using Riok.Mapperly.Abstractions;
using TaskHub.Common.Enums;
using TaskHub.Entities;

namespace TaskHub.Models;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Status Status { get; set; }
    public ProjectDto Project { get; set; } = default!;
    public List<CommentDto> Comments { get; set; } = [];
}

[Mapper]
public static partial class TaskMapper
{
    public static partial TaskDto AsDto(this Entities.Task task);
    public static partial List<TaskDto> AsDtos(this List<Entities.Task> tasks);
}
