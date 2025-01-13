using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Trackr.API.Infrastructure.Errors;
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

    /// <summary>
    /// Registers the user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">User registered successfully</response>
    /// <response code="400">Request contains invalid data</response>
    /// <response code="409">User with that email address already exists</response>
    /// <response code="500">Something went wrong on the server</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserResponseModel), 200)]
    [ProducesResponseType(typeof(APIError), 400)]
    [ProducesResponseType(typeof(APIError), 409)]
    [ProducesResponseType(typeof(APIError), 500)]
    [HttpPost("register")]
    public async Task<UserResponseModel> Register([FromBody] UserRequestModel user, CancellationToken cancellationToken)
    {
        UserResponseModel res = await _userService.Register(user, cancellationToken);
        Log.Information("User has registered - {name} - {email}", user.Name, user.Email);
        return res;
    }

    /// <summary>
    /// Logs the user in system
    /// </summary>
    /// <param name="user"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">User log in was successful</response>
    /// <response code="400">Request contains invalid data</response>
    /// <response code="401">Email was not confirmed or password was incorrect</response>
    /// <response code="404">User does not exist</response>
    /// <response code="500">Something went wrong on the server</response>
    [HttpPost("login")]
    public async Task<string> Login([FromBody] UserLoginRequestModel user, CancellationToken cancellationToken)
    {
        string res = await _userService.Login(user, cancellationToken);
        return res;
    }

    /// <summary>
    /// Returns user's cost limit
    /// </summary>
    /// <param name="costLimit"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Successfully updated user's monthly cost limit</response>
    /// <response code="400">Request contains invalid data</response>
    /// <response code="404">User does not exist</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpPatch("cost-limit")]
    public async Task<UserResponseModel> UpdateCostLimit([FromBody] CostLimitModel costLimit, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var user = await _userService.UpdateCostLimit(costLimit.CostLimit, id, cancellationToken);
        return user;
    }

    /// <summary>
    /// Returns user's current balance
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <response code="200">User's current balance was retrieved successfully</response>
    /// <response code="400">User's current balance was retrieved successfully</response>
    /// <response code="404">User does not exist</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpGet("balance")]
    public async Task<BalanceModel> GetBalance(CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var balance = await _userService.GetBalance(id, cancellationToken);
        return balance;
    }

    /// <summary>
    /// Updates user's current balance
    /// </summary>
    /// <param name="newBalance"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">User's balance was retrieved successfully</response>
    /// <response code="400">Request contains invalid data</response>
    /// <response code="404">User does not exist</response>
    /// <response code="500">Something went wrong on the server</response>
    [Authorize]
    [HttpPatch("balance")]
    public async Task<UserResponseModel> UpdateBalance(BalanceModel newBalance, CancellationToken cancellationToken)
    {
        var id = CredentialRetriever.GetUserId(HttpContext);
        var response = await _userService.UpdateBalance(id, newBalance, cancellationToken);
        return response;
    }

    /// <summary>
    /// Confirms the newly registered user's email
    /// </summary>
    /// <param name="token"></param>
    /// <response code="200">Successfully confirmed the email</response>
    /// <response code="400">Token was invalid or expired</response>
    /// <response code="500">Something went wrong on the server</response>
    [HttpPatch("/confirm-email/{token}")]
    public async Task<bool> ConfirmEmail(string token)
    {
        var response = await _userService.ConfirmMail(token);
        return response;
    }
}
