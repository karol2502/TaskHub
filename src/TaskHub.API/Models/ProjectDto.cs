using Riok.Mapperly.Abstractions;
using TaskHub.Entities;
using TaskHub.Features.Projects;

namespace TaskHub.Models;

public class ProjectDto
{
    public Guid Id { get; set; } 
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<TaskDto> Tasks { get; set; } = [];
}

[Mapper]
public static partial class ProjectMapper
{
    public static partial ProjectDto AsDto(this Project project);
    public static partial List<ProjectDto> AsDtos(this List<Project> projects);
}

