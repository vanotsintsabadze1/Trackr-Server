using System.Security.Claims;
namespace Trackr.API.Infrastructure.Helpers;

public static class CredentialRetriever
{
    public static string GetUserId(HttpContext ctx)
    {
        var user = ctx.User;
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return id!;
    }
}
