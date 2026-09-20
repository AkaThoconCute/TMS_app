using back_end_for_TMS.Business;
using back_end_for_TMS.Example.BusinessImpl;
using back_end_for_TMS.Example.IBusiness;
using back_end_for_TMS.Infrastructure.Mapper;
using back_end_for_TMS.Infrastructure.Normalizer;
using back_end_for_TMS.Models.Repository;

namespace back_end_for_TMS.Infrastructure.Business;

public static class BusinessExtensions
{
  public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration config)
  {
    services.AddExceptionHandler<ExceptionNormalizer>();

    services.AddProblemDetails();

    services.AddAutoMapper(typeof(AppMapperProfile).Assembly);

    // Repositories
    services.AddScoped<TenantRepo>();

    services.AddScoped<TruckRepo>();

    services.AddScoped<DriverRepo>();

    services.AddScoped<CustomerRepo>();

    services.AddScoped<OrderRepo>();

    services.AddScoped<TripRepo>();

    // Services
    services.AddScoped<IWorkRequestService, WorkRequestService>();

    services.AddScoped<TokenService>();

    services.AddScoped<AccountService>();

    services.AddScoped<TruckService>();

    services.AddScoped<DriverService>();

    services.AddScoped<CustomerService>();

    services.AddScoped<OrderService>();

    services.AddScoped<TripService>();

    return services;
  }
}
