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

    public PairController(IPairService pairService)
    {
        _pairService = pairService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePair(string userCode1, string userCode2)
    {
        var response = new DefaultReturn();
        try
        {
            var result = await _pairService.CreatePair(userCode1, userCode2);
            response.Success = true;
            response.Data = result;
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
    public async Task<IActionResult> DeletePair(string userCode)
    {
        var response = new DefaultReturn();
        try
        {
            var result = await _pairService.DeletePair(userCode);
            response.Success = true;
            response.Data = result;
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
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPairByUserCode(string userCode)
    {
        var response = new DefaultReturn();
        try
        {
            var result = await _pairService.GetPairByUserCode(userCode);
            response.Success = true;
            response.Data = result;
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