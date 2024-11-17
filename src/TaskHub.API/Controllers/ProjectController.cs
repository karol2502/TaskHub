using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Features.Projects;
using TaskHub.Features.Tasks;
using TaskHub.Models;

namespace TaskHub.Controllers;

[Route("api/projects")]
[Authorize]
[ApiController]
public sealed class ProjectController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject([FromBody] CreateProject.Command command)
    {
        var project = await mediator.Send(command);

        return Created($"api/projects/{project.Id}", project);
    }
    
    [HttpPut("{projectId}")]
    public async Task<IActionResult> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProject.Command command)
    {
        command.ProjectId = projectId;
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("{projectId}/addUser")]
    public async Task<IActionResult> AddUser([FromRoute] Guid projectId, [FromBody] AddUserToProject.Command command)
    {
        command.ProjectId = projectId;
        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("{projectId}/removeUser")]
    public async Task<IActionResult> RemoveUser([FromRoute] Guid projectId, [FromBody] RemoveUserFromProject.Command command)
    {
        command.ProjectId = projectId;
        await mediator.Send(command);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetProjects()
    {
        var projects = await mediator.Send(new GetMyProjects.Query());

        return Ok(projects);
    }

    [HttpGet("{projectId}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById([FromRoute] Guid projectId)
    {
        var projects = await mediator.Send(new GetMyProjects.Query());

        return Ok(projects);
    }

    [HttpPost("{projectId}/createTask")]
    public async Task<ActionResult<ProjectDto>> CreateTask([FromRoute] Guid projectId, [FromBody] CreateTask.Command command)
    {
        command.ProjectId = projectId;
        var task = await mediator.Send(command);
        return Created($"api/tasks/{task.Id}", task);
    }
}
