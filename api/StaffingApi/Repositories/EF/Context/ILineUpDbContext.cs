using Microsoft.EntityFrameworkCore;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF.Context;

public interface ILineUpDbContext
{
    public DbSet<LineUp> LineUps { get; init; }
}