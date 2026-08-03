using UFAMS.Infrastructure.ArcGIS.Models;

namespace UFAMS.Infrastructure.ArcGIS;

public interface IArcGisFeatureProvider
{
    Task<IReadOnlyList<ArcGisFeature>> GetFeaturesAsync(
        CancellationToken cancellationToken = default);
}