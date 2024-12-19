namespace Trackr.Application.Exceptions;

public class UnauthorizedException : Exception
{
    public string Code { get; set; }
    public UnauthorizedException(string message, string? code = "UnauthorizedUser") : base(message)
    {
        Code = code ?? "UnauthorizedUser";
    }
}
