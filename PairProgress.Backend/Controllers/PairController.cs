using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class PairController : ControllerBase
{
    private readonly IPairService _pairService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PairController(IPairService pairService, IHttpContextAccessor httpContextAccessor)
    {
        _pairService = pairService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPair(string userCode2)
    {
        var response = new DefaultReturn();
        try
        {
            var userCode1 =_httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            var result = await _pairService.CreatePair(userCode1, userCode2);
            response.Success = true;
            response.Data = result;
            response.Message = "Pair created successfully.";
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
    
    [HttpDelete]
    public async Task<IActionResult> DeletePairByToken()
    {
        var response = new DefaultReturn();
        try
        {
            var userCode = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
            
            var result = await _pairService.DeletePair(userCode);
            response.Success = true;
            response.Data = result;
            response.Message = "Pair deleted successfully.";
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
    
    [HttpGet("{userCode}")]
    public async Task<IActionResult> GetPairByUserCode(string userCode)
    {
        var response = new DefaultReturn();
        try
        {
            var result = await _pairService.GetPairByUserCode(userCode);
            response.Success = true;
            response.Data = result;
            response.Message = "Pair retrieved successfully.";
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