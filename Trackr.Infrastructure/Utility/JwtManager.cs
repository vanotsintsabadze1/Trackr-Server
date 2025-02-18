﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Trackr.Application.Exceptions;
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

    public Task<string> CreateJwtForUser(User user)
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

    public Task<string> CreateJwtForEmailVerification(string email)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = _configuration["Jwt:SecretKey"]!;
        var privateKey = Encoding.UTF8.GetBytes(key);

        var ssk = new SymmetricSecurityKey(privateKey);

        var credentials = new SigningCredentials(ssk, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Subject = GenerateClaimsForEmailConfirmation(email),
            Expires = DateTime.UtcNow.AddMinutes(10)
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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var ci = new ClaimsIdentity(cl);

        return ci;
    }

    private ClaimsIdentity GenerateClaimsForEmailConfirmation(string email)
    {
        var cl = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email)
        };

        var ci = new ClaimsIdentity(cl);

        return ci;
    }

    public ClaimsPrincipal Verify(string token)
    {
        var key = _configuration["Jwt:SecretKey"];
        var privateKey = Encoding.UTF8.GetBytes(key!);

        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParams = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(privateKey)
        };

        var principal = tokenHandler.ValidateToken(token, validationParams, out SecurityToken secToken);

        if (secToken is JwtSecurityToken validToken)
        {
            return principal;
        }

        throw new UnauthorizedException("Token is invalid", "InvalidToken");
    }

}
