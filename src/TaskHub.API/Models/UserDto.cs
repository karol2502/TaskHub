using Riok.Mapperly.Abstractions;
using TaskHub.Entities;

namespace TaskHub.Models;

public class UserDto
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
}

[Mapper]
public static partial class UserMapper
{
    [MapPropertyFromSource(nameof(UserDto.Id))]
    [MapPropertyFromSource(nameof(UserDto.Email))]
    public static partial UserDto AsDto(this User user);
}
