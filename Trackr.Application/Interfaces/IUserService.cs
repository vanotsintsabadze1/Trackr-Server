﻿using Trackr.Application.Models;
using Trackr.Application.Models.Users;

namespace Trackr.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseModel> Register(UserRequestModel user);
    Task<string> Login(UserLoginRequestModel user);

}
