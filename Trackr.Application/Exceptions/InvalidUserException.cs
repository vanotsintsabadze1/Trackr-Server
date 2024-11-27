namespace Trackr.Application.Exceptions;

public class InvalidUserException : Exception
{
    public string Code { get; set; }
    public InvalidUserException(string message, string code) : base(message)
    {
        Code = code;
    }
}
