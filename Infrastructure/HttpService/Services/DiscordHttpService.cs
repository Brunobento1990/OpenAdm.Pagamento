using Domain.Pkg.Errors;
using Infrastructure.HttpService.Interfaces;
using Infrastructure.Model;
using System.Net.Http.Headers;

namespace Infrastructure.HttpService.Services;

public sealed class DiscordHttpService : IDiscordHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DiscordHttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task NotifyExceptionAsync(DiscordModel discordModel, string webHookId, string webHookToken)
    {
        if (string.IsNullOrWhiteSpace(webHookId))
            throw new Exception(CodigoErrors.WebHookIdDiscordInvalido);

        if (string.IsNullOrWhiteSpace(webHookToken))
            throw new Exception(CodigoErrors.WebHookTokenDiscordInvalido);

        var url = $"{webHookId}/{webHookToken}";
        var httpClient = _httpClientFactory.CreateClient("Discord");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        await httpClient.PostAsync(url, discordModel.ToJson());
    }
}
