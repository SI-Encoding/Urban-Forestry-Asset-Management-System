import { apiFetch } from "./api-client";

export function getGeoJson() {
    return apiFetch<object>(
        "/trees/geojson"
    );
}