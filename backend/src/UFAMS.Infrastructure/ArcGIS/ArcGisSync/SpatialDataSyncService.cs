using UFAMS.Application.Features.ArcGisSync.Models;

using UFAMS.Application.Interfaces;

using UFAMS.Domain.Entities;
using UFAMS.Domain.Enums;
using UFAMS.Domain.ValueObjects;

using UFAMS.Infrastructure.ArcGIS;
using UFAMS.Infrastructure.ArcGIS.Models;


namespace UFAMS.Application.Features.ArcGisSync;


public sealed class SpatialDataSyncService
{
    private readonly IArcGisFeatureProvider _provider;

    private readonly ITreeRepository _treeRepository;

    private readonly ISpeciesRepository _speciesRepository;

    private readonly IParkRepository _parkRepository;

    private readonly IUnitOfWork _unitOfWork;



    public SpatialDataSyncService(
        IArcGisFeatureProvider provider,
        ITreeRepository treeRepository,
        ISpeciesRepository speciesRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork)
    {
        _provider = provider;
        _treeRepository = treeRepository;
        _speciesRepository = speciesRepository;
        _parkRepository = parkRepository;
        _unitOfWork = unitOfWork;
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
                        tree.ArcGisFeatureId == feature.Id
                        ||
                        tree.AssetTag.Equals(
                            feature.AssetTag,
                            StringComparison.OrdinalIgnoreCase));



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



            var reasons =
                new List<string>();



            if (
                existingTree.HealthStatus.ToString()
                !=
                feature.HealthStatus)
            {
                reasons.Add(
                    "Health status changed");
            }



            if (
                LocationChanged(
                    existingTree,
                    feature))
            {
                reasons.Add(
                    "Location changed");
            }



            if (reasons.Count > 0)
            {
                updated++;

                actions.Add(
                    new SpatialSyncAction(
                        "Update",
                        feature.AssetTag,
                        string.Join(
                            ", ",
                            reasons)));

                continue;
            }



            unchanged++;

            actions.Add(
                new SpatialSyncAction(
                    "Unchanged",
                    feature.AssetTag,
                    "No differences detected"));
        }



        return new SpatialSyncResult(
            Created: created,
            Updated: updated,
            Deleted: 0,
            Unchanged: unchanged,
            Actions: actions);
    }







    public async Task<SpatialSyncResult> ApplyAsync(
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



        var species =
            await _speciesRepository.GetAllAsync(
                cancellationToken);



        var parks =
            await _parkRepository.GetAllAsync(
                cancellationToken);



        int created = 0;

        int updated = 0;

        int unchanged = 0;





        foreach (var feature in features)
        {
            var existingTree =
                trees.FirstOrDefault(
                    tree =>
                        tree.ArcGisFeatureId == feature.Id
                        ||
                        tree.AssetTag.Equals(
                            feature.AssetTag,
                            StringComparison.OrdinalIgnoreCase));





            if (
                existingTree is not null &&
                string.IsNullOrWhiteSpace(
                    existingTree.ArcGisFeatureId))
            {
                existingTree.AssignArcGisFeatureId(
                    feature.Id);
            }





            if (existingTree is null)
            {
                var matchedSpecies =
                    species.FirstOrDefault(
                        s =>
                            s.CommonName.Equals(
                                feature.Species,
                                StringComparison.OrdinalIgnoreCase));



                var matchedPark =
                    parks.FirstOrDefault(
                        p =>
                            p.Name.Equals(
                                feature.Park,
                                StringComparison.OrdinalIgnoreCase));



                if (
                    matchedSpecies is null ||
                    matchedPark is null)
                {
                    actions.Add(
                        new SpatialSyncAction(
                            "Failed",
                            feature.AssetTag,
                            "Species or park could not be matched"));

                    continue;
                }



                var tree =
                    new Tree(
                        feature.AssetTag,
                        matchedSpecies,
                        matchedPark,
                        new GeoCoordinate(
                            feature.Latitude,
                            feature.Longitude),
                        Enum.Parse<TreeHealthStatus>(
                            feature.HealthStatus),
                        DateOnly.FromDateTime(
                            DateTime.Today),
                        0,
                        0);



                tree.AssignArcGisFeatureId(
                    feature.Id);



                await _treeRepository.AddAsync(
                    tree,
                    cancellationToken);



                created++;



                actions.Add(
                    new SpatialSyncAction(
                        "Create",
                        feature.AssetTag,
                        "Tree created in UFAMS"));



                continue;
            }






            var reasons =
                new List<string>();




            if (
                existingTree.HealthStatus.ToString()
                !=
                feature.HealthStatus)
            {
                existingTree.UpdateHealth(
                    Enum.Parse<TreeHealthStatus>(
                        feature.HealthStatus));


                reasons.Add(
                    "Health status updated");
            }





            if (
                LocationChanged(
                    existingTree,
                    feature))
            {
                existingTree.Relocate(
                    existingTree.Park,
                    new GeoCoordinate(
                        feature.Latitude,
                        feature.Longitude));


                reasons.Add(
                    "Location updated");
            }






            if (reasons.Count > 0)
            {
                updated++;

                actions.Add(
                    new SpatialSyncAction(
                        "Update",
                        feature.AssetTag,
                        string.Join(
                            ", ",
                            reasons)));

                continue;
            }






            unchanged++;

            actions.Add(
                new SpatialSyncAction(
                    "Unchanged",
                    feature.AssetTag,
                    "No changes detected"));
        }





        await _unitOfWork.SaveChangesAsync(
            cancellationToken);





        return new SpatialSyncResult(
            Created: created,
            Updated: updated,
            Deleted: 0,
            Unchanged: unchanged,
            Actions: actions);
    }







    private static bool LocationChanged(
        Tree tree,
        ArcGisFeature feature)
    {
        const double tolerance = 0.000001;


        return
            Math.Abs(
                tree.Location.Latitude -
                feature.Latitude)
            > tolerance
            ||
            Math.Abs(
                tree.Location.Longitude -
                feature.Longitude)
            > tolerance;
    }
}