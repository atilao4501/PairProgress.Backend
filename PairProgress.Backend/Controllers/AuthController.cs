using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var token = await _authService.Login(loginModel);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
    {
        var result = await _authService.Register(registerUserModel);
        if (!result)
        {
            return BadRequest();
        }

        return Ok();
    }
}