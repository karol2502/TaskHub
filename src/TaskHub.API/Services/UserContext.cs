using System.Security.Claims;
using TaskHub.Models;

namespace TaskHub.Services;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser GetCurrentUser()
    {
        var user = (httpContextAccessor?.HttpContext?.User) ?? throw new InvalidOperationException("User context is not present");
        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("User is not authenticated");
        }
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;

        return new CurrentUser(userId, email);
    }
}
