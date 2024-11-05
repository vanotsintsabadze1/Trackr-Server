using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Trackr.Application.Interfaces;
using Trackr.Application.Models;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Repositories;

public class UserRepository : IUserRepository, IDisposable
{
    private string _conString;
    private List<User> _users = new();
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

        using SqlCommand com = new SqlCommand("INSERT INTO Users (Name, Email, Password) VALUES (@name, @email, @password)", connection: _con);
        com.Parameters.AddWithValue("@name", user.Name);
        com.Parameters.AddWithValue("@email", user.Email);
        com.Parameters.AddWithValue("@password", hashedPassword);

        await com.ExecuteNonQueryAsync();

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
        using SqlCommand com = new("SELECT * FROM Users", connection: _con);
        using SqlDataReader reader = await com.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            User user = new()
            {
                Id = reader["Id"].ToString() ?? string.Empty,
                CreatedAt = (DateTime)reader["CreatedAt"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Password = reader["Password"].ToString() ?? string.Empty
            };
            _users.Add(user);
        }

        await reader.CloseAsync();
        return _users;
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
        using SqlCommand com = new("SELECT * FROM Users WHERE Email  = @email", connection: _con);
        com.Parameters.AddWithValue("@email", email);
        using SqlDataReader reader = await com.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {

            User user = new()
            {
                Id = reader["Id"].ToString() ?? string.Empty,
                CreatedAt = (DateTime)reader["CreatedAt"],
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Password = reader["Password"].ToString() ?? string.Empty
            };
            return user;
        }

        return null;

    }
}
