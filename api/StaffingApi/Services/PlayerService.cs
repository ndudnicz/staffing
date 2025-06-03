using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class PlayerService(IPlayerContextRepository repository): IPlayerService
{
    public async Task<PlayerDto?> GetAsync(string id)
    {
        return await repository.GetAsync(id);
    }

    public async Task<PlayerDto> CreateAsync(Player player)
    {
        return await repository.CreateAsync(player);
    }
}