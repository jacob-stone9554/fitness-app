using fitness.api.Data;
using fitness.api.Data.Entities.Workouts;
using fitness.api.Features.WorkoutLogging;
using fitness.api.Features.WorkoutLogging.Services;
using fitness.api.Infrastructure.Errors;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace fitness.api.Tests.Features.WorkoutLogging;

public class WorkoutSessionServiceTests
{
    // Each test gets a fresh in-memory database to keep tests isolated.
    private static AppDbContext CreateDb() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task CreateSession_WithValidRequest_ReturnsSession()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSessionService(db);
        var userId = Guid.NewGuid();
        var request = new CreateWorkoutSessionRequest(DateTimeOffset.UtcNow, "Leg day");

        // Act
        var result = await service.CreateSessionAsync(userId, request);

        // Assert
        result.Id.Should().NotBeEmpty();
        result.StartedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(5));
        result.Notes.Should().Be("Leg day");
        result.Exercises.Should().BeEmpty();
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
    public async Task GetSession_OwnedByAnotherUser_ThrowsNotFoundException()
    {
        // Arrange
        var db = CreateDb();
        var service = new WorkoutSessionService(db);
        var ownerId = Guid.NewGuid();
        var intruderId = Guid.NewGuid();

        var session = new WorkoutSession
        {
            Id = Guid.NewGuid(),
            UserId = ownerId,
            StartedAt = DateTimeOffset.UtcNow,
            CreatedAtUtc = DateTime.UtcNow,
        };
        db.WorkoutSessions.Add(session);
        await db.SaveChangesAsync();

        // Act
        var act = async () => await service.GetSessionAsync(intruderId, session.Id);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}