using System.Text.Json.Serialization;

namespace Application.Dtos.MercadoPago;

public class DataMercadoPago
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
