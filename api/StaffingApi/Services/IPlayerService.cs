using MongoDB.Bson;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Services;

public interface IPlayerService
{
    public Task<PlayerDto?> GetAsync(string id);
    public Task<PlayerDto> CreateAsync(Player player);
}