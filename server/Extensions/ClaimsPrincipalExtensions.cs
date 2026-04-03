using System.Security.Claims;

namespace RooCode.Chat.Server.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(ClaimTypes.Name);
        return userId ?? throw new UnauthorizedAccessException("User id claim not found.");
    }
}
