using Trackr.Domain.Interfaces;

namespace Trackr.Domain.Models;

public class Transaction : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required Guid UserId { get; set; }
    public required int Type { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required decimal Amount { get; set; }
    public DateTime TranDate { get; set; } = DateTime.UtcNow;
}

