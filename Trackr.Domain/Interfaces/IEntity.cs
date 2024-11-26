namespace Trackr.Domain.Interfaces;

public class IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}