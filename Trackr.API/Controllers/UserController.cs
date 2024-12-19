using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Trackr.API.Infrastructure.Helpers;
using Trackr.API.Infrastructure.Models;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Application.Models.Users;

namespace Trackr.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("/v{version:apiversion}/[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<UserResponseModel> Register([FromBody] UserRequestModel user, CancellationToken cancellationToken)
    {
        UserResponseModel res = await _userService.Register(user, cancellationToken);
        Log.Information("User has registered - {name} - {email}", user.Name, user.Email);
        return res;
    }

    [HttpPost("login")]
    public async Task<string> Login([FromBody] UserLoginRequestModel user, CancellationToken cancellationToken)
    {
        string res = await _userService.Login(user, cancellationToken);
        return res;
    }

    [Authorize]
    [HttpPatch("cost-limit")]
    public async Task<UserResponseModel> UpdateCostLimit([FromBody] CostLimitModel costLimit, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var user = await _userService.UpdateCostLimit(costLimit.CostLimit, id, cancellationToken);
        return user;
    }

    [Authorize]
    [HttpGet("balance")]
    public async Task<BalanceModel> GetBalance(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var balance = await _userService.GetBalance(id, cancellationToken);
        return balance;
    }

    [Authorize]
    [HttpPatch("balance")]
    public async Task<UserResponseModel> UpdateBalance(BalanceModel newBalance, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var response = await _userService.UpdateBalance(id, newBalance, cancellationToken);
        return response;
    }
}
