export interface SpatialSyncAction {
    action: string;
    assetTag: string;
    reason: string;
}

export interface SpatialSyncResult {
    created: number;
    updated: number;
    deleted: number;
    unchanged: number;
    actions: SpatialSyncAction[];
}