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
    
    public async Task<LineUpDto?> GetAsync(string id)
    {
        return LineUpDto.FromLineUp(await _db.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id)));
    }

    public async Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids)
    {
        var objectIds = ids.Select(ObjectId.Parse).ToList();
        return LineUpDto.FromLineUps(await _db.LineUps.Where(x => objectIds.Contains(x._id)).ToListAsync());
    }
    
    public async Task<LineUpDto?> GetByNameAsync(string name)
    {
        return LineUpDto.FromLineUp(await _db.LineUps.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
    }
    
    public async Task<LineUpDto> CreateAsync(LineUp element)
    {
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await _db.LineUps.AddAsync(element);
        await _db.SaveChangesAsync();
        return LineUpDto.FromLineUp(element)!;
    }
}