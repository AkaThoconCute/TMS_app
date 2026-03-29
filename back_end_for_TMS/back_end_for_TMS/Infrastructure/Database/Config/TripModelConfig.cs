using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Config;

public class TripModelConfig : IModelConfig<Trip>
{
  public static void Setup(ModelBuilder builder, List<Trip> seeding)
  {
    builder.Entity<Trip>(entity =>
        {
          // 1. Tenant FK
          entity.HasOne<Tenant>().WithMany().HasForeignKey(t => t.TenantId).OnDelete(DeleteBehavior.Restrict);

          // 2. Order FK (configured in OrderModelConfig via HasMany)
          // 3. Truck FK
          entity.HasOne(t => t.Truck).WithMany().HasForeignKey(t => t.TruckId).OnDelete(DeleteBehavior.Restrict);

          // 4. Driver FK
          entity.HasOne(t => t.Driver).WithMany().HasForeignKey(t => t.DriverId).OnDelete(DeleteBehavior.Restrict);

          // 5. Indexes
          entity.HasIndex(t => t.TenantId);
          entity.HasIndex(t => t.OrderId);
          entity.HasIndex(t => new { t.TenantId, t.TripNumber }).IsUnique();

          // 6. Field constraints
          entity.Property(t => t.TripNumber).IsRequired().HasMaxLength(20);
          entity.Property(t => t.CancellationReason).HasMaxLength(500);
          entity.Property(t => t.CostNotes).HasMaxLength(500);
          entity.Property(t => t.FuelCost).HasColumnType("decimal(18,2)");
          entity.Property(t => t.TollCost).HasColumnType("decimal(18,2)");
          entity.Property(t => t.OtherCost).HasColumnType("decimal(18,2)");

          // 7. Seed Data
          entity.HasData(seeding);
        });
  }
}
