namespace Trackr.Application.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public string Code { get; set; }
    public UserAlreadyExistsException(string message = "User with this mail already exists", string code = "UserAlreadyExists") : base(message)
    {
        Code = code;
    }
}
