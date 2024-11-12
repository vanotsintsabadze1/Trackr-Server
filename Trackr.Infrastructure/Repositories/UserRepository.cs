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
    private string _conString;
    private SqlConnection _con;
    private IPasswordHasher _passwordHasher;

    public UserRepository(IConfiguration configuration, IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
        _conString = configuration.GetConnectionString(name: "DefaultConnection")!;
        _con = new(_conString);
        _con.Open();
    }
    public void Dispose()
    {
        _con.Close();
    }

    public async Task<UserRequestModel> Register(UserRequestModel user)
    {
        string hashedPassword = _passwordHasher.Hash(user.Password);
        
        await _con.ExecuteAsync("INSERT INTO Users (Name, Email, Password, Balance) VALUES (@name, @email, @password, @balance)",
        new
        {
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

    public async Task<UserResponseModel?> Login(UserLoginRequestModel user)
    {
        var userFromDb = await GetByEmail(user.Email);

        var matches = _passwordHasher.Verify(user.Password, userFromDb!.Password);

        if (!matches)
        {
            return null;
        }

        UserResponseModel response = new()
        {
            Name = userFromDb.Name,
            Email = userFromDb.Email,
        };

        return response;

    }

    public async Task<User?> GetByEmail(string email)
    {
        var user = await _con.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users where Email = @email", new { email });
        return user;
    }
}
