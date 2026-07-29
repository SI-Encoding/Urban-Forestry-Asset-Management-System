"use client";

import { Marker, Popup } from "react-leaflet";
import { useEffect, useRef } from "react";
import type { Marker as LeafletMarker } from "leaflet";
import { treeIcons } from "./treeIcons";
import type { Tree } from "@/types/tree";

interface TreeMarkerProps {
    tree: Tree;
    selected?: boolean;
    onSelect: (tree: Tree) => void;
}

export function TreeMarker({
    tree,
    selected,
    onSelect,
}: TreeMarkerProps) {
    const markerRef = useRef<LeafletMarker>(null);

useEffect(() => {
    if (selected) {
        markerRef.current?.openPopup();
    } else {
        markerRef.current?.closePopup();
    }
}, [selected]);
    return (
        <Marker
            ref={markerRef}
            icon={
                treeIcons[
                    tree.healthStatus
                ]
            }
            position={[
                tree.location.latitude,
                tree.location.longitude,
            ]}
            eventHandlers={{
                click: () => onSelect(tree),
            }}
        >       
            <Popup>
                <strong>{tree.assetTag}</strong>

                <br />

                {tree.speciesName}

                <br />

                {tree.parkName}

                <br />

                {tree.healthStatus}
            </Popup>
        </Marker>
    );
}