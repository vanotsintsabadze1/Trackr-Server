using Trackr.Domain.Interfaces;

namespace Trackr.Domain.Models;
public class User : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public required decimal CostLimit { get; set; }
    public required bool EmailConfirmed { get; set; }
}