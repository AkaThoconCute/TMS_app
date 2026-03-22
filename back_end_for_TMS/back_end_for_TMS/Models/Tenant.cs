namespace back_end_for_TMS.Models;

public class Tenant
{
  // Identity
  public Guid TenantId { get; set; } = Guid.CreateVersion7();
  public string Name { get; set; } = string.Empty;        // Tên hộ kinh doanh
  public string OwnerId { get; set; } = string.Empty;     // FK → AppUser.Id

  // Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
