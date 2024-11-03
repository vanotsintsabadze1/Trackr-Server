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
}
