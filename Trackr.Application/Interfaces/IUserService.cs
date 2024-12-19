using Trackr.Application.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseModel> Register(UserRequestModel user, CancellationToken cancellationToken);
    Task<string> Login(UserLoginRequestModel user, CancellationToken cancellationToken);
    Task<UserResponseModel> GetCurrentUser(string userId, CancellationToken cancellationToken);
    Task<UserResponseModel> UpdateCostLimit(decimal costLimit, string id, CancellationToken cancellationToken);
    Task<BalanceModel> GetBalance(string id, CancellationToken cancellationToken);
    Task<UserResponseModel> UpdateBalance(string id, BalanceModel newBalance, CancellationToken cancellationToken);
    Task<bool> ConfirmMail(string token);
}
