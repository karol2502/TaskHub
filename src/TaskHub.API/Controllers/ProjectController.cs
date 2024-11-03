using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Features.Projects;
using TaskHub.Models;

namespace TaskHub.Controllers;

[Route("api/projects")]
[Authorize]
[ApiController]
public sealed class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateProject([FromBody] CreateProject.Command command)
    {
        var project = await mediator.Send(command);

        return Created($"api/projects/{project.Id}", project);
    }
    
    [HttpPut("{projectId}")]
    public async Task<ActionResult<TaskDto>> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProject.Command command)
    {
        command.ProjectId = projectId;
        await mediator.Send(command);

        return NoContent();
    }
    
}
