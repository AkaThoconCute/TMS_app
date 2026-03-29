namespace back_end_for_TMS.Business.Types;

// DTO for creating a new customer
public class CreateCustomerDto
{
  public string Name { get; set; } = string.Empty;
  public string? ContactPerson { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string? Address { get; set; }
  public string? TaxCode { get; set; }
  public int CustomerType { get; set; } = 1;
  public int Status { get; set; } = 1;
  public string? Notes { get; set; }
}

// DTO for updating customer information
public class UpdateCustomerDto
{
  public string? Name { get; set; }
  public string? ContactPerson { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Email { get; set; }
  public string? Address { get; set; }
  public string? TaxCode { get; set; }
  public int? CustomerType { get; set; }
  public int? Status { get; set; }
  public string? Notes { get; set; }
}

// DTO for returning customer information
public class CustomerDto
{
  public Guid CustomerId { get; set; }
  public string Name { get; set; } = string.Empty;
  public string? ContactPerson { get; set; }
  public string PhoneNumber { get; set; } = string.Empty;
  public string? Email { get; set; }
  public string? Address { get; set; }
  public string? TaxCode { get; set; }
  public int CustomerType { get; set; }
  public string CustomerTypeLabel { get; set; } = string.Empty;  // "Individual" or "Business"
  public int Status { get; set; }
  public string StatusLabel { get; set; } = string.Empty;        // "Active" or "Inactive"
  public string? Notes { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset? UpdatedAt { get; set; }
}
