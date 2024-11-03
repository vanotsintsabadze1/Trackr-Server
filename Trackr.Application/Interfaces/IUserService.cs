using Trackr.Application.Models;

namespace Trackr.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseModel> Register(UserRequestModel user);
}
