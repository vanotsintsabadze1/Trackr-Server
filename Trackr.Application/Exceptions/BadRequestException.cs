namespace Trackr.Application.Exceptions;

public class BadRequestException : Exception
{
    public string Code { get; set; }
    public BadRequestException(string message, string code) : base(message)
    {
        Code = code;
    }
}
