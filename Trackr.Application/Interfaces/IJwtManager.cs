using Trackr.Domain.Models;

namespace Trackr.Application.Interfaces;

public interface IJwtManager
{
    Task<string> Create(User user);
}
