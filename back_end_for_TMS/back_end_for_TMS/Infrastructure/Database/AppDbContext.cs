using back_end_for_TMS.Infrastructure.Database.Creator;
using back_end_for_TMS.Infrastructure.Database.Seeder;
using back_end_for_TMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : IdentityDbContext<AppUser>(options)
{
  public DbSet<Tenant> Tenants { get; set; } = default!;
  public DbSet<Truck> Trucks { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
    if (builder.IsConfigured)
      return;

    var connectionString = config.GetConnectionString("Database");
    if (string.IsNullOrEmpty(connectionString))
      throw new InvalidOperationException("ConnectionStrings.Database is required in appsettings.json");

    builder.UseSqlServer(connectionString);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Setup Tenant model and seeding
    List<Tenant> tenants = TenantDataSeeder.Generate();
    var firstTenantId = tenants.First().TenantId;
    TenantCreator.Setup(builder, tenants);


    // Setup Identity models and seeding
    var (roles, users, userRoles) = IdentityDataSeeder.GenerateIdentityData(); // (firstTenantId);
    builder.Entity<IdentityRole>().HasData(roles);

    builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

    builder.Entity<AppUser>(entity =>
    {
      entity.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.SetNull);

      entity.HasIndex(u => u.TenantId);

      entity.HasData(users);
    });

    // Setup Truck model and seeding
    List<Truck> trucks = TruckDataSeeder.Generate(); // (firstTenantId);
    TruckCreator.Setup(builder, trucks);
  }
}
