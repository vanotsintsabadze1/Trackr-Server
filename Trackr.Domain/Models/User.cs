namespace Trackr.Domain.Models;
public class User
{
    public required string Id { get; set; } = Guid.NewGuid().ToString();
    public required DateTime CreatedAt { get; set; } = DateTime.Now;
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
