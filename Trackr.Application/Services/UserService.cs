using Mapster;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;

namespace Trackr.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtManager _jwtManager;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
    }

    public async Task<UserResponseModel> Register(UserRequestModel user, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email, cancellationToken); 

        if (userFromDb is not null)
        {
            throw new UserAlreadyExistsException("User already exists with this email address");
        }

        var hashedPassword = _passwordHasher.Hash(user.Password);

        var createdUser = await _userRepository.Register(user, hashedPassword);

        return createdUser.Adapt<UserResponseModel>();
    }

    public async Task<string> Login(UserLoginRequestModel user, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email, cancellationToken);

        if (userFromDb is null)
        {
            throw new InvalidUserException("The user with such email does not exist", "InvalidEmail");
        }

        var matches = _passwordHasher.Verify(user.Password, userFromDb.Password);
        
        if (!matches)
        {
            throw new InvalidUserException("The password is incorrect for the given user", "InvalidPassword");
        }

        var token = await _jwtManager.Create(userFromDb);

        return token;
    }

    public async Task<UserResponseModel> GetCurrentUser(string userId, CancellationToken cancellationToken)
    {
        Guid userGuidId = new Guid(userId);
        var user = await _userRepository.GetById(userGuidId, cancellationToken);
        
        if (user is null)
        {
            throw new InvalidUserException("User does not exist", "UserDoesNotExist");
        }

        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> UpdateCostLimit(decimal costLimit, string id, CancellationToken cancellationToken)
    {
        var userGuidId = new Guid(id);
        var user = await _userRepository.UpdateCostLimit(costLimit, userGuidId, cancellationToken);

        if (user is null)
        {
            throw new InvalidUserException("User does not exist", "UserDoesNotExist");
        }

        return user.Adapt<UserResponseModel>();
    }

    public async Task<BalanceModel> GetBalance(string id, CancellationToken cancellationToken)
    {
        var userGuidId = new Guid(id);
        var user = await _userRepository.GetById(userGuidId, cancellationToken);
        return user.Adapt<BalanceModel>();
    }

    public async Task<UserResponseModel> UpdateBalance(string id, BalanceModel newBalance, CancellationToken cancellationToken)
    {
        var userGuidId = new Guid(id);
        var updatedUser = await _userRepository.UpdateBalance(userGuidId, newBalance, cancellationToken);
        if (updatedUser is null)
        {
            throw new InvalidUserException("User does not exist", "UserDoesNotExist");
        }
        return updatedUser.Adapt<UserResponseModel>();
    }
}
