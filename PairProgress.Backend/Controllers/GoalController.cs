using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GoalController(IGoalService goalService, IHttpContextAccessor httpContextAccessor)
    {
        _goalService = goalService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalInput goalInput)
    {
        try
        {
            var userCode = _httpContextAccessor.HttpContext.User.Identity.Name;
            await _goalService.CreateGoal(goalInput, userCode);

            return Ok(new DefaultReturn
            {
                Success = true,
                Message = "Goal created successfully",
                Data = null
            });
        }
        catch (PersonalizedException ex)
        {
            return BadRequest(new DefaultReturn
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetGoalsByUserCode()
    {
        try
        {
            var userCode = _httpContextAccessor.HttpContext.User.Identity.Name;
            var goals = await _goalService.GetGoalsByUserCode(userCode);

            return Ok(new DefaultReturn
            {
                Success = true,
                Message = "Goals retrieved successfully",
                Data = goals
            });
        }
        catch (PersonalizedException ex)
        {
            return BadRequest(new DefaultReturn
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpPut]
    public async Task<IActionResult> EditGoalById([FromBody] UpdateGoalInput goalInput)
    {
        try
        {
            await _goalService.EditGoalById(goalInput);

            return Ok(new DefaultReturn
            {
                Success = true,
                Message = "Goal updated successfully",
                Data = null
            });
        }
        catch (PersonalizedException ex)
        {
            return BadRequest(new DefaultReturn
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpGet("{goalId}")]
    public async Task<IActionResult> GetGoalById(int goalId)
    {
        try
        {
            var goal = await _goalService.GetGoalById(goalId);

            return Ok(new DefaultReturn
            {
                Success = true,
                Message = "Goal retrieved successfully",
                Data = goal
            });
        }
        catch (PersonalizedException ex)
        {
            return BadRequest(new DefaultReturn
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }
}