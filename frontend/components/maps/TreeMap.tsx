"use client";

import dynamic from "next/dynamic";

import type { Tree } from "@/types/tree";

interface TreeMapProps {
    trees: Tree[];
    selectedTree?: Tree;
    onSelectTree: (tree: Tree) => void;
    showHeatMap: boolean;
}

const ClientTreeMap = dynamic(
    () =>
        import("./ClientTreeMap").then(
            (module) => module.ClientTreeMap
        ),
    {
        ssr: false,
        loading: () => (
            <div>
                Loading map...
            </div>
        ),
    }
);

export function TreeMap({
    trees,
    selectedTree,
    onSelectTree,
    showHeatMap,
}: TreeMapProps) {
    return (
        <ClientTreeMap
            trees={trees}
            selectedTree={selectedTree}
            onSelectTree={onSelectTree}
            showHeatMap={showHeatMap}
        />
    );
}