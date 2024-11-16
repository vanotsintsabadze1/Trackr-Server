namespace Trackr.Application.Exceptions;

class TransactionDoesNotExist : Exception
{
    public string Code { get; set; }
    public TransactionDoesNotExist(string message, string code) : base(message)
    {
        Code = code;
    }
}
