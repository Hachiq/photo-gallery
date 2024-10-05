using Core.Contracts;
using Core.Exceptions;
using Core.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            await _authService.Register(request);
            return Ok();
        }
        catch (UsernameTakenException)
        {
            return Conflict();
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }
        catch (Exception ex) when (ex is InvalidUsernameException or WrongPasswordException)
        {
            return BadRequest();
        }
    }
}
