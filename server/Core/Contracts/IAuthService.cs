using Core.Requests;

namespace Core.Contracts;

public interface IAuthService
{
    Task Register(RegisterRequest model);
    Task<string> Login(LoginRequest model);
}
