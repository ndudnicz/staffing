
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;

namespace StaffingApi.Repositories.EF;

public interface ILineUpContextRepository
{
    // public string CollectionName { get; }
    public Task<LineUpDto?> GetAsync(string id);
    public Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids);
    public Task<LineUpDto?> GetByNameAsync(string name);
    public Task<LineUpDto> CreateAsync(LineUp element);
    // public Task<Player> UpdateAsync(Player element);
    // public Task<bool> DeleteAsync(string id);
}