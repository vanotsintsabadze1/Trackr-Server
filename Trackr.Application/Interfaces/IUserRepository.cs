using Trackr.Application.Models;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;
namespace Trackr.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<UserRequestModel> Register(UserRequestModel user);
    bool Delete(string id);
    User Edit(string id);
    Task<User?> GetByEmail(string email);
    Task<User?> Login(UserLoginRequestModel user);
}
