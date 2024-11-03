namespace Trackr.Application.Exceptions;

public class UserInvalidCredentialsException : Exception
{
    public string Code { get; set; }
    public UserInvalidCredentialsException(string message, string code) : base(message)
    {
        Code = code;
    }
}
