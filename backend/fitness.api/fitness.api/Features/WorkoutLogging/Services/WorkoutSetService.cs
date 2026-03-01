using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Features.WorkoutLogging.Services;

public interface IWorkoutSetService
{
    Task<WorkoutSetResponse> AddSetAsync(Guid userId, Guid exerciseId, CreateWorkoutSetRequest request);
    Task<WorkoutSetResponse> UpdateSetAsync(Guid userId, Guid setId, UpdateWorkoutSetRequest request);
    Task DeleteSetAsync(Guid userId, Guid setId);
}

public class WorkoutSetService : IWorkoutSetService
{
    private readonly AppDbContext _db;

    public WorkoutSetService(AppDbContext db) => _db = db;

    public async Task<WorkoutSetResponse> AddSetAsync(Guid userId, Guid exerciseId, CreateWorkoutSetRequest request)
    {
        var exerciseExists = await _db.WorkoutExercises
            .Include(e => e.WorkoutSession)
            .AnyAsync(e => e.Id == exerciseId && e.WorkoutSession.UserId == userId);

        if (!exerciseExists)
            throw new NotFoundException($"Exercise {exerciseId} not found.");

        var set = new WorkoutSet
        {
            Id = Guid.NewGuid(),
            WorkoutExerciseId = exerciseId,
            SetNumber = request.SetNumber,
            Reps = request.Reps,
            Weight = request.Weight,
            Rpe = request.Rpe,
            DurationSeconds = request.DurationSeconds,
            DistanceMeters = request.DistanceMeters,
            RestSeconds = request.RestSeconds,
            IsWarmup = request.IsWarmup,
            Notes = request.Notes,
        };

        _db.WorkoutSets.Add(set);
        await _db.SaveChangesAsync();

        return ToResponse(set);
    }

    public async Task<WorkoutSetResponse> UpdateSetAsync(Guid userId, Guid setId, UpdateWorkoutSetRequest request)
    {
        var set = await _db.WorkoutSets
            .Include(s => s.Exercise)
            .ThenInclude(e => e.WorkoutSession)
            .FirstOrDefaultAsync(s => s.Id == setId && s.Exercise.WorkoutSession.UserId == userId);

        if (set is null)
            throw new NotFoundException($"Set {setId} not found.");

        if (request.Reps is not null) set.Reps = request.Reps;
        if (request.Weight is not null) set.Weight = request.Weight;
        if (request.Rpe is not null) set.Rpe = request.Rpe;
        if (request.DurationSeconds is not null) set.DurationSeconds = request.DurationSeconds;
        if (request.DistanceMeters is not null) set.DistanceMeters = request.DistanceMeters;
        if (request.RestSeconds is not null) set.RestSeconds = request.RestSeconds;
        if (request.IsWarmup is not null) set.IsWarmup = request.IsWarmup.Value;
        if (request.Notes is not null) set.Notes = request.Notes;

        await _db.SaveChangesAsync();

        return ToResponse(set);
    }

    public async Task DeleteSetAsync(Guid userId, Guid setId)
    {
        var set = await _db.WorkoutSets
            .Include(s => s.Exercise)
            .ThenInclude(e => e.WorkoutSession)
            .FirstOrDefaultAsync(s => s.Id == setId && s.Exercise.WorkoutSession.UserId == userId);

        if (set is null)
            throw new NotFoundException($"Set {setId} not found.");

        _db.WorkoutSets.Remove(set);
        await _db.SaveChangesAsync();
    }

    private static WorkoutSetResponse ToResponse(WorkoutSet set) =>
        new(
            set.Id,
            set.SetNumber,
            set.Reps,
            set.Weight,
            set.Rpe,
            set.DurationSeconds,
            set.DistanceMeters,
            set.RestSeconds,
            set.IsWarmup,
            set.Notes
        );
}