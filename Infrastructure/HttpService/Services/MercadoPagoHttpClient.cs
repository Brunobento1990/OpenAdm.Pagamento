using Domain.Model;
using Infrastructure.HttpService.Interfaces;
using Infrastructure.Model.MercadoPago;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.HttpService.Services;

public sealed class MercadoPagoHttpClient : IMercadoPagoHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MercadoPagoHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MercadoPagoResponse> PostAsync(MercadoPagoRequest mercadoPagoRequest, string accessToken)
    {
        var httpClient = _httpClientFactory.CreateClient("MercadoPago");

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await httpClient.PostAsync("payments", mercadoPagoRequest.ToJson());

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine(error);
            throw new Exception($"Não foi possível efetuar o pagamento! AccessToken: {accessToken}");
        }
        var content = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<MercadoPagoResponse>(content, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }) ?? throw new Exception("Não foi possível desserealizar o objeto response do mercado pago!");
    }
}
