export interface SpatialSyncAction {
    action: string;
    assetTag: string;
    reason: string;

    ufamsSpecies?: string;
    arcGisSpecies?: string;

    ufamsPark?: string;
    arcGisPark?: string;

    ufamsHealthStatus?: string;
    arcGisHealthStatus?: string;

    ufamsLatitude?: number;
    arcGisLatitude?: number;

    ufamsLongitude?: number;
    arcGisLongitude?: number;
}

export interface SpatialSyncResult {
    created: number;
    updated: number;
    deleted: number;
    unchanged: number;
    actions: SpatialSyncAction[];
}