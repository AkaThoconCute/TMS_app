using back_end_for_TMS.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class AppUserDataSeeder : IDataSeeder<AppUser>
{
  private const string StaticHash = "AQAAAAIAAYagAAAAEOf6Pb8v/8VwLIDv8T6/7UfVvJqR9Z0X5Y6vX5Y6vX";
  private const string StaticStamp = "STATIC_STAMP_123456";
  private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  public static List<AppUser> Generate()
  {
    return new List<AppUser>
        {
            new() {
                Id = "u-admin",
                TenantId = TenantDataSeeder.AlphaTenantId,
                UserName = "Top1Server",
                NormalizedUserName = "TOP1SERVER",
                Email = "admin@asp.app",
                NormalizedEmail = "ADMIN@ASP.APP",
                EmailConfirmed = true,
                CreatedAt = SeedDate,
                PasswordHash = StaticHash,
                SecurityStamp = StaticStamp,
                ConcurrencyStamp = StaticStamp
            },
            new() {
                Id = "u-user1",
                TenantId = TenantDataSeeder.AlphaTenantId,
                UserName = "User1",
                NormalizedUserName = "USER1",
                Email = "user1@asp.app",
                NormalizedEmail = "USER1@ASP.APP",
                EmailConfirmed = true,
                CreatedAt = SeedDate,
                PasswordHash = StaticHash,
                SecurityStamp = StaticStamp,
                ConcurrencyStamp = StaticStamp
            }
        };
  }
}

public class RoleDataSeeder : IDataSeeder<IdentityRole>
{
  public const string StaticStamp = "STATIC_STAMP_123456";

  public static List<IdentityRole> Generate()
  {
    return new List<IdentityRole>
        {
            new() { Id = "r-admin", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = StaticStamp },
            new() { Id = "r-user", Name = "User", NormalizedName = "USER", ConcurrencyStamp = StaticStamp }
        };
  }
}

public class UserRoleDataSeeder : IDataSeeder<IdentityUserRole<string>>
{
  public static List<IdentityUserRole<string>> Generate()
  {
    return new List<IdentityUserRole<string>>
        {
            new() { RoleId = "r-admin", UserId = "u-admin" },
            new() { RoleId = "r-user", UserId = "u-user1" }
        };
  }
}