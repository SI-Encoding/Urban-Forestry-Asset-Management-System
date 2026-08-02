using System.Text.Json.Serialization;

namespace UFAMS.Infrastructure.ArcGIS.Models;

public sealed class ArcGisFeatureServiceInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("layers")]
    public List<ArcGisLayerInfo> Layers { get; set; } = [];
}


public sealed class ArcGisLayerInfo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}