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
    private IJwtManager _jwtManager;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
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

    public async Task<string> Login(UserLoginRequestModel user)
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

        var token = await _jwtManager.Create(userFromDb);

        return token;

    }
}
