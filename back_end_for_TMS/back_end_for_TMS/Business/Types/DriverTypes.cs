namespace back_end_for_TMS.Business.Types;

// DTO for creating a new driver
public class CreateDriverDto
{
  public string FullName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public string LicenseNumber { get; set; } = string.Empty;
  public string? LicenseClass { get; set; }
  public DateTime? LicenseExpiry { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public int Status { get; set; } = 1;
  public DateTime? HireDate { get; set; }
  public string? Notes { get; set; }
}

// DTO for updating driver information
public class UpdateDriverDto
{
  public string? FullName { get; set; }
  public string? PhoneNumber { get; set; }
  public string? LicenseNumber { get; set; }
  public string? LicenseClass { get; set; }
  public DateTime? LicenseExpiry { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public int? Status { get; set; }
  public DateTime? HireDate { get; set; }
  public string? Notes { get; set; }
}

// DTO for returning driver information
public class DriverDto
{
  public Guid DriverId { get; set; }
  public string FullName { get; set; } = string.Empty;
  public string PhoneNumber { get; set; } = string.Empty;
  public string LicenseNumber { get; set; } = string.Empty;
  public string? LicenseClass { get; set; }
  public DateTime? LicenseExpiry { get; set; }
  public DateTime? DateOfBirth { get; set; }
  public int Status { get; set; }
  public DateTime? HireDate { get; set; }
  public string? Notes { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? UpdatedAt { get; set; }
  public bool IsLicenseExpiringSoon { get; set; }
}
