using Trackr.Application.Models;
using Trackr.Domain.Models;
namespace Trackr.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<UserRequestModel> Register(UserRequestModel user);
    bool Delete(string id);
    User Edit(string id);
    Task<bool> GetByEmail(string email);
    Task<UserResponseModel?> Login(UserLoginRequestModel user);
}
