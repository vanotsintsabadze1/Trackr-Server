using Trackr.Application.Models;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;
namespace Trackr.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll(CancellationToken cancellationToken);
    Task<UserRequestModel> Register(UserRequestModel user, string hashedPassword);
    bool Delete(string id);
    User Edit(string id);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<User?> Login(UserLoginRequestModel user);
    Task<User?> GetById(Guid id, CancellationToken cancellationToken);
}
