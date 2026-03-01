using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitness.api.Data.Entities.Workouts;

public class WorkoutExercise
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid WorkoutSessionId { get; set; }

    [ForeignKey(nameof(WorkoutSessionId))] 
    public WorkoutSession WorkoutSession { get; set; } = null!;

    [Required] [MaxLength(120)]
    public string Name { get; set; } = null!;
    
    public int Order { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }

    public List<WorkoutSet> Sets { get; set; } = new();
}