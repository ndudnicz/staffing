using StaffingApi.Entities.Bson;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class PlayerService(
    IPlayerContextRepository playerRepository,
    ILineUpContextRepository lineUpRepository
    ): IPlayerService
{
    public async Task<PlayerDto?> GetAsync(string id)
    {
        return PlayerDto.FromPlayer(await playerRepository.GetAsync(id));
    }

    public async Task<PlayerDto> CreateAsync(CreatePlayerDto dto)
    {
        return PlayerDto.FromPlayer(await playerRepository.CreateAsync(Player.FromCreatePlayerDto(dto)))!;
    }
    
    public async Task<PlayerDto?> UpdateAsync(PlayerDto dto)
    {
        return PlayerDto.FromPlayer(await playerRepository.UpdateAsync(Player.FromPlayerDto(dto)));
    }
    
    public async Task<int> DeleteAsync(string id)
    {
        var lineUps = (await lineUpRepository.GetBulkAsync(
            (await playerRepository.GetAsync(id))?.LineUpIds ?? []))
            .ToList();
        foreach (var lineUp in lineUps)
        {
            lineUp.PlayerIds = lineUp.PlayerIds.Where(playerId => playerId != id).ToArray();
        }
        await lineUpRepository.UpdateBulkAsync(lineUps);
        return await playerRepository.DeleteAsync(id);
    }
}