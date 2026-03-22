using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Creator;

public class TruckCreator : IModelCreator<Truck>
{
  public static void Setup(ModelBuilder builder, List<Truck> seeding)
  {
    builder.Entity<Truck>(entity =>
        {
          // 1. Cấu hình Properties
          entity.Property(e => e.MaxPayloadKg).HasPrecision(18, 2);
          entity.Property(e => e.OdometerReading).HasPrecision(18, 2);

          // 2. Tenant FK
          entity.HasOne<Tenant>().WithMany().HasForeignKey(t => t.TenantId).OnDelete(DeleteBehavior.Restrict);

          // 3. Cấu hình Index
          entity.HasIndex(t => t.TenantId);
          entity.HasIndex(e => e.LicensePlate).IsUnique();

          // 4. Seed Data
          entity.HasData(seeding);
        });
  }
}