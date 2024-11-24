using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Application.Models.Users;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Repositories;

public class UserRepository : IUserRepository, IDisposable
{
    private readonly SqlConnection _con;

    public UserRepository(IConfiguration configuration)
    {
        var conString = configuration.GetConnectionString(name: "DefaultConnection")!;
        _con = new(conString);
        _con.Open();
    }
    public void Dispose()
    {
        _con.Close();
    }

    public async Task<UserRequestModel> Register(UserRequestModel user, string hashedPassword)
    {
        await _con.ExecuteAsync("INSERT INTO Users (Id, Name, Email, Password, Balance) VALUES (@id, @name, @email, @password, @balance)",
        new
        {
            @id = Guid.NewGuid(),
            name = user.Name,
            email = user.Email,
            password = hashedPassword,
            balance = 0M,
        });

        return user;
    }

    public bool Delete(string id)
    {
        throw new NotImplementedException();
    }

    public User Edit(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAll()
    {
        IEnumerable<User> users = await _con.QueryAsync<User>("SELECT * FROM Users");
        var listOfUsers = users.ToList();
        return listOfUsers;
    }

    public async Task<User?> Login(UserLoginRequestModel user)
    {
        var userFromDb = await GetByEmail(user.Email);
        return userFromDb;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users where Email = @email", new { email });
        return user;
    }

    public async Task<User> GetById(int id)
    {
        var user = await _con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
        return user!;
    }
}
