namespace back_end_for_TMS.Infrastructure.Tenancy;

public class TenantContext
{
  public Guid TenantId { get; set; } = Guid.Empty;
}