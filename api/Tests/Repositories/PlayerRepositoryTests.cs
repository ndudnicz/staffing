using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;
using StaffingApi.Repositories.EF.Context;
using FluentAssertions;

namespace StaffingApi.Tests.Repositories;

public class PlayerContextRepositoryTests
{
    private static PlayerDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<PlayerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new PlayerDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    [Fact]
    public async Task CreateAsync_ShouldInsertPlayerAndReturn()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new PlayerContextRepository(context);
        var player = new Player { Name = "Test Player", LineUpIds = [] };

        // Act
        var result = await repository.CreateAsync(player);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Player");
        result.Id.Should().NotBeNullOrEmpty();
        context.Players.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetAsync_ShouldReturn_WhenIdExists()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var player = new Player
        {
            _id = ObjectId.GenerateNewId(),
            Name = "Test Player",
            Created = DateTime.UtcNow,
            LineUpIds = []
        };
        context.Players.Add(player);
        await context.SaveChangesAsync();

        var repository = new PlayerContextRepository(context);

        // Act
        var result = await repository.GetAsync(player._id.ToString());

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(player._id.ToString());
        result.Name.Should().Be("Test Player");
    }

    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new PlayerContextRepository(context);
        var fakeId = ObjectId.GenerateNewId().ToString();

        // Act
        var result = await repository.GetAsync(fakeId);

        // Assert
        result.Should().BeNull();
    }
}
