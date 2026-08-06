namespace UFAMS.Infrastructure.ArcGIS;

public class ArcGisOAuthOptions
{
    public string PortalUrl { get; set; } = "https://www.arcgis.com";

    public string ClientId { get; set; } = string.Empty;

    public string RedirectUri { get; set; } = string.Empty;
}