using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WindSync.Core.Models;

namespace WindSync.DAL.DB;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<WindFarm> WindFarms { get; set; }
    public DbSet<Turbine> Turbines { get; set; }
    public DbSet<TurbineData> TurbineData { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
