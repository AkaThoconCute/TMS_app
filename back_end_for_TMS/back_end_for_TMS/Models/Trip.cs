namespace back_end_for_TMS.Models;

public class Trip : ITenantEntity
{
  // Tenant Information (Bắt buộc cho multi-tenant)
  public Guid TenantId { get; set; }                              // FK → Tenant (multi-tenant)

  // Identity Group
  public Guid TripId { get; set; } = Guid.CreateVersion7();
  public string TripNumber { get; set; } = string.Empty;          // Auto-generated: TRP-{D6}, unique per tenant
  public Guid OrderId { get; set; }                                // FK → Order
  public Guid TruckId { get; set; }                                // FK → Truck
  public Guid DriverId { get; set; }                               // FK → Driver

  // Schedule (Planned)
  public DateTime? PlannedPickupDate { get; set; }                 // Ngày dự kiến lấy hàng
  public DateTime? PlannedDeliveryDate { get; set; }               // Ngày dự kiến giao hàng

  // Schedule (Actual)
  public DateTime? ActualPickupDate { get; set; }                  // Ngày thực tế lấy hàng
  public DateTime? ActualDeliveryDate { get; set; }                // Ngày thực tế giao hàng

  // Operational Data
  public int Status { get; set; } = 1;                             // 1=Planned, 2=InTransit, 3=Completed, 4=Cancelled

  // Cost Recording (recorded at completion)
  public decimal? FuelCost { get; set; }                           // Chi phí nhiên liệu
  public decimal? TollCost { get; set; }                           // Chi phí cầu đường
  public decimal? OtherCost { get; set; }                          // Chi phí khác
  public string? CostNotes { get; set; }                           // Ghi chú chi phí (max 500)

  // Lifecycle
  public string? CancellationReason { get; set; }                  // Lý do hủy (max 500)
  public string? Notes { get; set; }                               // Ghi chú

  // Lifecycle Timestamps
  public DateTimeOffset? CompletedAt { get; set; }                 // Thời điểm hoàn thành
  public DateTimeOffset? CancelledAt { get; set; }                 // Thời điểm hủy

  // Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? UpdatedAt { get; set; }

  // Navigation Properties
  public Order Order { get; set; } = default!;
  public Truck Truck { get; set; } = default!;
  public Driver Driver { get; set; } = default!;
}
