namespace UFAMS.Infrastructure.ArcGIS.Models;

public sealed class ArcGisFeatureResponse
{
    public ArcGisAttributes Attributes { get; set; } = null!;

    public ArcGisGeometry Geometry { get; set; } = null!;
}