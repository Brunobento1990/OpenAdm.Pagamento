using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Application;

public static class DependencyInjectServices
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IPagamentoSerivce, PagamentoSerivce>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IConfiguracoesPagamentosMercadoPagoService, ConfiguracoesPagamentosMercadoPagoService>();
        return services;
    }
}
