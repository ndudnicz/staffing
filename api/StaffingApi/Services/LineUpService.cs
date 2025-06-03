using StaffingApi.Entities.Bson;
using StaffingApi.Repositories;

namespace StaffingApi.Services;

public class LineUpService(ILineUpRepository repository): ILineUpService
{
    public async Task<LineUp?> GetAsync(string id)
    {
        return await repository.GetAsync(id);
    }

    public Task<LineUp> CreateAsync(LineUp lineUp)
    {
        return repository.CreateAsync(lineUp);
    }
}