using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Transactions;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;
using Trackr.Infrastructure.Context;

namespace Trackr.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(TrackrDBContext dbContext) : base(dbContext)
    {
    }

    public async Task<UserRequestModel> Register(UserRequestModel user, string hashedPassword)
    {
        var registrationUser = new User()
        {
            Name = user.Name,
            Email = user.Email,
            Password = hashedPassword,
            Balance = 0,
            CostLimit = 0,
            EmailConfirmed = false,
        };
        var res = await _dbSet.AddAsync(registrationUser);
        await _dbContext.SaveChangesAsync();

        return user;
    }
    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public new async Task<User?> Update(User user, CancellationToken cancellationToken)
    {
        var updatedUser = await base.Update(user, cancellationToken);
        return updatedUser;

    }

    public async Task<User?> UpdateCostLimit(decimal costLimit, Guid id, CancellationToken cancellationToken)
    {
        var user = await base.GetById(id, cancellationToken);
        if (user is not null)
        {
            user.CostLimit = costLimit;
            await base.Update(user, cancellationToken);
        }
        return user;

    }

    public new async Task<List<User>> GetAll(CancellationToken cancellationToken)
    {
        var users = await base.GetAll(cancellationToken);
        return users;
    }

    public async Task<User?> Login(UserLoginRequestModel user)
    {
        var userFromDb = await _dbSet.FirstOrDefaultAsync(u => u.Email == user.Email);
        return userFromDb;
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        return user;
    }

    public new async Task<User?> GetById(Guid id, CancellationToken cancellationToken)
    {
        var res = await base.GetById(id, cancellationToken);
        return res;
    }

    public async Task<User?> UpdateBalance(Guid id, BalanceModel newBalance, CancellationToken cancellationToken)
    {
        var user = await base.GetById(id, cancellationToken);
        if (user is not null)
        {
            user.Balance = newBalance.Balance;
            await base.Update(user, cancellationToken);
        };
        return user;
    }

    public async Task<bool> ConfirmMail(string email)
    {
        var user = await _dbSet.FirstOrDefaultAsync<User>(u => u.Email == email);
        
        if (user is not null)
        {
            user.EmailConfirmed = true;
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }
}