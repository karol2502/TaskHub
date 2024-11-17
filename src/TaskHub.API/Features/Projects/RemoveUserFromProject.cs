﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskHub.Common.Enums;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskHub.Features.Projects;

public static class RemoveUserFromProject
{
    public class Command : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.UserId).NotEmpty();
        }
    }

    internal sealed class Handler(
        IUserContext userContext,
        TaskHubDbContext dbContext,
        UserManager<User> userManager)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var project = await dbContext
                              .Projects
                              .Include(x => x.UserProjects)
                              .FirstOrDefaultAsync(x => x.Id == request.ProjectId, cancellationToken)
                ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            // Check if user is admin
            var userProject = project.UserProjects.FirstOrDefault(x => x.UserId == currentUser.Id)
                              ?? throw new NotFoundException(nameof(Project), request.ProjectId.ToString());

            if (userProject.Role != Role.Admin)
            {
                throw new NotFoundException(nameof(Project), request.ProjectId.ToString());
            }

            // Check if user exists
            var user = userManager.FindByIdAsync(request.UserId)
                ?? throw new NotFoundException(nameof(User), request.UserId.ToString());

            // Remove user
            project.UserProjects = project.UserProjects.Where(x => x.UserId != request.UserId).ToList();

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
