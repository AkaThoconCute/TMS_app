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

    // Seed Identity Data
    var (roles, users, userRoles) = IdentityDataSeeder.GenerateIdentityData();
    builder.Entity<IdentityRole>().HasData(roles);
    builder.Entity<AppUser>().HasData(users);
    builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

    // AppUser → Tenant relationship
    builder.Entity<AppUser>(entity =>
        {
          entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.SetNull);

          entity.HasIndex(u => u.TenantId);
        });

    // Tenant config
    builder.Entity<Tenant>(entity =>
        {
          entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

          entity.Property(e => e.OwnerId)
            .IsRequired();

          entity.HasIndex(e => e.OwnerId);
        });

    // Seed Truck Data
    var trucks = TruckDataSeeder.Generate();
    builder.Entity<Truck>(entity =>
        {
          // 1. Cấu hình Precision
          entity.Property(e => e.MaxPayloadKg)
            .HasPrecision(18, 2);

          entity.Property(e => e.OdometerReading)
            .HasPrecision(18, 2);

          // 2. Cấu hình Index
          entity.HasIndex(e => e.LicensePlate)
            .IsUnique();

          // 3. Seed Data (Phải nằm trong khối entity này)
          entity.HasData(trucks);
        });
  }
}
