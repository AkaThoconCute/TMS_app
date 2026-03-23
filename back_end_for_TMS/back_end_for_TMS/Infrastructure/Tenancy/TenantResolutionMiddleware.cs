using System.Security.Claims;

namespace back_end_for_TMS.Infrastructure.Tenancy;

public class TenantResolutionMiddleware(RequestDelegate next)
{
  private static readonly string[] TenantClaimTypes =
  [
    "tenantId", "tenant_id", "TenantId", ClaimTypes.GroupSid
  ];

  public async Task InvokeAsync(HttpContext context, TenantContext tenantContext)
  {
    tenantContext.TenantId = Guid.Empty;

    if (context.User.Identity?.IsAuthenticated == true)
    {
      var tenantIdClaim = TenantClaimTypes
          .Select(claimType => context.User.FindFirst(claimType)?.Value)
          .FirstOrDefault(claimValue => !string.IsNullOrWhiteSpace(claimValue));

      if (Guid.TryParse(tenantIdClaim, out var tenantId))
      {
        tenantContext.TenantId = tenantId;
      }
    }

    await next(context);
  }
}