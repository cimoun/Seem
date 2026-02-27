using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seem.Application.Common.Interfaces;
using Seem.Domain.Interfaces;
using Seem.Infrastructure.Persistence;
using Seem.Infrastructure.Persistence.Interceptors;
using Seem.Infrastructure.Services;

namespace Seem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntityInterceptor>();

        services.AddDbContext<SeemDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions => npgsqlOptions.MigrationsAssembly(typeof(SeemDbContext).Assembly.FullName))
                .AddInterceptors(interceptor);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SeemDbContext>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SeemDbContext>());
        services.AddSingleton<IDateTime, DateTimeService>();

        return services;
    }
}
