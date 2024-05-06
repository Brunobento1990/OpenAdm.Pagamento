using Infrastructure.HttpService.Interfaces;
using Infrastructure.HttpService.Services;
using Infrastructure.Model;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.Infrastructure;

public static class HttpClientDependencyInject
{
    public static IServiceCollection HttpClientInject(this IServiceCollection services, IList<HttpClientInjectModel> models)
    {
        services.AddTransient<IDiscordHttpService, DiscordHttpService>();
        services.AddTransient<IMercadoPagoHttpClient, MercadoPagoHttpClient>();

        foreach (var model in models)
        {
            services.AddHttpClient(model.HttpClient, x =>
            {
                x.BaseAddress = new Uri(model.UrlBase);
            });
        }

        return services;
    }
}
