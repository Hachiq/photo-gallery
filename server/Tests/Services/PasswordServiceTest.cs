using Services.Implementations;

namespace Tests.Services;

public class PasswordServiceTests
{
    private readonly PasswordService _passwordService;

    public PasswordServiceTests()
    {
        _passwordService = new PasswordService();
    }

    [Fact]
    public void CreatePasswordHash_ValidPassword_GeneratesHashAndSalt()
    {
        // Arrange
        var password = "MySecurePassword";

        // Act
        _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Assert
        Assert.NotNull(passwordHash);
        Assert.NotEmpty(passwordHash);
        Assert.NotNull(passwordSalt);
        Assert.NotEmpty(passwordSalt);
    }

    [Fact]
    public void VerifyPasswordHash_ValidPassword_ReturnsTrue()
    {
        // Arrange
        var password = "MySecurePassword";
        _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Act
        var isPasswordValid = _passwordService.VerifyPasswordHash(password, passwordHash, passwordSalt);

        // Assert
        Assert.True(isPasswordValid);
    }

    [Fact]
    public void VerifyPasswordHash_InvalidPassword_ReturnsFalse()
    {
        // Arrange
        var password = "MySecurePassword";
        var wrongPassword = "WrongPassword";
        _passwordService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        // Act
        var isPasswordValid = _passwordService.VerifyPasswordHash(wrongPassword, passwordHash, passwordSalt);

        // Assert
        Assert.False(isPasswordValid);
    }
}
