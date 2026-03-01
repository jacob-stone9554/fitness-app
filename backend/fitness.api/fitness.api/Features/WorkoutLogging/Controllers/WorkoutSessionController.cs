using System.Security.Claims;
using fitness.api.Features.WorkoutLogging.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fitness.api.Features.WorkoutLogging.Controllers;

[ApiController]
[Route("workout-sessions")]
[Authorize]
public class WorkoutSessionController : ControllerBase
{
    private readonly IWorkoutSessionService _sessionService;

    public WorkoutSessionController(IWorkoutSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<ActionResult<WorkoutSessionResponse>> Create(CreateWorkoutSessionRequest request)
    {
        var response = await _sessionService.CreateSessionAsync(GetUserId(), request);
        return CreatedAtAction(nameof(GetById), new { sessionId = response.Id }, response);
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkoutSessionSummaryResponse>>> GetAll()
    {
        var sessions = await _sessionService.GetSessionsAsync(GetUserId());
        return Ok(sessions);
    }

    [HttpGet("{sessionId:guid}")]
    public async Task<ActionResult<WorkoutSessionResponse>> GetById(Guid sessionId)
    {
        var session = await _sessionService.GetSessionAsync(GetUserId(), sessionId);
        return Ok(session);
    }

    [HttpPatch("{sessionId:guid}")]
    public async Task<ActionResult<WorkoutSessionResponse>> Update(Guid sessionId, UpdateWorkoutSessionRequest request)
    {
        var session = await _sessionService.UpdateSessionAsync(GetUserId(), sessionId, request);
        return Ok(session);
    }

    [HttpDelete("{sessionId:guid}")]
    public async Task<IActionResult> Delete(Guid sessionId)
    {
        await _sessionService.DeleteSessionAsync(GetUserId(), sessionId);
        return NoContent();
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}

