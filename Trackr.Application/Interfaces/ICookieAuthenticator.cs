using Trackr.Application.Models;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface ICookieAuthenticator
{
    Task SignInAsync(UserLoginRequestModel user);
    Task SignOutAsync();
}
