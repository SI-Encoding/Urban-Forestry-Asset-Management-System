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

            var speciesChanged =
                !existingTree.Species.CommonName.Equals(
                    feature.Species,
                    StringComparison.OrdinalIgnoreCase);

            if (speciesChanged)
            {
                reasons.Add("Species changed");
            }

            var parkChanged =
                !existingTree.Park.Name.Equals(
                    feature.Park,
                    StringComparison.OrdinalIgnoreCase);

            if (parkChanged)
            {
                reasons.Add("Park changed");
            }

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
                        Action: "Update",
                        AssetTag: feature.AssetTag,
                        Reason: string.Join(", ", reasons),

                        UfamsSpecies: existingTree.Species.CommonName,
                        ArcGisSpecies: feature.Species,

                        UfamsPark: existingTree.Park.Name,
                        ArcGisPark: feature.Park,

                        UfamsHealthStatus: existingTree.HealthStatus.ToString(),
                        ArcGisHealthStatus: feature.HealthStatus,

                        UfamsLatitude: existingTree.Location.Latitude,
                        ArcGisLatitude: feature.Latitude,

                        UfamsLongitude: existingTree.Location.Longitude,
                        ArcGisLongitude: feature.Longitude
                    ));

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
    var actions = new List<SpatialSyncAction>();

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
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: "Species or park could not be matched",
                        UfamsSpecies: null,
                        ArcGisSpecies: feature.Species,
                        UfamsPark: null,
                        ArcGisPark: feature.Park,
                        UfamsHealthStatus: null,
                        ArcGisHealthStatus: feature.HealthStatus,
                        UfamsLatitude: null,
                        ArcGisLatitude: feature.Latitude,
                        UfamsLongitude: null,
                        ArcGisLongitude: feature.Longitude));

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
                    Action: "Create",
                    AssetTag: feature.AssetTag,
                    Reason: "Tree created in UFAMS",
                    UfamsSpecies: matchedSpecies.CommonName,
                    ArcGisSpecies: feature.Species,
                    UfamsPark: matchedPark.Name,
                    ArcGisPark: feature.Park,
                    UfamsHealthStatus: tree.HealthStatus.ToString(),
                    ArcGisHealthStatus: feature.HealthStatus,
                    UfamsLatitude: tree.Location.Latitude,
                    ArcGisLatitude: feature.Latitude,
                    UfamsLongitude: tree.Location.Longitude,
                    ArcGisLongitude: feature.Longitude));

            continue;
        }

        var reasons = new List<string>();

        if (!existingTree.Species.CommonName.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase))
        {
            var matchedSpecies =
                species.FirstOrDefault(
                    s =>
                        s.CommonName.Equals(
                            feature.Species,
                            StringComparison.OrdinalIgnoreCase));

            if (matchedSpecies is not null)
            {
                existingTree.ChangeSpecies(
                    matchedSpecies);

                reasons.Add(
                    "Species updated");
            }
        }

        if (!existingTree.Park.Name.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase))
        {
            var matchedPark =
                parks.FirstOrDefault(
                    p =>
                        p.Name.Equals(
                            feature.Park,
                            StringComparison.OrdinalIgnoreCase));

            if (matchedPark is not null)
            {
                existingTree.ChangePark(
                    matchedPark);

                reasons.Add(
                    "Park updated");
            }
        }

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
                    Action: "Update",
                    AssetTag: feature.AssetTag,
                    Reason: string.Join(", ", reasons),
                    UfamsSpecies: existingTree.Species.CommonName,
                    ArcGisSpecies: feature.Species,
                    UfamsPark: existingTree.Park.Name,
                    ArcGisPark: feature.Park,
                    UfamsHealthStatus: existingTree.HealthStatus.ToString(),
                    ArcGisHealthStatus: feature.HealthStatus,
                    UfamsLatitude: existingTree.Location.Latitude,
                    ArcGisLatitude: feature.Latitude,
                    UfamsLongitude: existingTree.Location.Longitude,
                    ArcGisLongitude: feature.Longitude));

            continue;
        }

        unchanged++;

        actions.Add(
            new SpatialSyncAction(
                Action: "Unchanged",
                AssetTag: feature.AssetTag,
                Reason: "No changes detected",
                UfamsSpecies: existingTree.Species.CommonName,
                ArcGisSpecies: feature.Species,
                UfamsPark: existingTree.Park.Name,
                ArcGisPark: feature.Park,
                UfamsHealthStatus: existingTree.HealthStatus.ToString(),
                ArcGisHealthStatus: feature.HealthStatus,
                UfamsLatitude: existingTree.Location.Latitude,
                ArcGisLatitude: feature.Latitude,
                UfamsLongitude: existingTree.Location.Longitude,
                ArcGisLongitude: feature.Longitude));
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

public async Task<SpatialSyncResult> ApplySingleAsync(
    string assetTag,
    CancellationToken cancellationToken = default)
{
    var actions =
        new List<SpatialSyncAction>();


    if (string.IsNullOrWhiteSpace(assetTag))
    {
        actions.Add(
            new SpatialSyncAction(
                Action: "Failed",
                AssetTag: assetTag,
                Reason: "Asset tag is required.",
                UfamsSpecies: null,
                ArcGisSpecies: null,
                UfamsPark: null,
                ArcGisPark: null,
                UfamsHealthStatus: null,
                ArcGisHealthStatus: null,
                UfamsLatitude: null,
                ArcGisLatitude: null,
                UfamsLongitude: null,
                ArcGisLongitude: null));

        return new SpatialSyncResult(
            Created: 0,
            Updated: 0,
            Deleted: 0,
            Unchanged: 0,
            Actions: actions);
    }


    var features =
        await _provider.GetFeaturesAsync(
            cancellationToken);


    var feature =
        features.FirstOrDefault(
            f =>
                f.AssetTag.Equals(
                    assetTag,
                    StringComparison.OrdinalIgnoreCase));


    if (feature is null)
    {
        actions.Add(
            new SpatialSyncAction(
                Action: "Failed",
                AssetTag: assetTag,
                Reason: "Tree was not found in ArcGIS.",
                UfamsSpecies: null,
                ArcGisSpecies: null,
                UfamsPark: null,
                ArcGisPark: null,
                UfamsHealthStatus: null,
                ArcGisHealthStatus: null,
                UfamsLatitude: null,
                ArcGisLatitude: null,
                UfamsLongitude: null,
                ArcGisLongitude: null));

        return new SpatialSyncResult(
            Created: 0,
            Updated: 0,
            Deleted: 0,
            Unchanged: 0,
            Actions: actions);
    }


    var trees =
        await _treeRepository.GetAllAsync(
            cancellationToken);


    var species =
        await _speciesRepository.GetAllAsync(
            cancellationToken);


    var parks =
        await _parkRepository.GetAllAsync(
            cancellationToken);


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
        actions.Add(
            new SpatialSyncAction(
                Action: "Failed",
                AssetTag: feature.AssetTag,
                Reason: "Tree does not exist in UFAMS.",
                UfamsSpecies: null,
                ArcGisSpecies: feature.Species,
                UfamsPark: null,
                ArcGisPark: feature.Park,
                UfamsHealthStatus: null,
                ArcGisHealthStatus: feature.HealthStatus,
                UfamsLatitude: null,
                ArcGisLatitude: feature.Latitude,
                UfamsLongitude: null,
                ArcGisLongitude: feature.Longitude));

        return new SpatialSyncResult(
            Created: 0,
            Updated: 0,
            Deleted: 0,
            Unchanged: 0,
            Actions: actions);
    }


    /*
     * Capture the UFAMS values BEFORE making any changes.
     * These are what we want to show in the sync result.
     */

    var originalUfamsSpecies =
        existingTree.Species.CommonName;

    var originalUfamsPark =
        existingTree.Park.Name;

    var originalUfamsHealthStatus =
        existingTree.HealthStatus.ToString();

    var originalUfamsLatitude =
        existingTree.Location.Latitude;

    var originalUfamsLongitude =
        existingTree.Location.Longitude;


    bool arcGisFeatureIdAssigned = false;


    if (
        string.IsNullOrWhiteSpace(
            existingTree.ArcGisFeatureId))
    {
        existingTree.AssignArcGisFeatureId(
            feature.Id);

        arcGisFeatureIdAssigned = true;
    }


    var reasons =
        new List<string>();


    /*
     * Species
     */

    var matchedSpecies =
        species.FirstOrDefault(
            s =>
                s.CommonName.Equals(
                    feature.Species,
                    StringComparison.OrdinalIgnoreCase));


    if (
        !originalUfamsSpecies.Equals(
            feature.Species,
            StringComparison.OrdinalIgnoreCase))
    {
        if (matchedSpecies is null)
        {
            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason:
                        $"ArcGIS species '{feature.Species}' could not be matched in UFAMS.",
                    UfamsSpecies: originalUfamsSpecies,
                    ArcGisSpecies: feature.Species,
                    UfamsPark: originalUfamsPark,
                    ArcGisPark: feature.Park,
                    UfamsHealthStatus: originalUfamsHealthStatus,
                    ArcGisHealthStatus: feature.HealthStatus,
                    UfamsLatitude: originalUfamsLatitude,
                    ArcGisLatitude: feature.Latitude,
                    UfamsLongitude: originalUfamsLongitude,
                    ArcGisLongitude: feature.Longitude));

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        existingTree.ChangeSpecies(
            matchedSpecies);

        reasons.Add(
            "Species updated");
    }


    /*
     * Park
     */

    var matchedPark =
        parks.FirstOrDefault(
            p =>
                p.Name.Equals(
                    feature.Park,
                    StringComparison.OrdinalIgnoreCase));


    if (
        !originalUfamsPark.Equals(
            feature.Park,
            StringComparison.OrdinalIgnoreCase))
    {
        if (matchedPark is null)
        {
            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason:
                        $"ArcGIS park '{feature.Park}' could not be matched in UFAMS.",
                    UfamsSpecies: originalUfamsSpecies,
                    ArcGisSpecies: feature.Species,
                    UfamsPark: originalUfamsPark,
                    ArcGisPark: feature.Park,
                    UfamsHealthStatus: originalUfamsHealthStatus,
                    ArcGisHealthStatus: feature.HealthStatus,
                    UfamsLatitude: originalUfamsLatitude,
                    ArcGisLatitude: feature.Latitude,
                    UfamsLongitude: originalUfamsLongitude,
                    ArcGisLongitude: feature.Longitude));

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        existingTree.ChangePark(
            matchedPark);

        reasons.Add(
            "Park updated");
    }


    /*
     * Health status
     */

    if (
        !originalUfamsHealthStatus.Equals(
            feature.HealthStatus,
            StringComparison.OrdinalIgnoreCase))
    {
        if (
            !Enum.TryParse<TreeHealthStatus>(
                feature.HealthStatus,
                true,
                out var healthStatus))
        {
            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason:
                        $"Invalid ArcGIS health status: {feature.HealthStatus}",
                    UfamsSpecies: originalUfamsSpecies,
                    ArcGisSpecies: feature.Species,
                    UfamsPark: originalUfamsPark,
                    ArcGisPark: feature.Park,
                    UfamsHealthStatus: originalUfamsHealthStatus,
                    ArcGisHealthStatus: feature.HealthStatus,
                    UfamsLatitude: originalUfamsLatitude,
                    ArcGisLatitude: feature.Latitude,
                    UfamsLongitude: originalUfamsLongitude,
                    ArcGisLongitude: feature.Longitude));

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        existingTree.UpdateHealth(
            healthStatus);

        reasons.Add(
            "Health status updated");
    }


    /*
     * Location
     */

    if (
        LocationChanged(
            existingTree,
            feature))
    {
        existingTree.Relocate(
            matchedPark ?? existingTree.Park,
            new GeoCoordinate(
                feature.Latitude,
                feature.Longitude));

        reasons.Add(
            "Location updated");
    }


    /*
     * No actual ArcGIS changes.
     */

    if (reasons.Count == 0)
    {
        if (arcGisFeatureIdAssigned)
        {
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }


        actions.Add(
            new SpatialSyncAction(
                Action: "Unchanged",
                AssetTag: feature.AssetTag,
                Reason: "No differences detected.",
                UfamsSpecies: originalUfamsSpecies,
                ArcGisSpecies: feature.Species,
                UfamsPark: originalUfamsPark,
                ArcGisPark: feature.Park,
                UfamsHealthStatus: originalUfamsHealthStatus,
                ArcGisHealthStatus: feature.HealthStatus,
                UfamsLatitude: originalUfamsLatitude,
                ArcGisLatitude: feature.Latitude,
                UfamsLongitude: originalUfamsLongitude,
                ArcGisLongitude: feature.Longitude));

        return new SpatialSyncResult(
            Created: 0,
            Updated: 0,
            Deleted: 0,
            Unchanged: 1,
            Actions: actions);
    }


    /*
     * Save the selected tree only.
     */

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);


    /*
     * Return the BEFORE vs AFTER comparison.
     */

    actions.Add(
        new SpatialSyncAction(
            Action: "Update",
            AssetTag: feature.AssetTag,
            Reason: string.Join(
                ", ",
                reasons),

            UfamsSpecies:
                originalUfamsSpecies,

            ArcGisSpecies:
                feature.Species,

            UfamsPark:
                originalUfamsPark,

            ArcGisPark:
                feature.Park,

            UfamsHealthStatus:
                originalUfamsHealthStatus,

            ArcGisHealthStatus:
                feature.HealthStatus,

            UfamsLatitude:
                originalUfamsLatitude,

            ArcGisLatitude:
                feature.Latitude,

            UfamsLongitude:
                originalUfamsLongitude,

            ArcGisLongitude:
                feature.Longitude));


    return new SpatialSyncResult(
        Created: 0,
        Updated: 1,
        Deleted: 0,
        Unchanged: 0,
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