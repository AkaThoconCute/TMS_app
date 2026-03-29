namespace back_end_for_TMS.Models;

public class Customer : ITenantEntity
{
  // Tenant Information (Bắt buộc cho multi-tenant)
  public Guid TenantId { get; set; }                          // FK → Tenant (multi-tenant)

  // Identity Group
  public Guid CustomerId { get; set; } = Guid.CreateVersion7();
  public string Name { get; set; } = string.Empty;            // Tên khách hàng (max 200, required)
  public string? ContactPerson { get; set; }                   // Người liên hệ (max 100)
  public string PhoneNumber { get; set; } = string.Empty;     // Số điện thoại (max 20, required)
  public string? Email { get; set; }                           // Email (max 200)
  public string? Address { get; set; }                         // Địa chỉ (max 500)
  public string? TaxCode { get; set; }                         // Mã số thuế (max 50)

  // Specification
  public int CustomerType { get; set; } = 1;                   // 1=Individual (Cá nhân), 2=Business (Doanh nghiệp)

  // Operational Data
  public int Status { get; set; } = 1;                         // 1=Active, 2=Inactive
  public string? Notes { get; set; }                           // Ghi chú

  // Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? UpdatedAt { get; set; }
}
