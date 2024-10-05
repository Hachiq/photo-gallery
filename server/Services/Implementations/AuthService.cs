using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Core.Requests;

namespace Services.Implementations;

public class AuthService(
    IRepository _db,
    IPasswordService _passwordService,
    ITokenGenerator _accessTokenGenerator) : IAuthService
{
    public async Task Register(RegisterRequest model)
    {
        var existingUser = await _db.FindAsync<User>(u => u.Username == model.Username);

        if (existingUser is not null)
        {
            throw new UsernameTakenException();
        }

        _passwordService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = model.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };


        await _db.AddAsync(user);
        await _db.SaveChangesAsync();
    }
    public async Task<string> Login(LoginRequest model)
    {
        var user = await _db.FindAsync<User>(u => u.Username == model.Username) ?? throw new InvalidUsernameException();

        var passwordMatch = _passwordService.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt);

        if (!passwordMatch)
        {
            throw new WrongPasswordException();
        }

        var jwt = _accessTokenGenerator.GenerateAccessToken(user);

        return jwt;
    }
}
