using System.Security.Claims;
using back_end_for_TMS.Business.Context;

namespace back_end_for_TMS.Infrastructure.Tenancy;

public class TenantResolutionMiddleware(RequestDelegate next)
{
  private static readonly string[] TenantClaimTypes =
  [
    "tenantId", "tenant_id", "TenantId", ClaimTypes.GroupSid
  ];

  public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext)
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

public class TenantResolutionMiddlewareIssue(RequestDelegate next, ITenantContext tenantContext)
{
  private readonly RequestDelegate _next = next;
  private readonly ITenantContext _tenantContext = tenantContext;

  private static readonly string[] TenantClaimTypes =
  [
      "tenantId", "tenant_id", "TenantId", ClaimTypes.GroupSid
  ];

  public async Task InvokeAsync(HttpContext context)
  {
    _tenantContext.TenantId = Guid.Empty;

    if (context.User.Identity?.IsAuthenticated == true)
    {
      var tenantIdClaim = TenantClaimTypes
          .Select(claimType => context.User.FindFirst(claimType)?.Value)
          .FirstOrDefault(claimValue => !string.IsNullOrWhiteSpace(claimValue));

      if (Guid.TryParse(tenantIdClaim, out var tenantId))
      {
        _tenantContext.TenantId = tenantId;
      }
    }

    await _next(context);
  }
}