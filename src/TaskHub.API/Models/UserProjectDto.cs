using Riok.Mapperly.Abstractions;
using TaskHub.Common.Enums;
using TaskHub.Entities;

namespace TaskHub.Models;

public class UserProjectDto
{
    public Guid Id { get; set; } 
    public UserDto User { get; set; } = default!;
    public ProjectDto Project { get; set; } = default!;
    public Role Role { get; set; }
}

[Mapper]
public static partial class UserProjectMapper
{
    [MapperIgnoreSource(nameof(UserProject.UserId))]
    [MapperIgnoreSource(nameof(UserProject.ProjectId))]
    public static partial UserProjectDto AsDto(this UserProject user);
}
