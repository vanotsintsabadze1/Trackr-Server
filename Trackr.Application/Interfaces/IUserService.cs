using Trackr.Application.Models;
using Trackr.Application.Models.Users;

namespace Trackr.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseModel> Register(UserRequestModel user, CancellationToken cancellationToken);
    Task<string> Login(UserLoginRequestModel user, CancellationToken cancellationToken);
    Task<UserResponseModel> GetCurrentUser(string userId, CancellationToken cancellationToken);
}
