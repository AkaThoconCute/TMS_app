namespace back_end_for_TMS.Business.Types;

// DTO for creating a trip (assign truck + driver to an order)
public class CreateTripDto
{
  public Guid OrderId { get; set; }
  public Guid TruckId { get; set; }
  public Guid DriverId { get; set; }
  public DateTime? PlannedPickupDate { get; set; }
  public DateTime? PlannedDeliveryDate { get; set; }
  public string? Notes { get; set; }
}

// DTO for updating trip info (partial update — all nullable)
public class UpdateTripDto
{
  public Guid? TruckId { get; set; }
  public Guid? DriverId { get; set; }
  public DateTime? PlannedPickupDate { get; set; }
  public DateTime? PlannedDeliveryDate { get; set; }
  public string? Notes { get; set; }
}

// DTO for starting a trip (Planned → InTransit)
public class StartTripDto
{
  public DateTime? ActualPickupDate { get; set; }  // Defaults to now if not provided
}

// DTO for completing a trip (InTransit → Completed)
public class CompleteTripDto
{
  public Guid TripId { get; set; }
  public DateTime ActualDeliveryDate { get; set; }
  public decimal? FuelCost { get; set; }
  public decimal? TollCost { get; set; }
  public decimal? OtherCost { get; set; }
  public string? CostNotes { get; set; }
  public string? Notes { get; set; }
}

// DTO for cancelling a trip
public class CancelTripDto
{
  public Guid TripId { get; set; }
  public string CancellationReason { get; set; } = string.Empty;
}

// DTO for returning trip information
public class TripDto
{
  public Guid TripId { get; set; }
  public string TripNumber { get; set; } = string.Empty;
  public Guid OrderId { get; set; }
  public string OrderNumber { get; set; } = string.Empty;
  public Guid TruckId { get; set; }
  public string TruckLicensePlate { get; set; } = string.Empty;
  public Guid DriverId { get; set; }
  public string DriverFullName { get; set; } = string.Empty;
  public DateTime? PlannedPickupDate { get; set; }
  public DateTime? PlannedDeliveryDate { get; set; }
  public DateTime? ActualPickupDate { get; set; }
  public DateTime? ActualDeliveryDate { get; set; }
  public int Status { get; set; }
  public string StatusLabel { get; set; } = string.Empty;
  public decimal? FuelCost { get; set; }
  public decimal? TollCost { get; set; }
  public decimal? OtherCost { get; set; }
  public decimal? TotalCost { get; set; }
  public string? CostNotes { get; set; }
  public string? CancellationReason { get; set; }
  public string? Notes { get; set; }
  public DateTimeOffset? CompletedAt { get; set; }
  public DateTimeOffset? CancelledAt { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? UpdatedAt { get; set; }
}
