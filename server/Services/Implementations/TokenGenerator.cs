using Core.Contracts;
using Core.Entities;
using Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Services.Implementations;

public class TokenGenerator(IOptions<JwtSettings> jwtOptions) : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.Username.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Secret));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: credentials
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
