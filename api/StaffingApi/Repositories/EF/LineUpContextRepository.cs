using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Repositories.EF;

public class LineUpContextRepository: ILineUpContextRepository
{
    private readonly LineUpDbContext _db;
    
    public LineUpContextRepository(LineUpDbContext db)
    {
        _db = db;
    }
    
    public async Task<LineUp?> GetAsync(string id)
    {
        return await _db.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id));
    }

    public async Task<IEnumerable<LineUp>> GetBulkAsync(string[] ids)
    {
        var objectIds = ids.Select(ObjectId.Parse).ToList();
        return await _db.LineUps.Where(x => objectIds.Contains(x._id)).ToListAsync();
    }
    
    public async Task<LineUp?> GetByNameAsync(string name)
    {
        return await _db.LineUps.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    public async Task<LineUp> CreateAsync(LineUp element)
    {
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await _db.LineUps.AddAsync(element);
        await _db.SaveChangesAsync();
        return element;
    }

    public async Task<LineUp?> UpdateAsync(LineUp updatedElement)
    {
        var element = await _db.LineUps.FirstOrDefaultAsync(x => x._id == updatedElement._id);
        if (element == null)
            return null;
        element.Modified = DateTime.UtcNow;
        element.Name = updatedElement.Name;
        element.PlayerIds = updatedElement.PlayerIds.ToArray();
        await _db.SaveChangesAsync();
        return element;
    }
    
    public async Task<int> UpdateBulkAsync(IEnumerable<LineUp> updatedElements)
    {
        var elements = await _db.LineUps
            .Where(x => updatedElements.Select(e => e._id).Contains(x._id))
            .ToListAsync();
        var updatedElementsSet = updatedElements.ToDictionary(e => e._id);
        foreach (var element in elements)
        {
            if (!updatedElementsSet.TryGetValue(element._id, out var updatedElement))
                continue;
            element.Modified = DateTime.UtcNow;
            element.Name = updatedElement.Name;
            element.PlayerIds = updatedElement.PlayerIds.ToArray();
        }
        return await _db.SaveChangesAsync();
    }
    
    public async Task<int> DeleteAsync(string id)
    {
        var element = await GetAsync(id);
        if (element == null)
            return 0; 
        _db.LineUps.Remove(element);
        return await _db.SaveChangesAsync();
    }

    public async Task<LineUp?> AddPlayer(string id, string playerId)
    {
        var lineUp = await _db.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id));
        if (lineUp == null || lineUp.PlayerIds.Contains(playerId))
            return null;
        lineUp.PlayerIds = lineUp.PlayerIds.Append(playerId).ToArray();
        lineUp.Modified = DateTime.UtcNow;
        _db.LineUps.Update(lineUp);
        await _db.SaveChangesAsync();
        return lineUp;
    }
    
    public async Task<int> RemovePlayer(string id, string playerId)
    {
        var lineUp = await _db.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id));
        if (lineUp == null || !lineUp.PlayerIds.Contains(playerId))
            return 0;
        lineUp.PlayerIds = lineUp.PlayerIds.Where(x => x != playerId).ToArray();
        lineUp.Modified = DateTime.UtcNow;
        _db.LineUps.Update(lineUp);
        return await _db.SaveChangesAsync();
    }
}