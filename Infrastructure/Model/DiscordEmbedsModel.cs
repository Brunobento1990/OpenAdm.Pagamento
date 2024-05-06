using System.Text.Json.Serialization;

namespace Infrastructure.Model;

public class DiscordEmbedsModel
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("color")]
    public int? Color { get; set; }
}
