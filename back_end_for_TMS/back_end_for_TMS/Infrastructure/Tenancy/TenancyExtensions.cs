using back_end_for_TMS.Business.Context;

namespace back_end_for_TMS.Infrastructure.Tenancy;

public static class TenancyExtensions
{
  public static IServiceCollection AddTenancyServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddScoped<ITenantContext, TenantContext>();

    return services;
  }

  public static IApplicationBuilder UseTenantResolution(this IApplicationBuilder app)
  {
    return app.UseMiddleware<TenantResolutionMiddleware>();
  }
}