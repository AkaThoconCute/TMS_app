using back_end_for_TMS.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class IdentityDataSeeder
{
  public static (List<IdentityRole> Roles, List<AppUser> Users, List<IdentityUserRole<string>> UserRoles) GenerateIdentityData()
  {
    Randomizer.Seed = new Random(123456);
    var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    const string staticHash = "AQAAAAIAAYagAAAAEOf6Pb8v/8VwLIDv8T6/7UfVvJqR9Z0X5Y6vX5Y6vX";
    const string staticStamp = "STATIC_STAMP_123456";

    // 1. Define Roles
    var roles = new List<IdentityRole>
        {
            new() { Id = "r-admin", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = staticStamp },
            new() { Id = "r-user", Name = "User", NormalizedName = "USER", ConcurrencyStamp = staticStamp }
        };

    // 2. Define Users
    var admin = new AppUser
    {
      Id = "u-admin",
      UserName = "Top1Server",
      NormalizedUserName = "TOP1SERVER",
      Email = "admin@asp.app",
      NormalizedEmail = "ADMIN@ASP.APP",
      EmailConfirmed = true,
      CreatedAt = seedDate,
      PasswordHash = staticHash,
      SecurityStamp = staticStamp,
      ConcurrencyStamp = staticStamp
    };

    var user1 = new AppUser
    {
      Id = "u-user1",
      UserName = "User1",
      NormalizedUserName = "USER1",
      Email = "user1@asp.app",
      NormalizedEmail = "USER1@ASP.APP",
      EmailConfirmed = true,
      CreatedAt = seedDate,
      PasswordHash = staticHash,
      SecurityStamp = staticStamp,
      ConcurrencyStamp = staticStamp
    };

    var users = new List<AppUser> { admin, user1 };

    // 3. Map Users to Roles
    var userRoles = new List<IdentityUserRole<string>>
        {
            new() { RoleId = "r-admin", UserId = "u-admin" },
            new() { RoleId = "r-user", UserId = "u-user1" }
        };

    return (roles, users, userRoles);
  }
}
