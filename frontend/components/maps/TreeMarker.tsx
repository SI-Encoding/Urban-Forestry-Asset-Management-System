"use client";

import { Marker, Popup } from "react-leaflet";
import { treeIcons } from "./treeIcons";
import type { Tree } from "@/types/tree";

interface TreeMarkerProps {
    tree: Tree;
    onSelect: (tree: Tree) => void;
}

export function TreeMarker({
    tree,
    onSelect,
}: TreeMarkerProps) {
    return (
        <Marker
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