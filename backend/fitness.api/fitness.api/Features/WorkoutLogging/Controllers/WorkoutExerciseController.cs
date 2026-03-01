using System.Security.Claims;
using fitness.api.Features.WorkoutLogging.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fitness.api.Features.WorkoutLogging.Controllers;

[ApiController]
[Route("workout-sessions/{sessionId:guid}/exercises")]
[Authorize]
public class WorkoutExerciseController : ControllerBase
{
    private readonly IWorkoutExerciseService _exerciseService;

    public WorkoutExerciseController(IWorkoutExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutExerciseResponse>> Add(Guid sessionId, CreateWorkoutExerciseRequest request)
    {
        var response = await _exerciseService.AddExerciseAsync(GetUserId(), sessionId, request);
        return Created(string.Empty, response);
    }

    [HttpPatch("{exerciseId:guid}")]
    public async Task<ActionResult<WorkoutExerciseResponse>> Update(Guid sessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request)
    {
        var response = await _exerciseService.UpdateExerciseAsync(GetUserId(), sessionId, exerciseId, request);
        return Ok(response);
    }

    [HttpDelete("{exerciseId:guid}")]
    public async Task<IActionResult> Delete(Guid sessionId, Guid exerciseId)
    {
        await _exerciseService.DeleteExerciseAsync(GetUserId(), sessionId, exerciseId);
        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}