using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.TokenDtos;
using Limak.Infrastructure.Implementations.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Limak.Infrastructure.ServiceRegistrations;

public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IJWTService, JWTService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<IShipmentCalculatorService, ShipmentCalculatorService>();

        JWTOptionsDto options = configuration.GetSection("JWTOptions").Get<JWTOptionsDto>() ?? new();


        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(config =>
        {
            config.TokenValidationParameters = new()
            {
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = "Role",
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
            };

            config.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["AccessToken"];
                    return Task.CompletedTask;
                }
            };

        });
    }
}
