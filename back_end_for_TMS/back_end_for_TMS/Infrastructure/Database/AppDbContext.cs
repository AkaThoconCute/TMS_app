using System.Linq.Expressions;
using back_end_for_TMS.Business.Context;
using back_end_for_TMS.Infrastructure.Database.Config;
using back_end_for_TMS.Infrastructure.Database.Seeder;
using back_end_for_TMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config, ITenantContext tenantContext) : IdentityDbContext<AppUser>(options)
{
  private readonly ITenantContext _tenantContext = tenantContext;
  private Guid CurrentTenantId => _tenantContext.TenantId;
  public DbSet<Tenant> Tenants { get; set; } = default!;
  public DbSet<Truck> Trucks { get; set; } = default!;
  public DbSet<Driver> Drivers { get; set; } = default!;

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
    TenantModelConfig.Setup(builder, tenants);

    // Setup Identity models and seeding
    List<AppUser> users = AppUserDataSeeder.Generate();
    AppUserModelConfig.Setup(builder, users);

    List<IdentityRole> roles = RoleDataSeeder.Generate();
    RoleModelConfig.Setup(builder, roles);

    List<IdentityUserRole<string>> user_roles = UserRoleDataSeeder.Generate();
    UserRoleModelConfig.Setup(builder, user_roles);

    // Setup Truck model and seeding
    List<Truck> trucks = TruckDataSeeder.Generate();
    TruckModelConfig.Setup(builder, trucks);

    // Setup Driver model and seeding
    List<Driver> drivers = DriverDataSeeder.Generate();
    DriverModelConfig.Setup(builder, drivers);

    // Multi-tenant: Auto-apply query filter to all ITenantEntity entities
    foreach (var entityType in builder.Model.GetEntityTypes())
    {
      if (!typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
        continue;

      var parameter = Expression.Parameter(entityType.ClrType, "e");
      var tenantIdProp = Expression.Property(parameter, nameof(ITenantEntity.TenantId));
      var currentTenantId = Expression.Property(Expression.Constant(this), nameof(CurrentTenantId));
      var body = Expression.Equal(tenantIdProp, currentTenantId);
      var filter = Expression.Lambda(body, parameter);
      builder.Entity(entityType.ClrType).HasQueryFilter(filter);
    }
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    foreach (var entry in ChangeTracker.Entries<ITenantEntity>())
    {
      if (entry.State == EntityState.Added)
      {
        entry.Entity.TenantId = _tenantContext.TenantId;
      }
    }

    return base.SaveChangesAsync(cancellationToken);
  }
}
