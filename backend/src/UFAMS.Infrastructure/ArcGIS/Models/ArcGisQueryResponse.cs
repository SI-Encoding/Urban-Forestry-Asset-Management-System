namespace UFAMS.Infrastructure.ArcGIS.Models;

public sealed class ArcGisQueryResponse
{
    public List<ArcGisFeatureResponse> Features { get; set; } = [];
}