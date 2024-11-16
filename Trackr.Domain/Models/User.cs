namespace Trackr.Domain.Models;
public class User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required decimal Balance { get; set; }
    public required DateTime CreatedAt { get; set; }
}