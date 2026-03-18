using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Features.WorkoutLogging;
using fitness.api.Features.WorkoutLogging.Services;
using fitness.api.Infrastructure.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Tests.Features.WorkoutLogging;

public class WorkoutExerciseServiceTests
{
    private static AppDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // Seeds a session owned by the given userId and returns its Id.
    private static async Task<Guid> SeedSessionAsync(AppDbContext db, Guid userId)
    {
        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartedAt = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
        };
        db.WorkoutSessions.Add(session);
        await db.SaveChangesAsync();
        return session.Id;
    }

    // Seeds an exercise inside the given session and returns its Id.
    private static async Task<Guid> SeedExerciseAsync(AppDbContext db, Guid sessionId)
    {
        var exercise = new WorkoutExercise
        {
            Id = Guid.NewGuid(),
            WorkoutSessionId = sessionId,
            Name = "Squat",
            Order = 1,
        };
        db.WorkoutExercises.Add(exercise);
        await db.SaveChangesAsync();
        return exercise.Id;
    }

    [Fact]
    public async Task AddExercise_WithValidRequest_ReturnsExercise()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutExerciseService(db);
        var userId = Guid.NewGuid();
        var sessionId = await SeedSessionAsync(db, userId);
        var request = new CreateWorkoutExerciseRequest("Bench Press", 1, "Keep elbows tucked");

        // Act
        var result = await service.AddExerciseAsync(userId, sessionId, request);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be("Bench Press");
        result.Order.Should().Be(1);
        result.Notes.Should().Be("Keep elbows tucked");
        result.Sets.Should().BeEmpty();
    }

    [Fact]
    public async Task AddExercise_ToNonexistentSession_ThrowsNotFoundException()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutExerciseService(db);
        var request = new CreateWorkoutExerciseRequest("Squat", 1, null);

        // Act
        var act = async () => await service.AddExerciseAsync(Guid.NewGuid(), Guid.NewGuid(), request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateExercise_WithValidFields_ReturnsUpdatedExercise()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutExerciseService(db);
        var userId = Guid.NewGuid();
        var sessionId = await SeedSessionAsync(db, userId);
        var exerciseId = await SeedExerciseAsync(db, sessionId);
        var request = new UpdateWorkoutExerciseRequest("Deadlift", 2, "Neutral spine");

        // Act
        var result = await service.UpdateExerciseAsync(userId, sessionId, exerciseId, request);

        // Assert
        result.Id.Should().Be(exerciseId);
        result.Name.Should().Be("Deadlift");
        result.Order.Should().Be(2);
        result.Notes.Should().Be("Neutral spine");
    }

    [Fact]
    public async Task UpdateExercise_OwnedByAnotherUser_ThrowsNotFoundException()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutExerciseService(db);
        var ownerId = Guid.NewGuid();
        var sessionId = await SeedSessionAsync(db, ownerId);
        var exerciseId = await SeedExerciseAsync(db, sessionId);
        var request = new UpdateWorkoutExerciseRequest("Deadlift", null, null);

        // Act
        var act = async () => await service.UpdateExerciseAsync(Guid.NewGuid(), sessionId, exerciseId, request);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeleteExercise_RemovesExerciseFromDatabase()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutExerciseService(db);
        var userId = Guid.NewGuid();
        var sessionId = await SeedSessionAsync(db, userId);
        var exerciseId = await SeedExerciseAsync(db, sessionId);

        // Act
        await service.DeleteExerciseAsync(userId, sessionId, exerciseId);

        // Assert
        var exists = await db.WorkoutExercises.AnyAsync(e => e.Id == exerciseId);
        exists.Should().BeFalse();
    }
}