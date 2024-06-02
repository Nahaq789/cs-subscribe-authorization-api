using Authorization.API.infrastructure;
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
        var connectionString = configure.GetConnectionString("ConnectionStrings");

        builder.Services.AddDbContext<UserContext>((serviceProvider, option) =>
        {
            option.UseNpgsql(connectionString);
        });

        var services = builder.Services;
        services.AddMediatR((cfg) =>
        {
            cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
        });

        builder.Services.AddTransient<IUserAuthRepository, UserAuthRepository>();
    }
}