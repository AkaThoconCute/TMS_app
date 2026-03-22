using back_end_for_TMS.Business;
using back_end_for_TMS.Infrastructure.Mapper;
using back_end_for_TMS.Infrastructure.Response;
using back_end_for_TMS.Models.Repository;

namespace back_end_for_TMS.Infrastructure.Business;

public static class BusinessExtensions
{
  public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddExceptionHandler<GlobalExceptionHandler>();

    services.AddProblemDetails();

    services.AddAutoMapper(typeof(AppMapperProfile).Assembly);

    // Repositories
    services.AddScoped<TruckRepo>();

    // Services
    services.AddScoped<TokenService>();

    services.AddScoped<AccountService>();

    services.AddScoped<TruckService>();

    return services;
  }
}
