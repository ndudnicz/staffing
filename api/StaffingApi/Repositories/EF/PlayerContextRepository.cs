using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Repositories.EF;

public class PlayerContextRepository(IMongoConfig mongoConfig): IPlayerContextRepository
{
    private readonly PlayerDbContext _db = PlayerDbContext.Create(mongoConfig);
    public async Task<PlayerDto?> GetAsync(string id)
    {
        return PlayerDto.FromPlayer(await _db.Players.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id)));
    }
    
    public async Task<PlayerDto> CreateAsync(Player element)
    {
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await _db.Players.AddAsync(element);
        await _db.SaveChangesAsync();
        return PlayerDto.FromPlayer(element)!;
    }
}