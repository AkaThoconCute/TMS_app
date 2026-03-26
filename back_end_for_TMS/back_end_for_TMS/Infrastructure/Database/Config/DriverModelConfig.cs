using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Config;

public class DriverModelConfig : IModelConfig<Driver>
{
  public static void Setup(ModelBuilder builder, List<Driver> seeding)
  {
    builder.Entity<Driver>(entity =>
        {
          // 1. Tenant FK
          entity.HasOne<Tenant>().WithMany().HasForeignKey(d => d.TenantId).OnDelete(DeleteBehavior.Restrict);

          // 2. Add Index
          entity.HasIndex(d => d.TenantId);

          // 3. Seed Data
          entity.HasData(seeding);
        });
  }
}
