import { apiFetch, apiPost } from "@/services/api-client";

import type {
    SpatialSyncResult,
} from "@/types/arcgisSync";


export async function getArcGisSyncPreview(): Promise<SpatialSyncResult> {

    return apiFetch<SpatialSyncResult>(
        "/arcgis/sync/preview"
    );

}


export async function applyArcGisSync(): Promise<SpatialSyncResult> {

    return apiPost<SpatialSyncResult>(
        "/arcgis/sync/apply",
        {}
    );

}


export async function applySingleArcGisSync(
    assetTag: string
): Promise<SpatialSyncResult> {

    return apiPost<SpatialSyncResult>(
        `/arcgis/sync/apply/${encodeURIComponent(assetTag)}`,
        {}
    );

}