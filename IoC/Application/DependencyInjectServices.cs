using Application.Interfaces;
using Application.Models.NotificacaoMercadoPago;
using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Application;

public static class DependencyInjectServices
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IPagamentoSerivce, PagamentoSerivce>();
        services.AddScoped<INotificacaoMercadoPagoModel, NotificacaoMercadoPagoModel>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IConfiguracoesPagamentosMercadoPagoService, ConfiguracoesPagamentosMercadoPagoService>();
        return services;
    }
}
