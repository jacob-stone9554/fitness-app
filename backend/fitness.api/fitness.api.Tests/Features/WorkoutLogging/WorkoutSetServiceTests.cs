using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Features.WorkoutLogging;
using fitness.api.Features.WorkoutLogging.Services;
using fitness.api.Infrastructure.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Tests.Features.WorkoutLogging;

public class WorkoutSetServiceTests
{
    private static AppDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // Seeds a session + exercise owned by the given userId and returns the exerciseId.
    private static async Task<Guid> SeedExerciseAsync(AppDbContext db, Guid userId)
    {
        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartedAt = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
        };
        db.WorkoutSessions.Add(session);

        var exercise = new WorkoutExercise
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = session.Id,
            Name = "Squat",
            Order = 1,
        };
        db.WorkoutExercises.Add(exercise);
        await db.SaveChangesAsync();
        return exercise.Id;
    }

    // Seeds a set inside the given exercise and returns its Id.
    private static async Task<Guid> SeedSetAsync(AppDbContext db, Guid exerciseId)
    {
        var set = new WorkoutSet
        {
            Id = Guid.NewGuid(),
            WorkoutExerciseId = exerciseId,
            SetNumber = 1,
            Reps = 5,
            Weight = 100m,
        };
        db.WorkoutSets.Add(set);
        await db.SaveChangesAsync();
        return set.Id;
    }

    [Fact]
    public async Task AddSet_WithValidRequest_ReturnsSet()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSetService(db);
        var userId = Guid.NewGuid();
        var exerciseId = await SeedExerciseAsync(db, userId);
        var request = new CreateWorkoutSetRequest(1, 5, 100m, 8.5m, null, null, 90, false, "Felt strong");

        // Act
        var result = await service.AddSetAsync(userId, exerciseId, request);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.SetNumber.Should().Be(1);
        result.Reps.Should().Be(5);
        result.Weight.Should().Be(100m);
        result.Rpe.Should().Be(8.5m);
        result.RestSeconds.Should().Be(90);
        result.IsWarmup.Should().BeFalse();
        result.Notes.Should().Be("Felt strong");
    }

    [Fact]
    public async Task AddSet_ToNonexistentExercise_ThrowsNotFoundException()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSetService(db);
        var request = new CreateWorkoutSetRequest(1, 5, 100m, null, null, null, null);

        // Act
        var act = async () => await service.AddSetAsync(Guid.NewGuid(), Guid.NewGuid(), request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateSet_WithValidFields_ReturnsUpdatedSet()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSetService(db);
        var userId = Guid.NewGuid();
        var exerciseId = await SeedExerciseAsync(db, userId);
        var setId = await SeedSetAsync(db, exerciseId);
        var request = new UpdateWorkoutSetRequest(8, 120m, 9m, null, null, 120, null, "New PR");

        // Act
        var result = await service.UpdateSetAsync(userId, setId, request);

        // Assert
        result.Id.Should().Be(setId);
        result.Reps.Should().Be(8);
        result.Weight.Should().Be(120m);
        result.Rpe.Should().Be(9m);
        result.RestSeconds.Should().Be(120);
        result.Notes.Should().Be("New PR");
    }

    [Fact]
    public async Task UpdateSet_OwnedByAnotherUser_ThrowsNotFoundException()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSetService(db);
        var ownerId = Guid.NewGuid();
        var exerciseId = await SeedExerciseAsync(db, ownerId);
        var setId = await SeedSetAsync(db, exerciseId);
        var request = new UpdateWorkoutSetRequest(10, null, null, null, null, null, null, null);

        // Act
        var act = async () => await service.UpdateSetAsync(Guid.NewGuid(), setId, request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteSet_RemovesSetFromDatabase()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSetService(db);
        var userId = Guid.NewGuid();
        var exerciseId = await SeedExerciseAsync(db, userId);
        var setId = await SeedSetAsync(db, exerciseId);

        // Act
        await service.DeleteSetAsync(userId, setId);

        // Assert
        var exists = await db.WorkoutSets.AnyAsync(s => s.Id == setId);
        exists.Should().BeFalse();
    }
}