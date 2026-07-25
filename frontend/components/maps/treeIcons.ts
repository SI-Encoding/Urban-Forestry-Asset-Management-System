import { TreeHealthStatus } from "@/types/tree";
import L from "leaflet";

const shadow = "/leaflet/marker-shadow.png";

function createIcon(iconUrl: string) {
    return new L.Icon({
        iconUrl,
        shadowUrl: shadow,

        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41],
    });
}

export const treeIcons: Record<TreeHealthStatus, L.Icon> = {

    Excellent: createIcon(
        "/markers/blue.png"
    ),

    Good: createIcon(
        "/markers/green.png"
    ),

    Fair: createIcon(
        "/markers/yellow.png"
    ),

    Poor: createIcon(
        "/markers/orange.png"
    ),

    Dead: createIcon(
        "/markers/red.png"
    ),

};