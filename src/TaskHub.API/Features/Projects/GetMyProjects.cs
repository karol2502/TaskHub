using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Features.Projects;

public static class GetMyProjects
{
    public class Query : IRequest<List<ProjectDto>>;

    internal sealed class Handler(IUserContext userContext, TaskHubDbContext dbContext) : IRequestHandler<Query, List<ProjectDto>>
    {
        public async Task<List<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var projects = await dbContext.UserProjects
                .Include(x => x.Project)
                .Where(x => x.UserId == currentUser.Id)
                .Select(x => x.Project)
                .ToListAsync(cancellationToken);

            return projects.AsDtos();
        }
    }
}
