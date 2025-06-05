using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF.Config;

namespace StaffingApi.Repositories.EF.Context;

public class LineUpDbContext(DbContextOptions options) : DbContext(options), ILineUpDbContext
{
    public DbSet<LineUp> LineUps { get; init; }
    private const string CollectionName = "lineups";
    
    public static LineUpDbContext Create(IMongoConfig mongoConfig) =>
        new(new DbContextOptionsBuilder<LineUpDbContext>()
            .UseMongoDB(mongoConfig.MongoClient, mongoConfig.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<LineUp>().ToCollection(CollectionName);
    }
}