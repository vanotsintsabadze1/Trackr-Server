using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trackr.Domain.Interfaces;

namespace Trackr.Infrastructure.Repositories;

public class BaseRepository<T> where T : IEntity
{
    public readonly DbContext _dbContext;
    public readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var res = await _dbSet.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return res.Entity;
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        var res = await _dbSet.ToListAsync(cancellationToken);
        return res;
    }

    public async Task<T> Remove(T entity, CancellationToken cancellationToken)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        
        var res = _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return res.Entity;
    }

    public async Task<T> Update(T entity, CancellationToken cancellationToken)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var res = _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return res.Entity;
    }

    public async Task<T?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var res = await _dbSet.FindAsync(id, cancellationToken);
        
        if (res is null)
        {
            throw new Exception("Such entity was not found");
        }
        
        return res; 
    }
}