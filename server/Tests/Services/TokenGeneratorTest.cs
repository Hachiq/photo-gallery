using Core.Entities;
using Core.Options;
using Microsoft.Extensions.Options;
using Moq;
using Services.Implementations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tests.Services;

public class TokenGeneratorTests
{
    private readonly Mock<IOptions<JwtSettings>> _jwtSettingsMock;
    private readonly JwtSettings _jwtSettings;
    private readonly TokenGenerator _tokenGenerator;

    public TokenGeneratorTests()
    {
        _jwtSettings = new JwtSettings
        {
            Secret = "the_secret_authentication_key_must_be_sixty_four_characters_long",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            TokenExpirationInMinutes = 60
        };

        _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();
        _jwtSettingsMock.Setup(s => s.Value).Returns(_jwtSettings);

        _tokenGenerator = new TokenGenerator(_jwtSettingsMock.Object);
    }

    [Fact]
    public void GenerateAccessToken_ValidUser_ReturnsJwtToken()
    {
        // Arrange
        var passwordHash = Encoding.UTF8.GetBytes("fakePasswordHash");
        var passwordSalt = Encoding.UTF8.GetBytes("fakePasswordSalt");

        var user = new User
        {
            Username = "testuser",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = Core.Enums.Role.User
        };

        // Act
        var token = _tokenGenerator.GenerateAccessToken(user);

        // Assert
        Assert.NotNull(token);

        // Decode the token and validate its claims
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.NotNull(jwtToken);
        Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());

        // Validate the claims
        var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        Assert.Equal(user.Id.ToString(), idClaim);
        Assert.Equal(user.Username, nameClaim);
        Assert.Equal(user.Role.ToString(), roleClaim);

        // Check the token expiration time
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
    }
}