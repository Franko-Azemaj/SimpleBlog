using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Application.Users;

namespace SimpleBlog.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<LoginResponse> Login(LoginRequest req)
    {
        var jwt = await _authService.AuthenticateUserAsync(req.Email, req.Password);

        return new LoginResponse
        {
            JWT = jwt
        };
    }   
}
