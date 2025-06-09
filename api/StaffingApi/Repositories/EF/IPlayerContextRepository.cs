
using MongoDB.Bson;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF;

public interface IPlayerContextRepository
{
    public Task<Player?> GetAsync(string id);
    public Task<Player> CreateAsync(Player element);
    public Task<Player?> UpdateAsync(Player element);
    public Task<int> DeleteAsync(string id);
}