namespace back_end_for_TMS.Models;

public class Driver : ITenantEntity
{
  // Tenant Information (Bắt buộc cho multi-tenant)
  public Guid TenantId { get; set; }                        // FK → Tenant (multi-tenant)

  // Identity Group (Hầu như không đổi)
  public Guid DriverId { get; set; } = Guid.CreateVersion7();
  public string FullName { get; set; } = string.Empty;      // Họ và tên
  public string PhoneNumber { get; set; } = string.Empty;   // Số điện thoại
  public string LicenseNumber { get; set; } = string.Empty; // Số giấy phép lái xe
  public string? LicenseClass { get; set; }                  // Hạng bằng lái (B2, C, D, FC)
  public DateTime? LicenseExpiry { get; set; }               // Ngày hết hạn bằng lái
  public DateTime? DateOfBirth { get; set; }                 // Ngày sinh

  // Operational Data (Thay đổi thường xuyên)
  public int Status { get; set; } = 1;                      // 1=Active, 2=OnLeave, 3=Terminated
  public DateTime? HireDate { get; set; }                    // Ngày vào làm
  public string? Notes { get; set; }                         // Ghi chú

  // Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? UpdatedAt { get; set; }             // Cập nhật mỗi khi thông tin thay đổi
}
