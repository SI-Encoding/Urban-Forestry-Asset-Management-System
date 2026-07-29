"use client";

import dynamic from "next/dynamic";

import type { Tree } from "@/types/tree";

interface TreeMapProps {
    trees: Tree[];
    selectedTree?: Tree;
    onSelectTree: (tree: Tree) => void;
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
    onSelectTree
}: TreeMapProps) {
    return (
        <ClientTreeMap
            trees={trees}
            selectedTree={selectedTree}
            onSelectTree={onSelectTree}
        />
    );
}