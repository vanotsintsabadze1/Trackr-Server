using Mapster;
using Trackr.Application.Exceptions;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Users;

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

    public async Task<UserResponseModel> Register(UserRequestModel user)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email); 

        if (userFromDb is not null)
        {
            throw new UserAlreadyExistsException("User already exists with this email address");
        }

        var hashedPassword = _passwordHasher.Hash(user.Password);

        await _userRepository.Register(user, hashedPassword);

        return user.Adapt<UserResponseModel>();
    }

    public async Task<string> Login(UserLoginRequestModel user)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email);

        if (userFromDb is null)
        {
            throw new UserInvalidCredentialsException("The user with such email does not exist", "InvalidEmail");
        }

        var matches = _passwordHasher.Verify(user.Password, userFromDb.Password);
        
        if (!matches)
        {
            throw new UserInvalidCredentialsException("The password is incorrect for the given user", "InvalidPassword");
        }

        var token = await _jwtManager.Create(userFromDb);

        return token;
    }
}
