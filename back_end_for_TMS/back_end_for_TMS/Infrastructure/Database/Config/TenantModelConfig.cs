using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Config;

public class TenantModelConfig : IModelConfig<Tenant>
{
  public static void Setup(ModelBuilder builder, List<Tenant> seeding)
  {
    builder.Entity<Tenant>(entity =>
    {
      // Identity (It is optionally explicit since EF core will automatically detect it as PK)
      entity.HasKey(e => e.TenantId);

      // Properties
      entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
      entity.Property(e => e.OwnerId).IsRequired();

      // Seed Data
      entity.HasData(seeding);
    });
  }
}