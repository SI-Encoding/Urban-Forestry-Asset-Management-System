import type { GeoCoordinate } from "./geo-coordinate";

export type TreeHealthStatus =
    | "Excellent"
    | "Good"
    | "Fair"
    | "Poor"
    | "Dead";

export interface TreeLocation {
    latitude: number;
    longitude: number;
}

export interface Tree {
    id: string;
    assetTag: string;
    speciesName: string;
    parkName: string;
    location: GeoCoordinate;
    healthStatus: TreeHealthStatus;
}