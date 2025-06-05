using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Repositories.EF.Config;
using StaffingApi.Repositories.EF.Context;

namespace StaffingApi.Repositories.EF;

public class LineUpContextRepository(IMongoConfig mongoConfig): ILineUpContextRepository
{
    public async Task<LineUpDto?> GetAsync(string id)
    {
        await using var db = LineUpDbContext.Create(mongoConfig);
        return LineUpDto.FromLineUp(await db.LineUps.FirstOrDefaultAsync(x => x._id == ObjectId.Parse(id)));
    }

    public async Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids)
    {
        await using var db = LineUpDbContext.Create(mongoConfig);
        var objectIds = ids.Select(ObjectId.Parse).ToList();
        return LineUpDto.FromLineUps(await db.LineUps.Where(x => objectIds.Contains(x._id)).ToListAsync());
    }
    
    public async Task<LineUpDto?> GetByNameAsync(string name)
    {
        await using var db = LineUpDbContext.Create(mongoConfig);
        return LineUpDto.FromLineUp(await db.LineUps.FirstOrDefaultAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)));
    }
    
    public async Task<LineUpDto> CreateAsync(LineUp element)
    {
        await using var db = LineUpDbContext.Create(mongoConfig);
        element._id = ObjectId.GenerateNewId();
        element.Created = DateTime.UtcNow;
        element.Modified = null;
        await db.LineUps.AddAsync(element);
        await db.SaveChangesAsync();
        return LineUpDto.FromLineUp(element)!;
    }
}