using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class LineUpService(
    ILineUpContextRepository lineUpRepository,
    IPlayerContextRepository playerRepository
    ): ILineUpService
{
    public async Task<LineUpDto?> GetAsync(string id)
    {
        return LineUpDto.FromLineUp(await lineUpRepository.GetAsync(id));
    }

    public async Task<LineUpDto?> GetByNameAsync(string name)
    {
        return LineUpDto.FromLineUp(await lineUpRepository.GetByNameAsync(name));
    }
    
    public async Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids)
    {
        return LineUpDto.FromLineUps(await lineUpRepository.GetBulkAsync(ids));
    }
    
    public async Task<LineUpDto> CreateAsync(CreateLineUpDto dto)
    {
        return LineUpDto.FromLineUp(await lineUpRepository.CreateAsync(LineUp.FromCreateLineUpDto(dto)))!;
    }
    
    public async Task<LineUpDto?> UpdateAsync(LineUpDto dto)
    {
        return LineUpDto.FromLineUp(await lineUpRepository.UpdateAsync(LineUp.FromLineUpDto(dto)));
    }

    public async Task<int> DeleteAsync(string id)
    {
        var players = (await playerRepository.GetBulkAsync(
            (await lineUpRepository.GetAsync(id))?.PlayerIds ?? []))
            .ToList();
        foreach (var player in players)
        {
            player.LineUpIds = player.LineUpIds.Where(lineUpId => lineUpId != id).ToArray();
        }
        await playerRepository.UpdateBulkAsync(players);
        return await lineUpRepository.DeleteAsync(id);
    }

    public async Task<LineUpDto?> AddPlayerAsync(LineUpDto existingLineUp, PlayerDto existingPlayer)
    {
        existingLineUp.PlayerIds = existingLineUp.PlayerIds.Append(existingPlayer.Id).ToArray();
        existingLineUp = LineUpDto.FromLineUp(await lineUpRepository.UpdateAsync(LineUp.FromLineUpDto(existingLineUp)));
        
        existingPlayer.LineUpIds = existingPlayer.LineUpIds.Append(existingLineUp!.Id).ToArray();
        await playerRepository.UpdateAsync(Player.FromPlayerDto(existingPlayer));
        return existingLineUp;
    }

    public async Task<LineUpDto?> RemovePlayerAsync(LineUpDto existingLineUp, PlayerDto existingPlayer)
    {
        existingLineUp.PlayerIds = existingLineUp.PlayerIds.Where(id => id != existingPlayer.Id).ToArray();
        existingLineUp = LineUpDto.FromLineUp(await lineUpRepository.UpdateAsync(LineUp.FromLineUpDto(existingLineUp)));
        
        existingPlayer.LineUpIds = existingPlayer.LineUpIds.Where(id => id != existingLineUp!.Id).ToArray();
        await playerRepository.UpdateAsync(Player.FromPlayerDto(existingPlayer));
        return existingLineUp;
    }
}