using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;
using StaffingApi.Repositories;
using StaffingApi.Repositories.EF;

namespace StaffingApi.Services;

public class LineUpService(ILineUpContextRepository repository): ILineUpService
{
    public async Task<LineUpDto?> GetAsync(string id)
    {
        return await repository.GetAsync(id);
    }

    public async Task<LineUpDto?> GetByNameAsync(string name)
    {
        return await repository.GetByNameAsync(name);
    }
    
    public async Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids)
    {
        return await repository.GetBulkAsync(ids);
    }
    
    public Task<LineUpDto> CreateAsync(LineUp lineUp)
    {
        return repository.CreateAsync(lineUp);
    }
}