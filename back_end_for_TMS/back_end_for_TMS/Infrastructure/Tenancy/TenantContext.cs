using back_end_for_TMS.Business.Context;

namespace back_end_for_TMS.Infrastructure.Tenancy;

public class TenantContext : ITenantContext
{
  public Guid TenantId { get; set; } = Guid.Empty;
  public bool IsGlobalUser => TenantId == Guid.Empty;
}