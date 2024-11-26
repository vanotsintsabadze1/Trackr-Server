namespace Trackr.Application.Exceptions;

public class UserUnauthorizedException : Exception
{
    public string Code { get; set; } = "UnauthorizedUser";
    public UserUnauthorizedException(string message, string? code = "UnauthorizedUser") : base(message)
    { }
}
