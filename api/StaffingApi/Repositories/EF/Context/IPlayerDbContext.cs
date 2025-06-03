using Microsoft.EntityFrameworkCore;
using StaffingApi.Entities.Bson;

namespace StaffingApi.Repositories.EF.Context;

public interface IPlayerDbContext
{
    public DbSet<Player> Players { get; init; }
}