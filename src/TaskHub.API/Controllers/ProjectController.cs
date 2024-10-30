using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Features.Projects;

namespace TaskHub.Controllers;

[Authorize]
[Route("api/projects")]
[ApiController]
public sealed class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProject.Command command)
    {
        var project = await mediator.Send(command);

        return Created($"api/projects/{project.Id}", project);
    }
}
