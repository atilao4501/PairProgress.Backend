using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserByCode(string userCode)
    {
        var response = new DefaultReturn();
        try
        {
            var users = await _userService.GetUserByCode(userCode);
            response.Success = true;
            response.Data = users;
            response.Message = "User retrieved successfully.";
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