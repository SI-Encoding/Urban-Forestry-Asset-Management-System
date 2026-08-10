"use client";

import { useEffect } from "react";

import L from "leaflet";
import "leaflet.heat";

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
    showHeatMap?: boolean;
}

function FitToTrees({
    trees,
}: {
    trees: Tree[];
}) {
    const map = useMap();

    useEffect(() => {

        if (trees.length === 0) {
            return;
        }

        const bounds = L.latLngBounds(
            trees.map(tree => [
                tree.location.latitude,
                tree.location.longitude,
            ])
        );

        map.fitBounds(
            bounds,
            {
                padding: [50, 50],
            }
        );

    }, [trees, map]);

    return null;
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

function TreeHealthHeatMap({
    trees,
    enabled,
}: {
    trees: Tree[];
    enabled: boolean;
}) {
    const map = useMap();

    useEffect(() => {

        if (!enabled || trees.length === 0) {
            return;
        }

        const heatPoints = trees.map(tree => {

            let intensity = 0.2;

            switch (tree.healthStatus) {

                case "Excellent":
                    intensity = 0.1;
                    break;

                case "Good":
                    intensity = 0.3;
                    break;

                case "Fair":
                    intensity = 0.6;
                    break;

                case "Poor":
                    intensity = 0.9;
                    break;

                case "Dead":
                    intensity = 1.0;
                    break;

            }

            return [
                tree.location.latitude,
                tree.location.longitude,
                intensity,
            ] as [number, number, number];

        });

        const heatLayer =
            L.heatLayer(
                heatPoints,
                {
                    radius: 35,
                    blur: 25,
                    maxZoom: 17,
                    max: 1.0,
                }
            );

        heatLayer.addTo(map);

        return () => {

            map.removeLayer(
                heatLayer
            );

        };

    }, [trees, enabled, map]);

    return null;
}

export function ClientTreeMap({
    trees,
    selectedTree,
    onSelectTree,
    showHeatMap = false,
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

            <FitToTrees
                trees={trees}
            />

            <FlyToSelectedTree
                tree={selectedTree}
            />

            <TreeHealthHeatMap
                trees={trees}
                enabled={showHeatMap}
            />

            {trees.map(tree => (

                <TreeMarker
                    key={tree.id}
                    tree={tree}
                    selected={
                        tree.id === selectedTree?.id
                    }
                    onSelect={onSelectTree}
                />

            ))}

        </MapContainer>

    );
}