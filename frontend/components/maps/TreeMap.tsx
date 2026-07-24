"use client";
import "@/lib/leaflet";
import { MapContainer, TileLayer } from "react-leaflet";

import "leaflet/dist/leaflet.css";

export function TreeMap() {
    return (
        <MapContainer
            center={[49.2827, -123.1207]}
            zoom={12}
            style={{
                height: "700px",
                width: "100%",
            }}
        >
            <TileLayer
                attribution="OpenStreetMap"
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
        </MapContainer>
    );
}