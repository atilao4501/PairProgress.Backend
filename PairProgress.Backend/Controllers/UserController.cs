using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;

    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _contextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserByToken()
    {
        var response = new DefaultReturn();
        try
        {
            var userCode = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            var users = await _userService.GetUserByUserCode(userCode);
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
    
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditUserByToken(UpdateUserInput userInput)
    {
        var response = new DefaultReturn();
        try
        {
            var userCode = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            await _userService.EditUserByCode(userInput, userCode);
            response.Success = true;
            response.Message = "User edited successfully.";
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
    
    [HttpGet("{username}")]
    public IActionResult CheckIfUsernameExists(string username)
    {
        var response = new DefaultReturn();
        try
        {
            var exists = _userService.CheckIfUsernameExists(username);
            response.Success = true;
            response.Data = exists;
            response.Message = exists ? "Username exists." : "Username does not exist.";
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred.";
            return StatusCode(500, response);
        }
    }
    
    [HttpGet("{email}")]
    public IActionResult CheckIfEmailExists(string email)
    {
        var response = new DefaultReturn();
        try
        {
            var exists = _userService.CheckIfEmailExists(email);
            response.Success = true;
            response.Data = exists;
            response.Message = exists ? "Email exists." : "Email does not exist.";
            return Ok(response);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "An error occurred.";
            return StatusCode(500, response);
        }
    }
    
    [HttpGet("{userCode}")]
    public async Task<IActionResult> GetUserNameByUserCode(string userCode)
    {
        var response = new DefaultReturn();
        try
        {
            var user = await _userService.GetUserByUserCode(userCode);
            response.Success = true;
            response.Data = user.UserName;
            response.Message = "UserName retrieved successfully.";
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