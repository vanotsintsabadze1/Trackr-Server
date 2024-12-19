using System.Security.Claims;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface IJwtManager
{
    Task<string> CreateJwtForUser(User user);
    Task<string> CreateJwtForEmailVerification(string email);
    ClaimsPrincipal Verify(string token);
}
