using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ContributionController : ControllerBase
{
    private readonly IContributionService _contributionService;
    
        public ContributionController(IContributionService contributionService)
        {
            _contributionService = contributionService;
        }
    
        [HttpPost]
        public async Task<IActionResult> AddContribution([FromBody] CreateContributionInput contributionInput)
        {
            try
            {
                await _contributionService.AddContributionAsync(contributionInput);
                return Ok(new DefaultReturn
                {
                    Success = true,
                    Message = "Contribution added successfully",
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
        public async Task<IActionResult> GetContributionsByGoal(int goalId)
        {
            try
            {
                var contributions = await _contributionService.GetContributionsByGoalAsync(goalId);
                return Ok(new DefaultReturn
                {
                    Success = true,
                    Message = "Contributions retrieved successfully",
                    Data = contributions
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