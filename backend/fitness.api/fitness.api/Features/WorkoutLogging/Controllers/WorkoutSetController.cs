using System.Security.Claims;
using fitness.api.Features.WorkoutLogging.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fitness.api.Features.WorkoutLogging.Controllers;

[ApiController]
[Route("workout-sessions/{sessionId:guid}/exercises/{exerciseId:guid}/sets")]
[Authorize]
public class WorkoutSetController : ControllerBase
{
    private readonly IWorkoutSetService _setService;

    public WorkoutSetController(IWorkoutSetService setService)
    {
        _setService = setService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutSetResponse>> Add(Guid exerciseId, CreateWorkoutSetRequest request)
    {
        var response = await _setService.AddSetAsync(GetUserId(), exerciseId, request);
        return Created(string.Empty, response);
    }

    [HttpPatch("{setId:guid}")]
    public async Task<ActionResult<WorkoutSetResponse>> Update(Guid setId, UpdateWorkoutSetRequest request)
    {
        var response = await _setService.UpdateSetAsync(GetUserId(), setId, request);
        return Ok(response);
    }

    [HttpDelete("{setId:guid}")]
    public async Task<IActionResult> Delete(Guid setId)
    {
        await _setService.DeleteSetAsync(GetUserId(), setId);
        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}