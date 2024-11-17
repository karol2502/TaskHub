using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskHub.Common.Enums;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskHub.Features.Tasks;

public static class CreateTask
{
    public class Command : IRequest<TaskDto>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Status Status { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Status).NotEmpty();
        }
    }

    internal sealed class Handler(TaskHubDbContext dbContext, IUserContext userContext) : IRequestHandler<Command, TaskDto>
    {
        public async Task<TaskDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var project = await dbContext
                              .Projects
                              .Include(x => x.UserProjects)
                              .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            // Check if user is member
            var userProject = project.UserProjects.FirstOrDefault(x => x.UserId == currentUser.Id)
                              ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            var newTask = new Entities.Task
            {
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
            };

            project.Tasks.Add(newTask);
            await dbContext.SaveChangesAsync(cancellationToken);

            return newTask.AsDto();
        }
    }
}
