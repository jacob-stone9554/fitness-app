using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitness.api.Data.Entities.Workouts;

public class WorkoutSet
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid WorkoutExerciseId { get; set; }

    [ForeignKey(nameof(WorkoutExerciseId))]
    public WorkoutExercise Exercise { get; set; } = null!;
    
    [Required]
    public int SetNumber { get; set; }
    
    public int? Reps { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Rpe { get; set; }
    
    public int? DurationSeconds { get; set; }
    public decimal? DistanceMeters { get; set; }
    
    public int? RestSeconds { get; set; }
    public bool IsWarmup { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}