using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;

namespace Trackr.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("/v{version:apiversion}/User")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [ApiVersion(1)]
    [HttpPost("Register")]
    public async Task<UserResponseModel> Register([FromBody] UserRequestModel user)
    {
        UserResponseModel res = await _userService.Register(user);
        Log.Information("User has registered - {name} - {email}", user.Name, user.Email);
        return res;
    }

    [ApiVersion(1)]
    [HttpPost("Login")]
    public async Task<UserResponseModel> Login([FromBody] UserLoginRequestModel user)
    {
        UserResponseModel res = await _userService.Login(user);
        return res;
    }
}
