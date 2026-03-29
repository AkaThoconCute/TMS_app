using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Config;

public class OrderModelConfig : IModelConfig<Order>
{
  public static void Setup(ModelBuilder builder, List<Order> seeding)
  {
    builder.Entity<Order>(entity =>
        {
          // 1. Tenant FK
          entity.HasOne<Tenant>().WithMany().HasForeignKey(o => o.TenantId).OnDelete(DeleteBehavior.Restrict);

          // 2. Customer FK
          entity.HasOne(o => o.Customer).WithMany().HasForeignKey(o => o.CustomerId).OnDelete(DeleteBehavior.Restrict);

          // 3. One-to-Many: Order → Trips
          entity.HasMany(o => o.Trips).WithOne(t => t.Order).HasForeignKey(t => t.OrderId).OnDelete(DeleteBehavior.Cascade);

          // 4. Indexes
          entity.HasIndex(o => o.TenantId);
          entity.HasIndex(o => new { o.TenantId, o.OrderNumber }).IsUnique();

          // 5. Field constraints
          entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(20);
          entity.Property(o => o.PickupAddress).IsRequired().HasMaxLength(500);
          entity.Property(o => o.DeliveryAddress).IsRequired().HasMaxLength(500);
          entity.Property(o => o.CargoDescription).IsRequired().HasMaxLength(500);
          entity.Property(o => o.CancellationReason).HasMaxLength(500);
          entity.Property(o => o.CargoWeightKg).HasColumnType("decimal(10,2)");
          entity.Property(o => o.QuotedPrice).HasColumnType("decimal(18,2)");

          // 6. Seed Data
          entity.HasData(seeding);
        });
  }
}
