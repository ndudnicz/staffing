
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;

namespace StaffingApi.Repositories.EF;

public interface ILineUpContextRepository
{
    public Task<LineUp?> GetAsync(string id);
    public Task<IEnumerable<LineUp>> GetBulkAsync(string[] ids);
    public Task<LineUp?> GetByNameAsync(string name);
    public Task<LineUp> CreateAsync(LineUp element);
    public Task<LineUp?> UpdateAsync(LineUp element);
    public Task<int> DeleteAsync(string id);
}