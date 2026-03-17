namespace back_end_for_TMS.Business.Types;

// DTO for creating a new truck
public class CreateTruckDto
{
  public string LicensePlate { get; set; } = string.Empty;
  public string? VinNumber { get; set; }
  public string? EngineNumber { get; set; }
  public string? Brand { get; set; }
  public int? ModelYear { get; set; }
  public DateTime? PurchaseDate { get; set; }
  public string? TruckType { get; set; }
  public decimal? MaxPayloadKg { get; set; }
  public int? LengthMm { get; set; }
  public int? WidthMm { get; set; }
  public int? HeightMm { get; set; }
  public int OwnershipType { get; set; } = 1;
}

// DTO for updating truck information
public class UpdateTruckDto
{
  public string? LicensePlate { get; set; }
  public string? VinNumber { get; set; }
  public string? EngineNumber { get; set; }
  public string? Brand { get; set; }
  public int? ModelYear { get; set; }
  public DateTime? PurchaseDate { get; set; }
  public string? TruckType { get; set; }
  public decimal? MaxPayloadKg { get; set; }
  public int? LengthMm { get; set; }
  public int? WidthMm { get; set; }
  public int? HeightMm { get; set; }
  public int? OwnershipType { get; set; }
  public int? CurrentStatus { get; set; }
  public decimal? OdometerReading { get; set; }
  public DateTime? LastMaintenanceDate { get; set; }
}

// DTO for returning truck information
public class TruckDto
{
  public Guid TruckId { get; set; }
  public string LicensePlate { get; set; } = string.Empty;
  public string? VinNumber { get; set; }
  public string? EngineNumber { get; set; }
  public string? Brand { get; set; }
  public int? ModelYear { get; set; }
  public DateTime? PurchaseDate { get; set; }
  public string? TruckType { get; set; }
  public decimal? MaxPayloadKg { get; set; }
  public int? LengthMm { get; set; }
  public int? WidthMm { get; set; }
  public int? HeightMm { get; set; }
  public int OwnershipType { get; set; }
  public int CurrentStatus { get; set; }
  public decimal OdometerReading { get; set; }
  public DateTime? LastMaintenanceDate { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? UpdatedAt { get; set; }
}

// DTO for paginated list response
public class PaginatedTrucksDto
{
  public List<TruckDto> Data { get; set; } = [];
  public int TotalCount { get; set; }
  public int PageSize { get; set; }
  public int PageNumber { get; set; }
  public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
}

// DTO for updating odometer
public class UpdateOdometerDto
{
  public decimal OdometerReading { get; set; }
}

// DTO for updating status
public class UpdateStatusDto
{
  public int Status { get; set; }
}

// DTO for updating maintenance date
public class UpdateMaintenanceDateDto
{
  public DateTime MaintenanceDate { get; set; }
}
