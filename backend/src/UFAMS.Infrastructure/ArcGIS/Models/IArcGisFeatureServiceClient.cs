using UFAMS.Infrastructure.ArcGIS.Models;

namespace UFAMS.Infrastructure.ArcGIS;

public interface IArcGisFeatureServiceClient
{
    Task<ArcGisFeatureServiceInfo> GetServiceInfoAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Models.ArcGisFeature>> GetFeaturesAsync(
        CancellationToken cancellationToken = default);

    Task<string> GetRawFeaturesAsync(
        CancellationToken cancellationToken = default);
}