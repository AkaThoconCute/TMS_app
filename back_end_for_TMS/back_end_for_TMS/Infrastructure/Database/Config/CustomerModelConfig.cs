using back_end_for_TMS.Models;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.Config;

public class CustomerModelConfig : IModelConfig<Customer>
{
  public static void Setup(ModelBuilder builder, List<Customer> seeding)
  {
    builder.Entity<Customer>(entity =>
        {
          // 1. Tenant FK
          entity.HasOne<Tenant>().WithMany().HasForeignKey(c => c.TenantId).OnDelete(DeleteBehavior.Restrict);

          // 2. Add Index
          entity.HasIndex(c => c.TenantId);

          // 3. Field constraints
          entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
          entity.Property(c => c.ContactPerson).HasMaxLength(100);
          entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(20);
          entity.Property(c => c.Email).HasMaxLength(200);
          entity.Property(c => c.Address).HasMaxLength(500);
          entity.Property(c => c.TaxCode).HasMaxLength(50);

          // 4. Seed Data
          entity.HasData(seeding);
        });
  }
}
