using UFAMS.Application.Features.ArcGisSync.Models;

using UFAMS.Application.Interfaces;
using UFAMS.Infrastructure.ArcGIS;

namespace UFAMS.Application.Features.ArcGisSync;

public sealed class SpatialDataSyncService
{
    private readonly IArcGisFeatureProvider _provider;

    private readonly ITreeRepository _treeRepository;

    public SpatialDataSyncService(
        IArcGisFeatureProvider provider,
        ITreeRepository treeRepository)
    {
        _provider = provider;
        _treeRepository = treeRepository;
    }

    public async Task<SpatialSyncResult> SynchronizeAsync(
        CancellationToken cancellationToken = default)
    {
        var actions =
    new List<SpatialSyncAction>();
        
        var features =
            await _provider.GetFeaturesAsync(
                cancellationToken);

        var trees =
            await _treeRepository.GetAllAsync(
                cancellationToken);

        int created = 0;
        int updated = 0;
        int unchanged = 0;

        foreach (var feature in features)
        {
            var existingTree =
                trees.FirstOrDefault(
                    tree =>
                        tree.ArcGisFeatureId ==
                        feature.Id);

            if (existingTree is null)
{
    created++;

    actions.Add(
        new SpatialSyncAction(
            "Create",
            feature.AssetTag,
            "Tree does not exist in UFAMS"));

    continue;
}

if (
    existingTree.HealthStatus.ToString()
    !=
    feature.HealthStatus
)
{
    updated++;

    actions.Add(
        new SpatialSyncAction(
            "Update",
            feature.AssetTag,
            "Health status changed"));

    continue;
}

unchanged++;

actions.Add(
    new SpatialSyncAction(
        "Unchanged",
        feature.AssetTag,
        "No differences detected"));

            unchanged++;
        }

        return new SpatialSyncResult(
    Created: created,
    Updated: updated,
    Deleted: 0,
    Unchanged: unchanged,
    Actions: actions);
    }
}