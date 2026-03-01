using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Features.WorkoutLogging.Services;

public interface IWorkoutExerciseService
{
    Task<WorkoutExerciseResponse> AddExerciseAsync(Guid userId, Guid sessionId, CreateWorkoutExerciseRequest request);
    Task<WorkoutExerciseResponse> UpdateExerciseAsync(Guid userId, Guid sessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request);
    Task DeleteExerciseAsync(Guid userId, Guid sessionId, Guid exerciseId);
}

public class WorkoutExerciseService : IWorkoutExerciseService
{
    private readonly AppDbContext _db;

    public WorkoutExerciseService(AppDbContext db) => _db = db;

    public async Task<WorkoutExerciseResponse> AddExerciseAsync(Guid userId, Guid sessionId, CreateWorkoutExerciseRequest request)
    {
        var sessionExists = await _db.WorkoutSessions
            .AnyAsync(s => s.Id == sessionId && s.UserId == userId);

        if (!sessionExists)
            throw new NotFoundException($"Workout session {sessionId} not found.");

        var exercise = new WorkoutExercise
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = sessionId,
            Name = request.Name,
            Order = request.Order,
            Notes = request.Notes,
        };

        _db.WorkoutExercises.Add(exercise);
        await _db.SaveChangesAsync();

        return ToResponse(exercise);
    }

    public async Task<WorkoutExerciseResponse> UpdateExerciseAsync(Guid userId, Guid sessionId, Guid exerciseId, UpdateWorkoutExerciseRequest request)
    {
        var exercise = await _db.WorkoutExercises
            .Include(e => e.Sets.OrderBy(s => s.SetNumber))
            .Include(e => e.WorkoutSession)
            .FirstOrDefaultAsync(e => e.Id == exerciseId
                                      && e.WorkoutSessionId == sessionId
                                      && e.WorkoutSession.UserId == userId);

        if (exercise is null)
            throw new NotFoundException($"Exercise {exerciseId} not found.");

        if (request.Name is not null) exercise.Name = request.Name;
        if (request.Order is not null) exercise.Order = request.Order.Value;
        if (request.Notes is not null) exercise.Notes = request.Notes;

        await _db.SaveChangesAsync();

        return ToResponse(exercise);
    }

    public async Task DeleteExerciseAsync(Guid userId, Guid sessionId, Guid exerciseId)
    {
        var exercise = await _db.WorkoutExercises
            .Include(e => e.WorkoutSession)
            .FirstOrDefaultAsync(e => e.Id == exerciseId
                                      && e.WorkoutSessionId == sessionId
                                      && e.WorkoutSession.UserId == userId);

        if (exercise is null)
            throw new NotFoundException($"Exercise {exerciseId} not found.");

        _db.WorkoutExercises.Remove(exercise);
        await _db.SaveChangesAsync();
    }

    private static WorkoutExerciseResponse ToResponse(WorkoutExercise exercise) =>
        new(
            exercise.Id,
            exercise.Name,
            exercise.Order,
            exercise.Notes,
            exercise.Sets.Select(s => new WorkoutSetResponse(
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
        );
}