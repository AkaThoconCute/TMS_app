using back_end_for_TMS.Infrastructure.Normalizer;

namespace back_end_for_TMS.Infrastructure.Api;

public static class ApiExtensions
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddControllers(options =>
    {
      options.Filters.Add<ResultNormalizer>();
    });

    services.AddOpenApi();

    return services;
  }
}
