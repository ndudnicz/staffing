
using MongoDB.Bson;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF;

public interface IPlayerContextRepository
{
    public Task<Player?> GetAsync(string id);
    public Task<IEnumerable<Player>> GetBulkAsync(string[] ids);
    public Task<Player> CreateAsync(Player element);
    public Task<Player?> UpdateAsync(Player element);
    public Task<int> UpdateBulkAsync(IEnumerable<Player> elements);
    public Task<int> DeleteAsync(string id);
}