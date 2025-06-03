using StaffingApi.Entities.Bson;

namespace StaffingApi.Services;

public interface ILineUpService
{
    public Task<LineUp?> GetAsync(string id);
    public Task<LineUp> CreateAsync(LineUp lineUp);
}