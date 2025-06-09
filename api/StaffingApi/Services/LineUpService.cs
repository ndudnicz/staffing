using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Repositories;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class LineUpService(ILineUpContextRepository repository): ILineUpService
{
    public async Task<LineUpDto?> GetAsync(string id)
    {
        return LineUpDto.FromLineUp(await repository.GetAsync(id));
    }

    public async Task<LineUpDto?> GetByNameAsync(string name)
    {
        return LineUpDto.FromLineUp(await repository.GetByNameAsync(name));
    }
    
    public async Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids)
    {
        return LineUpDto.FromLineUps(await repository.GetBulkAsync(ids));
    }
    
    public async Task<LineUpDto> CreateAsync(CreateLineUpDto dto)
    {
        return LineUpDto.FromLineUp(await repository.CreateAsync(LineUp.FromCreateLineUpDto(dto)))!;
    }
    
    public async Task<LineUpDto?> UpdateAsync(LineUpDto dto)
    {
        return LineUpDto.FromLineUp(await repository.UpdateAsync(LineUp.FromLineUpDto(dto)));
    }

    public async Task<int> DeleteAsync(string id)
    {
        return await repository.DeleteAsync(id);
    }
}