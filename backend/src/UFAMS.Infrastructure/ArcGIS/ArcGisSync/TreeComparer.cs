using UFAMS.Domain.Entities;
using UFAMS.Infrastructure.ArcGIS.Models;

namespace UFAMS.Application.Features.ArcGisSync;

public static class TreeComparer
{
    public static TreeComparisonResult Compare(
        Tree tree,
        ArcGisFeature feature)
    {
        var healthChanged =
            tree.HealthStatus.ToString()
            != feature.HealthStatus;

        var speciesChanged =
            tree.Species.CommonName
            != feature.Species;

        var parkChanged =
            tree.Park.Name
            != feature.Park;

        var locationChanged =
            tree.Location.Latitude != feature.Latitude
            ||
            tree.Location.Longitude != feature.Longitude;

        return new TreeComparisonResult(
            HasChanges:
                healthChanged
                || speciesChanged
                || parkChanged
                || locationChanged,

            HealthChanged: healthChanged,
            SpeciesChanged: speciesChanged,
            ParkChanged: parkChanged,
            LocationChanged: locationChanged);
    }
}