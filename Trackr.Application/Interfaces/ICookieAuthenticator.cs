using Trackr.Application.Models;

namespace Trackr.Application.Interfaces;

public interface ICookieAuthenticator
{
    Task SignInAsync(UserLoginRequestModel user);
    Task SignOutAsync();
}
