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
    
    public async Task<int> DeleteAsync(string id)
    {
        var element = await GetAsync(id);
        if (element == null)
            return 0; 
        _db.LineUps.Remove(element);
        return await _db.SaveChangesAsync();
    }
}