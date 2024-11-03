using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("Register")]
    [ApiVersion(1)]
    public async Task<UserResponseModel> Register([FromBody] UserRequestModel user)
    {
        UserResponseModel res = await _userService.Register(user);
        return res;
    }
}
