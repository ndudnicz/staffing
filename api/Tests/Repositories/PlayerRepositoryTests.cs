using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Tests.Repositories
{
    public class PlayerContextRepositoryTests
    {
        private static PlayerDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PlayerDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB per test
                .Options;

            var dbContext = new PlayerDbContext(options);
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertPlayerAndReturnDto()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PlayerContextRepository(context);
            var player = new Player { Name = "Test Player", LineUpIds = [] };

            // Act
            var result = await repository.CreateAsync(player);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Player", result.Name);
            Assert.NotNull(result.Id);
            Assert.Equal(1, context.Players.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDto_WhenIdExists()
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
            Assert.NotNull(result);
            Assert.Equal(player._id.ToString(), result!.Id);
            Assert.Equal("Test Player", result.Name);
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
            Assert.Null(result);
        }
    }
}
