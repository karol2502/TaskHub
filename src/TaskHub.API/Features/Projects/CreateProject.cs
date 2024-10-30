using FluentValidation;
using MediatR;
using TaskHub.Common.Enums;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Features.Projects;

public static class CreateProject
{
    public class Command : IRequest<ProjectDto>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }

    internal sealed class Handler(TaskHubDbContext dbContext, IUserContext userContext) : IRequestHandler<Command, ProjectDto>
    {
        public async Task<ProjectDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var newProject = new Project
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTimeOffset.UtcNow,
            };

            newProject.UserProjects.Add(new UserProject
            {
                UserId = currentUser.Id,
                Role = Role.Owner,
            });

            await dbContext.Projects.AddAsync(newProject, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return newProject.AsDto();
        }
    }
}
