import type { GeoCoordinate } from "./geo-coordinate";

export type TreeHealthStatus =
    | "Good"
    | "Fair"
    | "Poor"
    | "Critical";

export interface Tree {
    id: string;
    assetTag: string;
    speciesName: string;
    parkName: string;
    location: GeoCoordinate;
    healthStatus: TreeHealthStatus;
}