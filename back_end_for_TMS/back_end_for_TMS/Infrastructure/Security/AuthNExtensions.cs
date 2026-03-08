using back_end_for_TMS.Infrastructure.Database;
using back_end_for_TMS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace back_end_for_TMS.Infrastructure.Security;

public static class AuthNExtensions
{
    public static IServiceCollection AddAuthNServices(this IServiceCollection services, IConfiguration config)
    {
        // 1. Identity Configuration
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        // 2. Authentication Configuration
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["JWT:SigningKey"] ?? throw new InvalidOperationException("JWT:SigningKey is missing"))
                ),
                ValidAlgorithms = [SecurityAlgorithms.HmacSha512],
                ClockSkew = TimeSpan.Zero // Standard practice: remove the default 5 min grace period for tokens
            };
        });

        return services;
    }
}
