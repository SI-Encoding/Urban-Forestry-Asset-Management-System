namespace UFAMS.Infrastructure.ArcGIS.Models;

public sealed class ArcGisAttributes
{
    public int OBJECTID { get; set; }

    public string assetTag { get; set; } = "";

    public string species { get; set; } = "";

    public string park { get; set; } = "";

    public string healthStatus { get; set; } = "";
}