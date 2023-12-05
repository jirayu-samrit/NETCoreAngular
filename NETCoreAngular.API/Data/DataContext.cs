using Microsoft.EntityFrameworkCore;
using NETCoreAngular.API.Entities;

namespace NETCoreAngular.API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
}
