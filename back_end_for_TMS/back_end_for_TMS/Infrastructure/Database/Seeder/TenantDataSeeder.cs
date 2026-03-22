using back_end_for_TMS.Models;

namespace back_end_for_TMS.Infrastructure.Database.Seeder;

public class TenantDataSeeder : IDataSeeder<Tenant>
{
  public static readonly Guid AlphaTenantId = Guid.Parse("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8e");

  public static List<Tenant> Generate()
  {
    // Fixed GUIDs to ensure migration stability
    var tenantIds = new[]
    {
            AlphaTenantId,
            Guid.Parse("0195c63a-727b-7f32-8e2d-3f4a5b6c7d8f"),
            Guid.Parse("0195c63a-727b-7f32-8e2d-3f4a5b6c7d90"),
            Guid.Parse("0195c63a-727b-7f32-8e2d-3f4a5b6c7d91"),
            Guid.Parse("0195c63a-727b-7f32-8e2d-3f4a5b6c7d92")
        };

    var owners = new[] { "u-admin", "u-user1", "u-admin", "u-user1", "u-admin" };
    var names = new[] { "Alpha Logistics", "Bravo Trans", "Charlie Freight", "Delta Moving", "Echo Shipping" };

    var tenants = new List<Tenant>();
    for (int i = 0; i < 5; i++)
    {
      tenants.Add(new Tenant
      {
        TenantId = tenantIds[i],
        Name = names[i],
        OwnerId = owners[i],
        CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
      });
    }

    return tenants;
  }
}