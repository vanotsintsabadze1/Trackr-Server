using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using System.Security.Claims;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Utility;

public class CookieAuthenticator : ICookieAuthenticator
{
    private IHttpContextAccessor _httpContextAccessor;
    private IUserRepository _userRepository;
    public CookieAuthenticator(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task SignInAsync(UserLoginRequestModel user)
    {
        var _ctx = _httpContextAccessor.HttpContext;
        var userFromDb = (await _userRepository.GetByEmail(user.Email))!;

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.Name, userFromDb.Name),
            new Claim(ClaimTypes.Email, userFromDb.Email),
            new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Cookie");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = true,
        };
        await _ctx.SignInAsync("Cookie", claimsPrincipal, authProperties);
    }

    public Task SignOutAsync()
    {
        throw new NotImplementedException();
    }
}
