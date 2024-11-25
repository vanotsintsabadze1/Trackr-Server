using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Trackr.Infrastructure.Repositories;

public class BaseRepository<T> where T : class
{
    private readonly DbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    /*
     * public async Task<T> Add(T entity, CancellationToken cancellationToken) 
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
    }
     */
    
    
    
}