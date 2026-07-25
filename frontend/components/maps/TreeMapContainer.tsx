"use client";

import { useState } from "react";

import type { Tree, TreeHealthStatus } from "@/types/tree";

import { TreeMap } from "./TreeMap";
import { TreeDetailsPanel } from "./TreeDetailsPanel";
import { MapToolbar } from "./MapToolbar";

interface TreeMapContainerProps {
    trees: Tree[];
}

export function TreeMapContainer({
    trees,
}: TreeMapContainerProps) {

    const [search, setSearch] =
    useState("");

    const [selectedTree, setSelectedTree] =
        useState<Tree>();

    const [selectedHealth, setSelectedHealth] =
        useState<TreeHealthStatus[]>([
            "Excellent",
            "Good",
            "Fair",
            "Poor",
            "Dead",
        ]);

    function toggleHealth(
        health: TreeHealthStatus
    ) {
        setSelectedHealth((current) =>

            current.includes(health)
                ? current.filter(
                    (x) => x !== health
                )
                : [...current, health]

        );
    }
    const filteredTrees =
    trees.filter((tree) => {

        const matchesHealth =
            selectedHealth.includes(
                tree.healthStatus
            );


        const query =
            search.toLowerCase();


        const matchesSearch =
            tree.assetTag
                .toLowerCase()
                .includes(query)
            ||
            tree.speciesName
                .toLowerCase()
                .includes(query)
            ||
            tree.parkName
                .toLowerCase()
                .includes(query);


        return (
            matchesHealth &&
            matchesSearch
        );
    });
    return (
    <div className="flex h-full flex-col gap-4">

        <MapToolbar
            selected={selectedHealth}
            onChange={toggleHealth}
            search={search}
            onSearchChange={setSearch}
        />

        <div className="flex flex-1">

            <div className="flex-1">

                <TreeMap
                    trees={filteredTrees}
                    onSelectTree={setSelectedTree}
                />
            </div>

            <TreeDetailsPanel
                tree={selectedTree}
            />

        </div>

    </div>
);
}