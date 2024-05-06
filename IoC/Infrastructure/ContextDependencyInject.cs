using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Infrastructure;

public static class ContextDependencyInject
{
    public static IServiceCollection InjectContext(this IServiceCollection services, string connectionString)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<OpenAdmContext>(opt => opt.UseNpgsql(connectionString));
        services.AddDbContext<ParceiroContext>();

        return services;
    }
}
