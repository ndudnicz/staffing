using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Repositories.EF;

public class PlayerContextRepository(IMongoConfig mongoConfig): IPlayerContextRepository
{
    public async Task<PlayerDto?> GetAsync(string id)
    {
        await using var db = PlayerDbContext.Create(mongoConfig);
        return PlayerDto.FromPlayer(await db.Players.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id)));
    }
    
    public async Task<PlayerDto> CreateAsync(Player element)
    {
        await using var db = PlayerDbContext.Create(mongoConfig);
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await db.Players.AddAsync(element);
        await db.SaveChangesAsync();
        return PlayerDto.FromPlayer(element)!;
    }
}