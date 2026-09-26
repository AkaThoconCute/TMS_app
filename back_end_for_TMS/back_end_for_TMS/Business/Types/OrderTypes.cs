namespace back_end_for_TMS.Business.Types;

// DTO for creating a new order
public class CreateOrderDto
{
  public Guid CustomerId { get; set; }
  public string PickupAddress { get; set; } = string.Empty;
  public string DeliveryAddress { get; set; } = string.Empty;
  public string CargoDescription { get; set; } = string.Empty;
  public decimal? CargoWeightKg { get; set; }
  public DateTime? RequestedPickupDate { get; set; }
  public DateTime? RequestedDeliveryDate { get; set; }
  public decimal? QuotedPrice { get; set; }
  public string? Notes { get; set; }
}

// DTO for updating order information (partial update — all nullable)
public class UpdateOrderDto
{
  public string? PickupAddress { get; set; }
  public string? DeliveryAddress { get; set; }
  public string? CargoDescription { get; set; }
  public decimal? CargoWeightKg { get; set; }
  public DateTime? RequestedPickupDate { get; set; }
  public DateTime? RequestedDeliveryDate { get; set; }
  public decimal? QuotedPrice { get; set; }
  public string? Notes { get; set; }
}

// DTO for completing an order (Delivered → Completed)
public class CompleteOrderDto
{
  public string? Notes { get; set; }
}

// DTO for cancelling an order
public class CancelOrderDto
{
  public Guid OrderId { get; set; }
  public string CancellationReason { get; set; } = string.Empty;
}

// DTO for returning order information
public class OrderDto
{
  public Guid OrderId { get; set; }
  public string OrderNumber { get; set; } = string.Empty;
  public Guid CustomerId { get; set; }
  public string CustomerName { get; set; } = string.Empty;
  public string PickupAddress { get; set; } = string.Empty;
  public string DeliveryAddress { get; set; } = string.Empty;
  public string CargoDescription { get; set; } = string.Empty;
  public decimal? CargoWeightKg { get; set; }
  public DateTime? RequestedPickupDate { get; set; }
  public DateTime? RequestedDeliveryDate { get; set; }
  public decimal? QuotedPrice { get; set; }
  public int Status { get; set; }
  public string StatusLabel { get; set; } = string.Empty;
  public string? CancellationReason { get; set; }
  public string? Notes { get; set; }
  public int TripCount { get; set; }
  public DateTimeOffset? CompletedAt { get; set; }
  public DateTimeOffset? CancelledAt { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? UpdatedAt { get; set; }
}

// DTO for status summary (pipeline counts)
public class OrderStatusSummaryDto
{
  public int Created { get; set; }
  public int Assigned { get; set; }
  public int InTransit { get; set; }
  public int Delivered { get; set; }
  public int Completed { get; set; }
  public int Cancelled { get; set; }
}
