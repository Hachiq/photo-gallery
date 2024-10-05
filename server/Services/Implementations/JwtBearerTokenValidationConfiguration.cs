using Core.Exceptions;
using Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Services.Implementations;

public class JwtBearerTokenValidationConfiguration(IOptions<JwtSettings> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    public void Configure(string? name, JwtBearerOptions options)
    {
        try
        {
            Configure(options);
        }
        catch (JwtConfigurationFailedException)
        {
            throw;
        }
    }

    public void Configure(JwtBearerOptions options)
    {
        if (_jwtSettings.Issuer is null || _jwtSettings.Audience is null || _jwtSettings.Secret is null)
        {
            throw new JwtConfigurationFailedException();
        }
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
        };
    }
}
