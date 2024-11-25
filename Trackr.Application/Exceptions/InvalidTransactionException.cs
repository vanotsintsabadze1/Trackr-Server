namespace Trackr.Application.Exceptions;

public class InvalidTransactionException : Exception
{
    public string Code { get; set; }
    public InvalidTransactionException(string message, string code) : base(message)
    {
        Code = code;
    }
}
