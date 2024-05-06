using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Infrastructure;

public static class RepositoriesDependencyInject
{
    public static IServiceCollection InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IConfiguracaoPagamentoMercadoPagoRepository, ConfiguracaoPagamentoMercadoPagoRepository>();
        services.AddScoped<IConfiguracaoParceiroRepository, ConfiguracaoParceiroRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPagamentoPedidoRepository, PagamentoPedidoRepository>();
        services.AddHttpContextAccessor();
        return services;
    }
}
