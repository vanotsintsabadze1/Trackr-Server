using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trackr.Application.Interfaces;
using Trackr.Domain.Models;

namespace Trackr.Infrastructure.Utility;

public class JwtManager : IJwtManager
{
    private IConfiguration _configuration;
    public JwtManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> Create(User user)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = _configuration["Jwt:SecretKey"]!;
        var privateKey = Encoding.UTF8.GetBytes(key);

        var ssk = new SymmetricSecurityKey(privateKey);

        var credentials = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddDays(3),
        };

        var token = handler.CreateToken(tokenDescriptor);

        return Task.Run(() => handler.WriteToken(token));
    }

    private ClaimsIdentity GenerateClaims(User user)
    {
        var cl = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var ci = new ClaimsIdentity(cl);

        return ci;
    }
}
