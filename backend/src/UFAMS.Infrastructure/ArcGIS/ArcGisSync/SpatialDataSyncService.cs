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

    private readonly ISyncAuditRepository _syncAuditRepository;

    public SpatialDataSyncService(
        IArcGisFeatureProvider provider,
        ITreeRepository treeRepository,
        ISpeciesRepository speciesRepository,
        IParkRepository parkRepository,
        IUnitOfWork unitOfWork,
        ISyncAuditRepository syncAuditRepository)
    {
        _provider = provider;
        _treeRepository = treeRepository;
        _speciesRepository = speciesRepository;
        _parkRepository = parkRepository;
        _unitOfWork = unitOfWork;
        _syncAuditRepository = syncAuditRepository;
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
    var audit =
        new SyncAudit(
            DateTime.UtcNow);

    await _syncAuditRepository.AddAsync(
        audit,
        cancellationToken);

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
    int failed = 0;


    foreach (var feature in features)
    {
        /*
         * Find the existing UFAMS tree.
         *
         * Prefer ArcGIS Feature ID when available,
         * otherwise fall back to AssetTag.
         */

        var existingTree =
            trees.FirstOrDefault(
                tree =>
                    tree.ArcGisFeatureId == feature.Id
                    ||
                    tree.AssetTag.Equals(
                        feature.AssetTag,
                        StringComparison.OrdinalIgnoreCase));


        /*
         * ============================================================
         * CREATE
         * ============================================================
         */

        if (existingTree is null)
        {
            /*
             * Resolve species and park before creating anything.
             */

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


            /*
             * Validate species.
             */

            if (matchedSpecies is null)
            {
                var reason =
                    $"ArcGIS species '{feature.Species}' could not be matched in UFAMS.";

                actions.Add(
                    new SpatialSyncAction(
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: reason,
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

                audit.AddEntry(
                    new SyncAuditEntry(
                        feature.AssetTag,
                        "Failed",
                        reason));

                failed++;

                continue;
            }


            /*
             * Validate park.
             */

            if (matchedPark is null)
            {
                var reason =
                    $"ArcGIS park '{feature.Park}' could not be matched in UFAMS.";

                actions.Add(
                    new SpatialSyncAction(
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: reason,
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

                audit.AddEntry(
                    new SyncAuditEntry(
                        feature.AssetTag,
                        "Failed",
                        reason));

                failed++;

                continue;
            }


            /*
             * Validate health status.
             */

            if (
                !Enum.TryParse<TreeHealthStatus>(
                    feature.HealthStatus,
                    true,
                    out var healthStatus))
            {
                var reason =
                    $"Invalid ArcGIS health status: {feature.HealthStatus}";

                actions.Add(
                    new SpatialSyncAction(
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: reason,
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

                audit.AddEntry(
                    new SyncAuditEntry(
                        feature.AssetTag,
                        "Failed",
                        reason));

                failed++;

                continue;
            }


            /*
             * All validation passed.
             *
             * Now it is safe to create the Tree.
             */

            var tree =
                new Tree(
                    feature.AssetTag,
                    matchedSpecies,
                    matchedPark,
                    new GeoCoordinate(
                        feature.Latitude,
                        feature.Longitude),
                    healthStatus,
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
                    UfamsSpecies:
                        matchedSpecies.CommonName,
                    ArcGisSpecies:
                        feature.Species,
                    UfamsPark:
                        matchedPark.Name,
                    ArcGisPark:
                        feature.Park,
                    UfamsHealthStatus:
                        tree.HealthStatus.ToString(),
                    ArcGisHealthStatus:
                        feature.HealthStatus,
                    UfamsLatitude:
                        tree.Location.Latitude,
                    ArcGisLatitude:
                        feature.Latitude,
                    UfamsLongitude:
                        tree.Location.Longitude,
                    ArcGisLongitude:
                        feature.Longitude));


            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Create",
                    "Tree created in UFAMS"));


            continue;
        }


        /*
         * ============================================================
         * EXISTING TREE
         * ============================================================
         *
         * Capture the original values before making any changes.
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


        /*
         * ============================================================
         * VALIDATION PHASE
         * ============================================================
         *
         * Nothing is changed on the Tree during this section.
         */


        /*
         * Resolve species.
         */

        var matchedSpeciesForUpdate =
            species.FirstOrDefault(
                s =>
                    s.CommonName.Equals(
                        feature.Species,
                        StringComparison.OrdinalIgnoreCase));


        /*
         * Resolve park.
         */

        var matchedParkForUpdate =
            parks.FirstOrDefault(
                p =>
                    p.Name.Equals(
                        feature.Park,
                        StringComparison.OrdinalIgnoreCase));


        /*
         * Validate species mismatch.
         */

        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase)
            &&
            matchedSpeciesForUpdate is null)
        {
            var reason =
                $"ArcGIS species '{feature.Species}' could not be matched in UFAMS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason: reason,
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


            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Failed",
                    reason));


            failed++;

            continue;
        }


        /*
         * Validate park mismatch.
         */

        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase)
            &&
            matchedParkForUpdate is null)
        {
            var reason =
                $"ArcGIS park '{feature.Park}' could not be matched in UFAMS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason: reason,
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


            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Failed",
                    reason));


            failed++;

            continue;
        }


        /*
         * Validate health status.
         */

        var healthStatusForUpdate =
            existingTree.HealthStatus;


        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            if (
                !Enum.TryParse<TreeHealthStatus>(
                    feature.HealthStatus,
                    true,
                    out healthStatusForUpdate))
            {
                var reason =
                    $"Invalid ArcGIS health status: {feature.HealthStatus}";

                actions.Add(
                    new SpatialSyncAction(
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: reason,
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


                audit.AddEntry(
                    new SyncAuditEntry(
                        feature.AssetTag,
                        "Failed",
                        reason));


                failed++;

                continue;
            }
        }


        /*
         * Determine whether the location changed.
         */

        var locationChanged =
            LocationChanged(
                existingTree,
                feature);


        /*
         * ============================================================
         * DETERMINE CHANGES
         * ============================================================
         */

        var reasons =
            new List<string>();


        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Species updated");
        }


        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Park updated");
        }


        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Health status updated");
        }


        if (locationChanged)
        {
            reasons.Add(
                "Location updated");
        }


        /*
         * ============================================================
         * UNCHANGED
         * ============================================================
         */

        if (reasons.Count == 0)
        {
            /*
             * The tree itself does not need updating.
             *
             * However, if this is the first time we have seen
             * this ArcGIS feature, we still need to establish
             * the external identity relationship.
             */

            if (
                string.IsNullOrWhiteSpace(
                    existingTree.ArcGisFeatureId))
            {
                existingTree.AssignArcGisFeatureId(
                    feature.Id);
            }


            unchanged++;


            actions.Add(
                new SpatialSyncAction(
                    Action: "Unchanged",
                    AssetTag: feature.AssetTag,
                    Reason: "No changes detected",
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


            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Unchanged",
                    "No changes detected"));


            continue;
        }


        /*
         * ============================================================
         * APPLY VALIDATED CHANGES
         * ============================================================
         *
         * At this point ALL validation has passed.
         * It is now safe to modify the Tree.
         */


        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.ChangeSpecies(
                matchedSpeciesForUpdate!);
        }


        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.ChangePark(
                matchedParkForUpdate!);
        }


        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.UpdateHealth(
                healthStatusForUpdate);
        }


        if (locationChanged)
        {
            existingTree.Relocate(
                matchedParkForUpdate ?? existingTree.Park,
                new GeoCoordinate(
                    feature.Latitude,
                    feature.Longitude));
        }


        /*
         * Assign ArcGIS Feature ID if it has not been established.
         */

        if (
            string.IsNullOrWhiteSpace(
                existingTree.ArcGisFeatureId))
        {
            existingTree.AssignArcGisFeatureId(
                feature.Id);
        }


        /*
         * Successful update.
         */

        updated++;


        var updateReason =
            string.Join(
                ", ",
                reasons);


        actions.Add(
            new SpatialSyncAction(
                Action: "Update",
                AssetTag: feature.AssetTag,
                Reason: updateReason,
                UfamsSpecies:
                    existingTree.Species.CommonName,
                ArcGisSpecies:
                    feature.Species,
                UfamsPark:
                    existingTree.Park.Name,
                ArcGisPark:
                    feature.Park,
                UfamsHealthStatus:
                    existingTree.HealthStatus.ToString(),
                ArcGisHealthStatus:
                    feature.HealthStatus,
                UfamsLatitude:
                    existingTree.Location.Latitude,
                ArcGisLatitude:
                    feature.Latitude,
                UfamsLongitude:
                    existingTree.Location.Longitude,
                ArcGisLongitude:
                    feature.Longitude));


        audit.AddEntry(
            new SyncAuditEntry(
                feature.AssetTag,
                "Update",
                updateReason));
    }


    /*
     * ================================================================
     * COMPLETE AUDIT
     * ================================================================
     */

    audit.Complete(
        created,
        updated,
        failed,
        unchanged);


    /*
     * ================================================================
     * SINGLE DATABASE SAVE
     * ================================================================
     *
     * SyncAudit, SyncAuditEntry, Tree changes, and newly-created
     * Trees are all tracked by the same UFAMSDbContext.
     *
     * Therefore this single SaveChangesAsync call persists the
     * entire synchronization operation together.
     */

    await _unitOfWork.SaveChangesAsync(
        cancellationToken);


    /*
     * ================================================================
     * RETURN RESULT
     * ================================================================
     */

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
    await _unitOfWork.BeginTransactionAsync(
        cancellationToken);

    try
    {
        var audit =
            new SyncAudit(
                DateTime.UtcNow);

        await _syncAuditRepository.AddAsync(
            audit,
            cancellationToken);

        var actions =
            new List<SpatialSyncAction>();


        /*
         * Asset tag validation
         */

        if (string.IsNullOrWhiteSpace(assetTag))
        {
            var reason =
                "Asset tag is required.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: assetTag,
                    Reason: reason,
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

            audit.AddEntry(
                new SyncAuditEntry(
                    assetTag,
                    "Failed",
                    reason));

            audit.Complete(
                0,
                0,
                1,
                0);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        /*
         * Retrieve ArcGIS feature.
         */

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
            var reason =
                "Tree was not found in ArcGIS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: assetTag,
                    Reason: reason,
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

            audit.AddEntry(
                new SyncAuditEntry(
                    assetTag,
                    "Failed",
                    reason));

            audit.Complete(
                0,
                0,
                1,
                0);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        /*
         * Load UFAMS data.
         */

        var trees =
            await _treeRepository.GetAllAsync(
                cancellationToken);

        var species =
            await _speciesRepository.GetAllAsync(
                cancellationToken);

        var parks =
            await _parkRepository.GetAllAsync(
                cancellationToken);


        /*
         * Locate the existing UFAMS tree.
         */

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
            var reason =
                "Tree does not exist in UFAMS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason: reason,
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

            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Failed",
                    reason));

            audit.Complete(
                0,
                0,
                1,
                0);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        /*
         * Capture original UFAMS values
         * before making any changes.
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


        /*
         * Resolve species and park.
         */

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


        /*
         * Validate species BEFORE modifying the Tree.
         */

        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase)
            &&
            matchedSpecies is null)
        {
            var reason =
                $"ArcGIS species '{feature.Species}' could not be matched in UFAMS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason: reason,
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

            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Failed",
                    reason));

            audit.Complete(
                0,
                0,
                1,
                0);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        /*
         * Validate park BEFORE modifying the Tree.
         */

        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase)
            &&
            matchedPark is null)
        {
            var reason =
                $"ArcGIS park '{feature.Park}' could not be matched in UFAMS.";

            actions.Add(
                new SpatialSyncAction(
                    Action: "Failed",
                    AssetTag: feature.AssetTag,
                    Reason: reason,
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

            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Failed",
                    reason));

            audit.Complete(
                0,
                0,
                1,
                0);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 0,
                Actions: actions);
        }


        /*
         * Validate health status BEFORE modifying the Tree.
         */

        TreeHealthStatus healthStatus =
            existingTree.HealthStatus;

        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            if (
                !Enum.TryParse<TreeHealthStatus>(
                    feature.HealthStatus,
                    true,
                    out healthStatus))
            {
                var reason =
                    $"Invalid ArcGIS health status: {feature.HealthStatus}";

                actions.Add(
                    new SpatialSyncAction(
                        Action: "Failed",
                        AssetTag: feature.AssetTag,
                        Reason: reason,
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

                audit.AddEntry(
                    new SyncAuditEntry(
                        feature.AssetTag,
                        "Failed",
                        reason));

                audit.Complete(
                    0,
                    0,
                    1,
                    0);

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);

                await _unitOfWork.CommitTransactionAsync(
                    cancellationToken);

                return new SpatialSyncResult(
                    Created: 0,
                    Updated: 0,
                    Deleted: 0,
                    Unchanged: 0,
                    Actions: actions);
            }
        }


        /*
         * Determine what will change.
         */

        var reasons =
            new List<string>();

        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Species updated");
        }

        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Park updated");
        }

        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            reasons.Add(
                "Health status updated");
        }

        var locationChanged =
            LocationChanged(
                existingTree,
                feature);

        if (locationChanged)
        {
            reasons.Add(
                "Location updated");
        }


        /*
         * Assign ArcGIS Feature ID if necessary.
         */

        var arcGisFeatureIdAssigned =
            false;

        if (
            string.IsNullOrWhiteSpace(
                existingTree.ArcGisFeatureId))
        {
            existingTree.AssignArcGisFeatureId(
                feature.Id);

            arcGisFeatureIdAssigned = true;
        }


        /*
         * No actual changes.
         */

        if (reasons.Count == 0)
        {
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

            audit.AddEntry(
                new SyncAuditEntry(
                    feature.AssetTag,
                    "Unchanged",
                    "No differences detected."));

            audit.Complete(
                0,
                0,
                0,
                1);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            await _unitOfWork.CommitTransactionAsync(
                cancellationToken);

            return new SpatialSyncResult(
                Created: 0,
                Updated: 0,
                Deleted: 0,
                Unchanged: 1,
                Actions: actions);
        }


        /*
         * Apply validated changes.
         */

        if (
            !originalUfamsSpecies.Equals(
                feature.Species,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.ChangeSpecies(
                matchedSpecies!);
        }

        if (
            !originalUfamsPark.Equals(
                feature.Park,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.ChangePark(
                matchedPark!);
        }

        if (
            !originalUfamsHealthStatus.Equals(
                feature.HealthStatus,
                StringComparison.OrdinalIgnoreCase))
        {
            existingTree.UpdateHealth(
                healthStatus);
        }

        if (locationChanged)
        {
            existingTree.Relocate(
                matchedPark ?? existingTree.Park,
                new GeoCoordinate(
                    feature.Latitude,
                    feature.Longitude));
        }


        /*
         * Record successful update.
         */

        var updateReason =
            string.Join(
                ", ",
                reasons);

        actions.Add(
            new SpatialSyncAction(
                Action: "Update",
                AssetTag: feature.AssetTag,
                Reason: updateReason,
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

        audit.AddEntry(
            new SyncAuditEntry(
                feature.AssetTag,
                "Update",
                updateReason));

        audit.Complete(
            0,
            1,
            0,
            0);


        /*
         * Save Tree + Audit together.
         */

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        await _unitOfWork.CommitTransactionAsync(
            cancellationToken);


        return new SpatialSyncResult(
            Created: 0,
            Updated: 1,
            Deleted: 0,
            Unchanged: 0,
            Actions: actions);
    }
    catch
    {
        await _unitOfWork.RollbackTransactionAsync(
            cancellationToken);

        throw;
    }
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