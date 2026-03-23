using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitness.api.Data.Entities.Food;

public class FoodLog
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Servings must be greater than 0.")]
    public decimal Servings { get; set; }
    
    [Required]
    public DateTime LoggedAt { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
}