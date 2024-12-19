using Trackr.Application.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll(CancellationToken cancellationToken);
    Task<UserRequestModel> Register(UserRequestModel user, string hashedPassword);
    bool Delete(string id);
    Task<User?> Update(User user, CancellationToken cancellationToken);
    Task<User?> GetByEmail(string email, CancellationToken cancellationToken);
    Task<User?> Login(UserLoginRequestModel user);
    Task<User?> GetById(Guid id, CancellationToken cancellationToken);
    Task<User?> UpdateCostLimit(decimal costLimit, Guid id, CancellationToken cancellationToken);
    Task<User?> UpdateBalance(Guid id, BalanceModel newBalance, CancellationToken cancellationToken);
    Task<bool> ConfirmMail(string email);
}
