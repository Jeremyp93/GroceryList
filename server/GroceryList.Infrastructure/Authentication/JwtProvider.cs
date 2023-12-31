﻿using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GroceryList.Infrastructure.Authentication;
public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IHostEnvironment _env;

    public JwtProvider(IOptions<JwtOptions> options, IHostEnvironment env)
    {
        _jwtOptions = options.Value;
        _env = env;
    }

    public string Generate(User user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
        };
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, null, _env.IsDevelopment() ? DateTime.UtcNow.AddHours(8) : DateTime.UtcNow.AddHours(1), signingCredentials);
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
