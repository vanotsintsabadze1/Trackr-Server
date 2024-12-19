namespace Trackr.Application.Exceptions;

public class NotFoundException : Exception
{
    public string Code { get; set; }

    public NotFoundException(string message, string code) : base(message)
    {
        Code = code;
    }
}
