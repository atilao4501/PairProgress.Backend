using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairProgress.Backend.Models;
using PairProgress.Backend.Services.Interfaces;

namespace PairProgress.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class BuddyController : ControllerBase
{
    private readonly IBuddyService _buddyService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuddyController(IBuddyService buddyService, IHttpContextAccessor httpContextAccessor)
    {
        _buddyService = buddyService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetBuddyMood()
    {
        try
        {
            var userCode = _httpContextAccessor.HttpContext.User.Identity.Name;
            var mood = await _buddyService.GetBuddyMoodForUserAsync(userCode);

            return Ok(new DefaultReturn
            {
                Success = true,
                Message = "Buddy mood retrieved successfully",
                Data = mood
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