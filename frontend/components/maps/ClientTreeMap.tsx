"use client";

import { useEffect } from "react";

import {
    MapContainer,
    TileLayer,
    useMap,
} from "react-leaflet";

import type { Tree } from "@/types/tree";
import { TreeMarker } from "./TreeMarker";

import { configureLeaflet } from "@/lib/leaflet";

import "leaflet/dist/leaflet.css";

interface ClientTreeMapProps {
    trees: Tree[];
    selectedTree?: Tree;
    onSelectTree: (tree: Tree) => void;
}

function FlyToSelectedTree({
    tree,
}: {
    tree?: Tree;
}) {
    const map = useMap();

    useEffect(() => {
        if (!tree) {
            return;
        }

        map.flyTo(
            [
                tree.location.latitude,
                tree.location.longitude,
            ],
            18,
            {
                animate: true,
                duration: 1.5,
            }
        );
    }, [tree, map]);

    return null;
}

export function ClientTreeMap({
    trees,
    selectedTree,
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
                height: "calc(100vh - 170px)",
                width: "100%",
            }}
        >
            <TileLayer
                attribution="OpenStreetMap"
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <FlyToSelectedTree
                tree={selectedTree}
            />
            {trees.map((tree) => (
                <TreeMarker
                    key={tree.id}
                    tree={tree}
                    selected={tree.id === selectedTree?.id}
                    onSelect={onSelectTree}
                />
            ))}
        </MapContainer>
    );
}