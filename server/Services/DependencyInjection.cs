using Core.Contracts;
using Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;

namespace Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(configuration);
        services.AddTokenGenerator();
        services.AddRepository();
        services.AddPasswordService();

        return services;
    }

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        services.AddScoped<IAuthService, AuthService>();

        services.ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    private static IServiceCollection AddTokenGenerator(this IServiceCollection services)
    {
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        return services;
    }

    private static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepository, Repository>();
        return services;
    }

    private static IServiceCollection AddPasswordService(this IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        return services;
    }
}
