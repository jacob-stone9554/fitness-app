using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fitness.api.Data.Entities.Workouts;

public class WorkoutSession
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))] 
    public AppUser User { get; set; } = null!;
    
    [Required]
    public DateTimeOffset StartedAt { get; set; }
    
    public DateTimeOffset? EndedAt { get; set; }
    
    [MaxLength(2000)]
    public string? Notes { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    
    public List<WorkoutExercise> Exercises { get; set; } = new();
}