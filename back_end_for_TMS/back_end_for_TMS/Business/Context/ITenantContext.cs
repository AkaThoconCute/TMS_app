namespace back_end_for_TMS.Business.Context;

public interface ITenantContext
{
  Guid TenantId { get; set; }
  bool IsGlobalUser { get; }
}