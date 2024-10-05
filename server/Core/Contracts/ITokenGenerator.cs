using Core.Entities;

namespace Core.Contracts;

public interface ITokenGenerator
{
    string GenerateAccessToken(User user);
}
