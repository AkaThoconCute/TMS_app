namespace back_end_for_TMS.Models;

public class Order : ITenantEntity
{
  // Tenant Information (Bắt buộc cho multi-tenant)
  public Guid TenantId { get; set; }                              // FK → Tenant (multi-tenant)

  // Identity Group
  public Guid OrderId { get; set; } = Guid.CreateVersion7();
  public string OrderNumber { get; set; } = string.Empty;         // Auto-generated: ORD-{D6}, unique per tenant
  public Guid CustomerId { get; set; }                             // FK → Customer

  // Shipment Details
  public string PickupAddress { get; set; } = string.Empty;       // Địa chỉ lấy hàng (max 500, required)
  public string DeliveryAddress { get; set; } = string.Empty;     // Địa chỉ giao hàng (max 500, required)
  public string CargoDescription { get; set; } = string.Empty;    // Mô tả hàng hóa (max 500, required)
  public decimal? CargoWeightKg { get; set; }                      // Trọng lượng ước tính (kg)

  // Schedule
  public DateTime? RequestedPickupDate { get; set; }               // Ngày yêu cầu lấy hàng
  public DateTime? RequestedDeliveryDate { get; set; }             // Ngày yêu cầu giao hàng

  // Pricing
  public decimal? QuotedPrice { get; set; }                        // Giá báo cho khách

  // Operational Data
  public int Status { get; set; } = 1;                             // 1=Created, 2=Assigned, 3=InTransit, 4=Delivered, 5=Completed, 6=Cancelled
  public string? CancellationReason { get; set; }                  // Lý do hủy (max 500)
  public string? Notes { get; set; }                               // Ghi chú

  // Lifecycle Timestamps
  public DateTimeOffset? CompletedAt { get; set; }                 // Thời điểm hoàn thành
  public DateTimeOffset? CancelledAt { get; set; }                 // Thời điểm hủy

  // Metadata
  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
  public DateTimeOffset? UpdatedAt { get; set; }

  // Navigation Properties
  public Customer Customer { get; set; } = default!;
  public ICollection<Trip> Trips { get; set; } = [];
}
