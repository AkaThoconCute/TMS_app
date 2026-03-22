using Microsoft.AspNetCore.Identity;

namespace back_end_for_TMS.Models
{
  public class AppUser : IdentityUser
  {
    public string? AppName { get; set; }
    public Guid? TenantId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
  }
}
