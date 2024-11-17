using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Features.Projects;

public static class GetProjectById
{
    public class Query : IRequest<ProjectDto>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
    }

    internal sealed class Handler(IUserContext userContext, TaskHubDbContext dbContext) : IRequestHandler<Query, ProjectDto>
    {
        public async Task<ProjectDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            // Check if user is member
            var userProject = dbContext.UserProjects.Where(x => x.ProjectId == request.ProjectId).FirstOrDefault(x => x.UserId == currentUser.Id)
                              ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            var project = await dbContext
                              .Projects
                              .Include(x => x.Tasks)
                              .ThenInclude(x => x.Comments)
                              .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            return project.AsDto();
        }
    }
}
