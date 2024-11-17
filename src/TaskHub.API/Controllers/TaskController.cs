using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Entities;
using TaskHub.Features.Comments;
using TaskHub.Features.Projects;
using TaskHub.Features.Tasks;
using TaskHub.Models;

namespace TaskHub.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskController(IMediator mediator) : ControllerBase
{
    [HttpPost("{taskId}")]
    public async Task<ActionResult<ProjectDto>> UpdateTask([FromRoute] Guid taskId, [FromBody] UpdateTask.Command command)
    {
        command.TaskId = taskId;
        await mediator.Send(command);

        return Ok(command);
    }

    [HttpPost("{taskId}/createComment")]
    public async Task<ActionResult<CommentDto>> CreateComment([FromRoute] Guid taskId, [FromBody] CreateComment.Command command)
    {
        command.TaskId = taskId;
        await mediator.Send(command);

        return Ok(command);
    }
}
