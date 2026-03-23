using fitness.api.Data.Entities;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Data.Entities.Food;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<WorkoutSet> WorkoutSets => Set<WorkoutSet>();
    public DbSet<FoodLog> FoodLogs => Set<FoodLog>();
    public DbSet<FoodItem> FoodItems => Set<FoodItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasIndex(p => p.UserId).IsUnique();

            entity.HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(p => p.DisplayName).HasMaxLength(100).IsRequired();
             entity.Property(p => p.Units).HasMaxLength(10).IsRequired();
         });


        modelBuilder.Entity<WorkoutSession>(entity =>
        {
            entity.HasIndex(session => new { session.UserId, session.StartedAt });

            entity.HasOne(session => session.User)
                .WithMany()
                .HasForeignKey(session => session.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(session => session.Notes).HasMaxLength(2000);
            
            entity.HasMany(session => session.Exercises)
                .WithOne(exercise => exercise.WorkoutSession)
                .HasForeignKey(exercise => exercise.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.Property(exercise => exercise.Name).HasMaxLength(120).IsRequired();
            entity.Property(exercise => exercise.Notes).HasMaxLength(1000);

            entity.HasIndex(exercise => new { exercise.WorkoutSessionId, exercise.Order });
                
            entity.HasMany(exercise => exercise.Sets)
                .WithOne(set => set.Exercise)
                .HasForeignKey(set => set.WorkoutExerciseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkoutSet>(entity =>
        {
            entity.Property(set => set.Weight).HasPrecision(10, 2);
            entity.Property(set => set.Rpe).HasPrecision(3, 1);
            entity.Property(set => set.DistanceMeters).HasPrecision(10, 2);
            
            entity.Property(set => set.Notes).HasMaxLength(1000);

            entity.HasIndex(set => new { set.WorkoutExerciseId, set.SetNumber });
        });

        modelBuilder.Entity<FoodItem>(entity =>
        {
            entity.HasOne(item => item.User)
                .WithMany()
                .HasForeignKey(item => item.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(item => item.Name).HasMaxLength(120).IsRequired();
            entity.Property(item => item.ServingUnit).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<FoodLog>(entity =>
        {
            entity.HasOne(log => log.FoodItem)
                .WithMany()
                .HasForeignKey(log => log.FoodItemId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(log => log.User)
                .WithMany()
                .HasForeignKey(log => log.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
 }
