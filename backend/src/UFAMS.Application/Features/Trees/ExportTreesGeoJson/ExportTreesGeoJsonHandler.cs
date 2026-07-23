using UFAMS.Application.Common.GIS;
using UFAMS.Application.Interfaces;

namespace UFAMS.Application.Features.Trees.ExportTreesGeoJson;

public sealed class ExportTreesGeoJsonHandler
{
    private readonly ITreeRepository _treeRepository;

    public ExportTreesGeoJsonHandler(
        ITreeRepository treeRepository)
    {
        _treeRepository = treeRepository;
    }

    public async Task<GeoJsonFeatureCollection> Handle(
        ExportTreesGeoJsonQuery query,
        CancellationToken cancellationToken = default)
    {
        var trees = await _treeRepository.SearchAsync(
            query.ParkId,
            query.SpeciesId,
            query.HealthStatus,
            query.MinLatitude,
            query.MaxLatitude,
            query.MinLongitude,
            query.MaxLongitude,
            cancellationToken);

        var features = trees
            .Select(tree =>
                new GeoJsonFeature(
                    "Feature",
                    new GeoJsonGeometry(
                        "Point",
                        new[]
                        {
                            tree.Location.Longitude,
                            tree.Location.Latitude
                        }),
                    new TreeGeoJsonProperties(
                        tree.Id,
                        tree.AssetTag,
                        tree.Species.CommonName,
                        tree.Park.Name,
                        tree.HealthStatus.ToString())))
            .ToList();

        return new GeoJsonFeatureCollection(
            "FeatureCollection",
            features);
    }
}