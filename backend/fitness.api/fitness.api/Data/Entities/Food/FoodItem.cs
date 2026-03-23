using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitness.api.Data.Entities.Food;

public class FoodItem
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser User { get; set; }

    [Required] [MaxLength(120)] 
    public string Name { get; set; } = null!;
    
    [Required]
    public decimal Calories { get; set; }
    
    public decimal Protein { get; set; }
    public decimal Carbohydrates { get; set; }
    public decimal Fats { get; set; }

    [Required]
    public string ServingUnit { get; set; } = null!;
    [Required]
    public decimal ServingSize { get; set; }
}