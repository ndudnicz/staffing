using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class PlayerService(IPlayerContextRepository repository): IPlayerService
{
    public async Task<PlayerDto?> GetAsync(string id)
    {
        return PlayerDto.FromPlayer(await repository.GetAsync(id));
    }

    public async Task<PlayerDto> CreateAsync(CreatePlayerDto dto)
    {
        return PlayerDto.FromPlayer(await repository.CreateAsync(Player.FromCreatePlayerDto(dto)))!;
    }
    
    public async Task<PlayerDto?> UpdateAsync(PlayerDto dto)
    {
        return PlayerDto.FromPlayer(await repository.UpdateAsync(Player.FromPlayerDto(dto)));
    }
    
    public async Task<int> DeleteAsync(string id)
    {
        return await repository.DeleteAsync(id);
    }
}