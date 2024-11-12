namespace Trackr.Application.Models;

public class UserLoginRequestModel
{
    public required string Email { get; set; }
    public required string Password { get; set; }

}
