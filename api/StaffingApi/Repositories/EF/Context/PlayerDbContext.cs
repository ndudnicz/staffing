using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF.Config;

namespace StaffingApi.Repositories.EF.Context;

public class PlayerDbContext(DbContextOptions options) : DbContext(options), IPlayerDbContext
{
    public DbSet<Player> Players { get; init; }
    private const string CollectionName = "players";
    
    public static PlayerDbContext Create(IMongoConfig mongoConfig) =>
        new(new DbContextOptionsBuilder<PlayerDbContext>()
            .UseMongoDB(mongoConfig.MongoClient, mongoConfig.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Player>().ToCollection(CollectionName);
    }
}