namespace back_end_for_TMS.Infrastructure.Security;

public static class AuthZExtensions
{
    public static IServiceCollection AddAuthZServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

        return services;
    }
}
