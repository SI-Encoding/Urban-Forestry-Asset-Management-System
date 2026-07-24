"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";

import { TreeMap } from "./TreeMap";
import { TreeDetailsPanel } from "./TreeDetailsPanel";
import { MapLegend } from "./MapLegend";

interface TreeMapContainerProps {
    trees: Tree[];
}

export function TreeMapContainer({
    trees,
}: TreeMapContainerProps) {

    const [selectedTree, setSelectedTree] =
        useState<Tree>();

    return (
        <div className="grid grid-cols-3 gap-6">

            <div className="col-span-2">
                <TreeMap
                    trees={trees}
                    onSelectTree={setSelectedTree}
                />
                <MapLegend />
            </div>

            <TreeDetailsPanel
                tree={selectedTree}
            />

        </div>
    );
}