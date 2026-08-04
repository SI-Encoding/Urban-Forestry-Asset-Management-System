import { apiFetch, apiPost } from "./api-client";

import type {
    SpatialSyncResult,
} from "@/types/arcgisSync";

export function getArcGisSyncPreview() {
    return apiFetch<SpatialSyncResult>(
        "/arcgis/sync/preview"
    );
}

export function applyArcGisSync() {

    return apiPost<SpatialSyncResult>(
        "/arcgis/sync/apply",
        {}
    );

}