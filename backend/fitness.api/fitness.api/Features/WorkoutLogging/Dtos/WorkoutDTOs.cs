using System.ComponentModel.DataAnnotations;

namespace fitness.api.Features.WorkoutLogging;

public record CreateWorkoutSessionRequest(
    DateTimeOffset? StartedAt,   // defaults to UtcNow if omitted
    [MaxLength(2000)] string? Notes
);

public record UpdateWorkoutSessionRequest(
    DateTimeOffset? EndedAt,
    [MaxLength(2000)] string? Notes
);

public record WorkoutSessionSummaryResponse(
    Guid Id,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt,
    string? Notes,
    int ExerciseCount,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);

public record WorkoutSessionResponse(
    Guid Id,
    DateTimeOffset StartedAt,
    DateTimeOffset? EndedAt,
    string? Notes,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    List<WorkoutExerciseResponse> Exercises
);

public record CreateWorkoutExerciseRequest(
    [Required] [MaxLength(120)] string Name,
    int Order,
    [MaxLength(1000)] string? Notes
);

public record UpdateWorkoutExerciseRequest(
    [MaxLength(120)] string? Name,
    int? Order,
    [MaxLength(1000)] string? Notes
);

public record WorkoutExerciseResponse(
    Guid Id,
    string Name,
    int Order,
    string? Notes,
    List<WorkoutSetResponse> Sets
);

public record CreateWorkoutSetRequest(
    int SetNumber,
    int? Reps,
    decimal? Weight,
    decimal? Rpe,
    int? DurationSeconds,
    decimal? DistanceMeters,
    int? RestSeconds,
    bool IsWarmup = false,
    [MaxLength(1000)] string? Notes = null
);

public record UpdateWorkoutSetRequest(
    int? Reps,
    decimal? Weight,
    decimal? Rpe,
    int? DurationSeconds,
    decimal? DistanceMeters,
    int? RestSeconds,
    bool? IsWarmup,
    [MaxLength(1000)] string? Notes
);

public record WorkoutSetResponse(
    Guid Id,
    int SetNumber,
    int? Reps,
    decimal? Weight,
    decimal? Rpe,
    int? DurationSeconds,
    decimal? DistanceMeters,
    int? RestSeconds,
    bool IsWarmup,
    string? Notes
);