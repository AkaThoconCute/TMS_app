using back_end_for_TMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace back_end_for_TMS.Infrastructure.Database.ModelConfig;

public class RoleModelConfig : IModelConfig<IdentityRole>
{
  public static void Setup(ModelBuilder builder, List<IdentityRole> seeding)
  {
    builder.Entity<IdentityRole>().HasData(seeding);
  }
}

public class AppUserModelConfig : IModelConfig<AppUser>
{
  public static void Setup(ModelBuilder builder, List<AppUser> seeding)
  {
    builder.Entity<AppUser>(entity =>
    {
      entity.HasOne<Tenant>().WithMany().HasForeignKey(u => u.TenantId).OnDelete(DeleteBehavior.SetNull);

      entity.HasIndex(u => u.TenantId);

      entity.HasData(seeding);
    });
  }
}

public class UserRoleModelConfig : IModelConfig<IdentityUserRole<string>>
{
  public static void Setup(ModelBuilder builder, List<IdentityUserRole<string>> seeding)
  {
    builder.Entity<IdentityUserRole<string>>().HasData(seeding);
  }
}