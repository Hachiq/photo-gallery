using Core.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementations;

public class PasswordService : IPasswordService
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHach = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHach.SequenceEqual(passwordHash);
    }
}
