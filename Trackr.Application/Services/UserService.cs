using Mapster;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Users;

namespace Trackr.Application.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private IPasswordHasher _passwordHasher;
    private ICookieAuthenticator _cookieAuthenticator;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, ICookieAuthenticator cookieAuthenticator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _cookieAuthenticator = cookieAuthenticator;
    }

    public async Task<UserResponseModel> Register(UserRequestModel user)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email); 

        if (userFromDb is not null)
        {
            throw new UserAlreadyExistsException("User already exists with this email address");
        }

        await _userRepository.Register(user);

        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> Login(UserLoginRequestModel user)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email);

        if (userFromDb is null)
        {
            throw new UserInvalidCredentialsException("The user with such email does not exist", "InvalidEmail");
        }

        var userResponseModel = await _userRepository.Login(user);

        if (userResponseModel is null)
        {
            throw new UserInvalidCredentialsException("The password is incorrect for the given user", "InvalidPassword");
        }

        await _cookieAuthenticator.SignInAsync(user);

        return userResponseModel;
    }
}
