namespace UFAMS.Infrastructure.Configuration;

public sealed class ArcGisOptions
{
    public const string SectionName = "ArcGIS";

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string FeatureServiceUrl { get; set; } = string.Empty;
}