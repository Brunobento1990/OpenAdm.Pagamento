using Domain.Factories;
using Infrastructure.Pagamento.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Infrastructure;

public static class PagamentoDependencyInject
{
    public static IServiceCollection PagamentoInject(this IServiceCollection services)
    {
        services.AddScoped<IPagamenoFactory, PagamenoFactory>();

        return services;
    }
}
