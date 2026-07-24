"use client";

import { useEffect } from "react";

import { MapContainer, TileLayer } from "react-leaflet";

import type { Tree } from "@/types/tree";
import { TreeMarker } from "./TreeMarker";

import { configureLeaflet } from "@/lib/leaflet";

import "leaflet/dist/leaflet.css";

interface ClientTreeMapProps {
    trees: Tree[];
    onSelectTree: (tree: Tree) => void;
}

export function ClientTreeMap({
    trees,
    onSelectTree,
}: ClientTreeMapProps) {
    useEffect(() => {
    async function setup() {
        await configureLeaflet();
    }

    setup();
}, []);

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

            {trees.map((tree) => (
                <TreeMarker
        key={tree.id}
        tree={tree}
        onSelect={onSelectTree}
    />
            ))}
        </MapContainer>
    );
}