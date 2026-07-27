"use client";

import { useEffect, useState } from "react";

import type { Tree, TreeHealthStatus } from "@/types/tree";

import { TreeMap } from "./TreeMap";
import { TreeDetailsPanel } from "./TreeDetailsPanel";
import { MapToolbar } from "./MapToolbar";

import { getTreeInspections } from "@/services/inspections";
import type { Inspection } from "@/types/inspection";
import { getTreeWorkOrders } from "@/services/workOrders";
import type { WorkOrder } from "@/types/workOrder";

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

    const [inspections, setInspections] =
        useState<Inspection[]>([]);

    const [workOrders, setWorkOrders] =
        useState<WorkOrder[]>([]);

    const [isLoadingInspections, setIsLoadingInspections] =
        useState(false);

    const [inspectionError, setInspectionError] =
        useState<string | null>(null);

    const [selectedHealth, setSelectedHealth] =
        useState<TreeHealthStatus[]>([
            "Excellent",
            "Good",
            "Fair",
            "Poor",
            "Dead",
        ]);
    
    async function loadInspections(
    treeId: string
) {

    try {

        setIsLoadingInspections(true);

        setInspectionError(null);

        const data =
            await getTreeInspections(treeId);

        setInspections(data);


    } catch (error) {

        console.error(
            "Failed to load inspections:",
            error
        );


        setInspectionError(
            "Unable to load inspections."
        );


        setInspections([]);


    } finally {

        setIsLoadingInspections(false);

    }

}

    async function loadWorkOrders(
    treeId: string
) {

    try {

        const data =
            await getTreeWorkOrders(treeId);

        setWorkOrders(data);

    } catch (error) {

        console.error(
            "Failed to load work orders:",
            error
        );

        setWorkOrders([]);

    }

}

    useEffect(() => {

    if (!selectedTree) {
        return;
    }

    const treeId = selectedTree.id;

    const timeout = setTimeout(() => {

        loadInspections(treeId);
        loadWorkOrders(treeId);
    }, 0);


    return () => {
        clearTimeout(timeout);
    };

}, [selectedTree]);

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

        <div className="flex min-h-0 flex-1">

            <div className="flex-1">

                <TreeMap
                    trees={filteredTrees}
                    onSelectTree={setSelectedTree}
                />
            </div>

            <TreeDetailsPanel
                tree={selectedTree}
                inspections={inspections}
                workOrders={workOrders}
                onInspectionsChanged={() => {
                    if (selectedTree) {
                        loadInspections(selectedTree.id);
                    }
                }}
                onWorkOrdersChanged={() => {
                    if (selectedTree) {
                        loadWorkOrders(selectedTree.id);
                    }
                }}
            />

        </div>

    </div>
);
}