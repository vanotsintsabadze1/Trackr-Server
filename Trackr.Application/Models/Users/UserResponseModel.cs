namespace Trackr.Application.Models.Users;

public class UserResponseModel
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required decimal CostLimit { get; set; }
}
