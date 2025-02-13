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
        var response = new DefaultReturn();
        try
        {
            var token = await _authService.Login(loginModel);
            if (token == null)
            {
                response.Success = false;
                response.Message = "Unauthorized";
                return Unauthorized(response);
            }
    
            response.Success = true;
            response.Message = "Login successful";
            response.Data = token;
            return Ok(response);
        }
        catch (PersonalizedException ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred.";
            return StatusCode(500, response);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
    {
        var response = new DefaultReturn();
        try
        {
            var result = await _authService.Register(registerUserModel);
            if (!result)
            {
                response.Success = false;
                response.Message = "Registration failed";
                return BadRequest(response);
            }
    
            response.Success = true;
            response.Message = "Registration successful";
            return Ok(response);
        }
        catch (PersonalizedException ex)
        {
            response.Success = false;
            response.Message = ex.Message;
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred.";
            return StatusCode(500, response);
        }
    }
}