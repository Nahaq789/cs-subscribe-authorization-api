using Authorization.API.application.service;
using Authorization.API.infrastructure;
using Authorization.API.infrastructure.repository;
using Microsoft.EntityFrameworkCore;

namespace User.Authentication.extensions;

internal static class Extensions
{
    /// <summary>
    /// 初回実行時にDIを行います。
    /// DbConntext
    /// MediatR
    /// IUserRepository
    /// </summary>
    public static void SetupDI(this WebApplicationBuilder builder)
    {
        var configure = builder.Configuration;
        var connectionString = configure.GetConnectionString("userdb");

        builder.Services.AddDbContext<UserContext>((serviceProvider, option) =>
        {
            option.UseNpgsql(connectionString);
        });

        var services = builder.Services;
        services.AddMediatR((cfg) =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
        });

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "host.docker.internal:6379";
        });

        builder.Services.AddTransient<ICryptoPasswordService, CryptoPasswordService>();
        builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
        builder.Services.AddTransient<IRefreshTokenService, RefreshTokenService>();

        builder.Services.AddTransient<IUserAuthRepository, UserAuthRepository>();
    }
}