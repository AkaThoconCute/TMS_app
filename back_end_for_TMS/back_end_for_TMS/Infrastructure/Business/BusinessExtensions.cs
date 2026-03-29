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
    services.AddScoped<TenantRepo>();

    services.AddScoped<TruckRepo>();

    services.AddScoped<DriverRepo>();

    services.AddScoped<CustomerRepo>();

    services.AddScoped<OrderRepo>();

    services.AddScoped<TripRepo>();

    // Services
    services.AddScoped<TokenService>();

    services.AddScoped<AccountService>();

    services.AddScoped<TruckService>();

    services.AddScoped<DriverService>();

    services.AddScoped<CustomerService>();

    services.AddScoped<OrderService>();

    return services;
  }
}
