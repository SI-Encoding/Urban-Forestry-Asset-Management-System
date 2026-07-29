"use client";

import { useState } from "react";

import {
    usePathname,
    useRouter,
    useSearchParams,
} from "next/navigation";

import type {
    Tree,
    TreeHealthStatus,
} from "@/types/tree";

import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";

import { TreeMap } from "./TreeMap";
import { TreeDetailsPanel } from "./TreeDetailsPanel";
import { MapToolbar } from "./MapToolbar";

interface TreeMapContainerProps {
    trees: Tree[];
    selectedTreeId?: string;
    inspections: Inspection[];
    workOrders: WorkOrder[];
}

export function TreeMapContainer({
    trees,
    selectedTreeId,
    inspections,
    workOrders,
}: TreeMapContainerProps) {

    const router = useRouter();

    const pathname = usePathname();

    const searchParams =
        useSearchParams();

    const [search, setSearch] =
        useState("");

    const [selectedHealth, setSelectedHealth] =
        useState<TreeHealthStatus[]>([
            "Excellent",
            "Good",
            "Fair",
            "Poor",
            "Dead",
        ]);

    const selectedTree =
        selectedTreeId
            ? trees.find(
                tree => tree.id === selectedTreeId
            )
            : undefined;

    function toggleHealth(
        health: TreeHealthStatus
    ) {

        setSelectedHealth(current =>

            current.includes(health)

                ? current.filter(
                    x => x !== health
                )

                : [
                    ...current,
                    health,
                ]

        );

    }

    function handleTreeSelected(
        tree?: Tree
    ) {

        if (!tree) {
            return;
        }

        const params =
            new URLSearchParams(
                searchParams.toString()
            );

        params.set(
            "treeId",
            tree.id
        );

        router.replace(
            `${pathname}?${params.toString()}`,
            {
                scroll: false,
            }
        );

    }

    const filteredTrees =
        trees.filter(tree => {

            const query =
                search.toLowerCase();

            return (

                selectedHealth.includes(
                    tree.healthStatus
                )

                &&

                (

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
                        .includes(query)

                )

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

            <div className="flex min-h-0 flex-1">

                <div className="flex-1">

                    <TreeMap
                        trees={filteredTrees}
                        selectedTree={selectedTree}
                        onSelectTree={handleTreeSelected}
                    />

                </div>

                <TreeDetailsPanel
                    tree={selectedTree}
                    inspections={inspections}
                    workOrders={workOrders}
                />

            </div>

        </div>

    );

}