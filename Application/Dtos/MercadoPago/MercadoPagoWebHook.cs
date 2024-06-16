using System.Text.Json.Serialization;

namespace Application.Dtos.MercadoPago;

public class MercadoPagoWebHook
{
    [JsonPropertyName("action")]
    public string? Action { get; set; } 
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("data")]
    public DataMercadoPago? Data { get; set; }
}
