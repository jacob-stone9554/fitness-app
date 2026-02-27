using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Features.WorkoutLogging.Services;

public interface IWorkoutSessionService
{
    Task<WorkoutSessionResponse> CreateSessionAsync(Guid userId, CreateWorkoutSessionRequest request);
    Task<List<WorkoutSessionSummaryResponse>> GetSessionsAsync(Guid userId);
    Task<WorkoutSessionResponse> GetSessionAsync(Guid userId, Guid sessionId);
    Task<WorkoutSessionResponse> UpdateSessionAsync(Guid userId, Guid sessionId, UpdateWorkoutSessionRequest request);
    Task DeleteSessionAsync(Guid userId, Guid sessionId);
}

public class WorkoutSessionService : IWorkoutSessionService
{
    private readonly AppDbContext _db;

    public WorkoutSessionService(AppDbContext db) => _db = db;

    public async Task<WorkoutSessionResponse> CreateSessionAsync(Guid userId, CreateWorkoutSessionRequest request)
    {
        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartedAt = request.StartedAt ?? DateTimeOffset.UtcNow,
            Notes = request.Notes,
            CreatedAtUtc = DateTime.UtcNow,
        };

        _db.WorkoutSessions.Add(session);
        await _db.SaveChangesAsync();

        return ToDetailResponse(session);
    }

    public async Task<List<WorkoutSessionSummaryResponse>> GetSessionsAsync(Guid userId)
    {
        return await _db.WorkoutSessions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartedAt)
            .Select(s => new WorkoutSessionSummaryResponse(
                s.Id,
                s.StartedAt,
                s.EndedAt,
                s.Notes,
                s.Exercises.Count,
                s.CreatedAtUtc,
                s.UpdatedAtUtc))
            .ToListAsync();
    }

    public async Task<WorkoutSessionResponse> GetSessionAsync(Guid userId, Guid sessionId)
    {
        var session = await _db.WorkoutSessions
            .Include(s => s.Exercises.OrderBy(e => e.Order))
            .ThenInclude(e => e.Sets.OrderBy(set => set.SetNumber))
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);

        if (session is null)
            throw new NotFoundException($"Workout session {sessionId} not found.");

        return ToDetailResponse(session);
    }

    public async Task<WorkoutSessionResponse> UpdateSessionAsync(Guid userId, Guid sessionId, UpdateWorkoutSessionRequest request)
    {
        var session = await _db.WorkoutSessions
            .Include(s => s.Exercises.OrderBy(e => e.Order))
            .ThenInclude(e => e.Sets.OrderBy(set => set.SetNumber))
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);

        if (session is null)
            throw new NotFoundException($"Workout session {sessionId} not found.");

        if (request.EndedAt is not null) session.EndedAt = request.EndedAt;
        if (request.Notes is not null) session.Notes = request.Notes;
        session.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return ToDetailResponse(session);
    }

    public async Task DeleteSessionAsync(Guid userId, Guid sessionId)
    {
        var session = await _db.WorkoutSessions
            .FirstOrDefaultAsync(s => s.Id == sessionId && s.UserId == userId);

        if (session is null)
            throw new NotFoundException($"Workout session {sessionId} not found.");

        _db.WorkoutSessions.Remove(session);
        await _db.SaveChangesAsync();
    }

    private static WorkoutSessionResponse ToDetailResponse(WorkoutSession session) =>
        new(
            session.Id,
            session.StartedAt,
            session.EndedAt,
            session.Notes,
            session.CreatedAtUtc,
            session.UpdatedAtUtc,
            session.Exercises.Select(e => new WorkoutExerciseResponse(
                e.Id,
                e.Name,
                e.Order,
                e.Notes,
                e.Sets.Select(s => new WorkoutSetResponse(
                    s.Id,
                    s.SetNumber,
                    s.Reps,
                    s.Weight,
                    s.Rpe,
                    s.DurationSeconds,
                    s.DistanceMeters,
                    s.RestSeconds,
                    s.IsWarmup,
                    s.Notes)).ToList()
            )).ToList()
        );
}