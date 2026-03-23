using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitness.api.Data.Entities;

public class UserProfile
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public AppUser User { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string Units { get; set; } = "imperial"; // imperial | metric
    
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public int DailyCalorieGoal { get; set; }
    public int Age { get; set; }
    
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}