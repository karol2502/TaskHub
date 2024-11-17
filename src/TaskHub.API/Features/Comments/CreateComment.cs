using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskHub.Common.Exceptions;
using TaskHub.Entities;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Features.Comments;

public static class CreateComment
{
    public class Command : IRequest<CommentDto>
    {
        [JsonIgnore]
        public Guid TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Content).NotEmpty();
        }
    }

    internal sealed class Handler(TaskHubDbContext dbContext, IUserContext userContext) : IRequestHandler<Command, CommentDto>
    {
        public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var currentUser = userContext.GetCurrentUser();

            var task = await dbContext.Tasks.Include(x => x.Project).ThenInclude(x => x.UserProjects).FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Entities.Task), request.TaskId.ToString());

            // Check if user is member
            var userProject = task.Project.UserProjects.FirstOrDefault(x => x.UserId == currentUser.Id)
                              ?? throw new NotFoundException(nameof(Entities.Task), request.TaskId.ToString());

            var newComment = new Comment
            {
                UserId = currentUser.Id,
                Content = request.Content,
            };

            task.Comments.Add(newComment);
            await dbContext.SaveChangesAsync(cancellationToken);

            return newComment.AsDto();
        }
    }
}
