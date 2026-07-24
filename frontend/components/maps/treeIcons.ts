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

export const treeIcons = {
    Good: createIcon("/markers/green.png"),
    Fair: createIcon("/markers/yellow.png"),
    Poor: createIcon("/markers/orange.png"),
    Critical: createIcon("/markers/red.png"),
};