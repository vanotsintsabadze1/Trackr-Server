﻿using Mapster;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    private readonly IEmailSender _emailSender;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtManager jwtManager, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtManager = jwtManager;
        _emailSender = emailSender;
    }

    public async Task<UserResponseModel> Register(UserRequestModel user, CancellationToken cancellationToken)
    {
        var userFromDb = await _userRepository.GetByEmail(user.Email, cancellationToken); 

        if (userFromDb is not null)
        {
            throw new ConflictException("User already exists with this email address", "UserAlreadyExists");
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
            throw new NotFoundException("The user with such email does not exist", "InvalidEmail");
        }

        var matches = _passwordHasher.Verify(user.Password, userFromDb.Password);
        
        if (!matches)
        {
            throw new BadRequestException("The password is incorrect for the given user", "InvalidPassword");
        }

        var token = await _jwtManager.CreateJwtForUser(userFromDb);

        return token;
    }

    public async Task<UserResponseModel> GetCurrentUser(string userId, CancellationToken cancellationToken)
    {
        Guid userGuidId = new Guid(userId);
        var user = await _userRepository.GetById(userGuidId, cancellationToken);
        
        if (user is null)
        {
            throw new NotFoundException("User does not exist", "UserDoesNotExist");
        }

        return user.Adapt<UserResponseModel>();
    }

    public async Task<UserResponseModel> UpdateCostLimit(decimal costLimit, string id, CancellationToken cancellationToken)
    {
        var userGuidId = new Guid(id);
        var user = await _userRepository.UpdateCostLimit(costLimit, userGuidId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException("User does not exist", "UserDoesNotExist");
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
            throw new NotFoundException("User does not exist", "UserDoesNotExist");
        }
        return updatedUser.Adapt<UserResponseModel>();
    }
}
