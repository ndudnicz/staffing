using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Tests.Repositories
{
    public class LineUpContextRepositoryTests
    {
        private LineUpDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<LineUpDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new LineUpDbContext(options);
        }

        private LineUp CreateSampleLineUp(string name = "Team Alpha") =>
            new()
            {
                _id = ObjectId.GenerateNewId(),
                Name = name,
                Created = DateTime.UtcNow,
                PlayerIds = []
            };

        [Fact]
        public async Task GetAsync_ShouldReturn_WhenIdMatches()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateInMemoryContext(dbName);
            var lineUp = CreateSampleLineUp();
            context.LineUps.Add(lineUp);
            await context.SaveChangesAsync();

            var repo = new LineUpContextRepository(context);

            // Act
            var result = await repo.GetAsync(lineUp._id.ToString());

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(lineUp.Name);
        }

        [Fact]
        public async Task GetBulkAsync_ShouldReturn_WhenIdsMatch()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateInMemoryContext(dbName);
            var lineUps = new[]
            {
                CreateSampleLineUp("Team 1"),
                CreateSampleLineUp("Team 2")
            };
            await context.LineUps.AddRangeAsync(lineUps);
            await context.SaveChangesAsync();

            var repo = new LineUpContextRepository(context);
            var ids = lineUps.Select(x => x._id.ToString()).ToArray();

            // Act
            var result = (await repo.GetBulkAsync(ids)).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Select(x => x.Name).Should().Contain(new[] { "Team 1", "Team 2" });
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturn_WhenNameMatches()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateInMemoryContext(dbName);
            var lineUp = CreateSampleLineUp("Best Team");
            await context.LineUps.AddAsync(lineUp);
            await context.SaveChangesAsync();

            var repo = new LineUpContextRepository(context);

            // Act
            var result = await repo.GetByNameAsync("best team");

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Best Team");
        }

        [Fact]
        public async Task CreateAsync_ShouldInsertLineUpAndReturn()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = CreateInMemoryContext(dbName);
            var repo = new LineUpContextRepository(context);

            var newLineUp = new LineUp
            {
                Name = "Created Team",
                PlayerIds = []
            };

            // Act
            var result = await repo.CreateAsync(newLineUp);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Created Team");

            var inserted = await context.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(result.Id));
            inserted.Should().NotBeNull();
            inserted.Created.Should().NotBe(default);
        }
    }
}
