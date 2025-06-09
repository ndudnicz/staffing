using Microsoft.EntityFrameworkCore;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF.Context;

public interface ILineUpDbContext: IDisposable
{
    public DbSet<LineUp> LineUps { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}