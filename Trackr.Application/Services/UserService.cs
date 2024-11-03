using Mapster;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;

namespace Trackr.Application.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private IPasswordHasher _passwordHasher;
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseModel> Register(UserRequestModel user)
    {
        if (await _userRepository.GetByEmail(user.Email))
        {
            throw new UserAlreadyExistsException("User already exists with this email address");
        }

        await _userRepository.Register(user);

        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> Login(UserLoginRequestModel user)
    {
        var exists = await _userRepository.GetByEmail(user.Email);

        if (!exists)
        {
            throw new UserInvalidCredentialsException("The user with such email does not exist", "InvalidEmail");
        }

        var response = await _userRepository.Login(user);

        if (response is null)
        {
            throw new UserInvalidCredentialsException("The password is incorrect for the given user", "InvalidPassword");
        }

        return response;
    }
}
