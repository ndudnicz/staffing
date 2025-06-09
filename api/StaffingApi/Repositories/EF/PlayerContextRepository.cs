using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Repositories.EF;

public class PlayerContextRepository: IPlayerContextRepository
{
    private readonly PlayerDbContext _db;
    
    public PlayerContextRepository(PlayerDbContext db)
    {
        _db = db;
    }
    
    public async Task<Player?> GetAsync(string id)
    {
        return await _db.Players.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id));
    }
    
    public async Task<Player> CreateAsync(Player element)
    {
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await _db.Players.AddAsync(element);
        await _db.SaveChangesAsync();
        return element;
    }
    
    public async Task<Player?> UpdateAsync(Player updatedElement)
    {
        var element = await _db.Players.FirstOrDefaultAsync(x => x._id == updatedElement._id);
        if (element == null)
            return null;
        element.Modified = DateTime.UtcNow;
        element.Name = updatedElement.Name;
        element.LineUpIds = updatedElement.LineUpIds.ToArray();
        await _db.SaveChangesAsync();
        return element;
    }
    
    public async Task<int> DeleteAsync(string id)
    {
        var element = await GetAsync(id);
        if (element == null)
            return 0; 
        _db.Players.Remove(element);
        return await _db.SaveChangesAsync();
    }
}