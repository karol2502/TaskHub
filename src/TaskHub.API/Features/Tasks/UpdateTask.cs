using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskHub.Common.Enums;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Features.Tasks;

public static class UpdateTask
{
    public class Command : IRequest<TaskDto>
    {
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Status? Status { get; set; }
    }

    internal sealed class Handler(TaskHubDbContext dbContext, IUserContext userContext) : IRequestHandler<Command, TaskDto>
    {
        public async Task<TaskDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var task = await dbContext.Tasks.Include(x => x.Project).ThenInclude(x => x.UserProjects).FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Entities.Task), request.TaskId.ToString());

            // Check if user is member
            var userProject = task.Project.UserProjects.FirstOrDefault(x => x.UserId == currentUser.Id)
                              ?? throw new NotFoundException(nameof(Entities.Task), request.TaskId.ToString());


            if(request.Name is not null)
            {
                task.Name = request.Name;
            }
            if(request.Description is not null)
            {
                task.Description = request.Description;
            }
            if(request.Status is not null)
            {
                task.Status = request.Status.Value;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return task.AsDto();
        }
    }
}
