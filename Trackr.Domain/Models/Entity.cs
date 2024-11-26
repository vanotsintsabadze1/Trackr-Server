using Trackr.Domain.Interfaces;

namespace Trackr.Domain.Models;

public class Entity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();  
}