using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskHub.Common.Enums;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskHub.Features.Projects;

public static class UpdateProject
{
    public class Command : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
    
    internal sealed class Handler(IUserContext userContext, TaskHubDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            
            var project = await dbContext
                              .Projects
                              .Include(x => x.UserProjects)
                              .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());
            
            // Check if user is admin
            var userProject = project.UserProjects.FirstOrDefault(x => x.UserId == user.Id)
                              ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            if (userProject.Role != Role.Admin)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId.ToString());
            }

            if (request.Name is not null)
            {
                project.Name = request.Name;
            }
            if (request.Description is not null)
            {
                project.Description = request.Description;
            }
            
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}