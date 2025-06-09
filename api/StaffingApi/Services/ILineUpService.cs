using StaffingApi.Entities.Bson;
using StaffingApi.Entities.Dto;

namespace StaffingApi.Services;

public interface ILineUpService
{
    public Task<LineUpDto?> GetAsync(string id);
    public Task<IEnumerable<LineUpDto>> GetBulkAsync(string[] ids);
    public Task<LineUpDto?> GetByNameAsync(string name);
    public Task<LineUpDto> CreateAsync(CreateLineUpDto dto);
    public Task<LineUpDto?> UpdateAsync(LineUpDto dto);
    public Task<int> DeleteAsync(string id);
}