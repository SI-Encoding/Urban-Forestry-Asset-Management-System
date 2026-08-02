using UFAMS.Infrastructure.ArcGIS.Models;

namespace UFAMS.Infrastructure.ArcGIS;

public interface IArcGisFeatureServiceClient
{
    Task<ArcGisFeatureServiceInfo> GetServiceInfoAsync(
        CancellationToken cancellationToken = default);
}