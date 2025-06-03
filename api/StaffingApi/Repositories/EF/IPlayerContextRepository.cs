
using MongoDB.Bson;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF;

public interface IPlayerContextRepository
{
    // public string CollectionName { get; }
    public Task<PlayerDto?> GetAsync(string id);
    public Task<PlayerDto> CreateAsync(Player element);
    // public Task<Player> UpdateAsync(Player element);
    // public Task<bool> DeleteAsync(string id);
}