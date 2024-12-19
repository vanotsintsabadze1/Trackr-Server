namespace Trackr.Application.Exceptions;

public class ConflictException : Exception
{
    public string Code { get; set; }
    public ConflictException(string message, string code) : base(message)
    {
        Code = code;
    }
}
