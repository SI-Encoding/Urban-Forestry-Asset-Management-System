"use client";

import {
    useEffect,
    useState,
} from "react";

import {
    useSearchParams,
} from "next/navigation";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";

import { getTrees } from "@/services/trees";
import { getTreeInspections } from "@/services/inspections";
import { getTreeWorkOrders } from "@/services/workOrders";

import { TreeMapContainer } from "@/components/maps/TreeMapContainer";

export function MapPageClient() {

    const searchParams =
        useSearchParams();

    const treeId =
        searchParams.get("treeId")
        ?? undefined;


    const [
        trees,
        setTrees,
    ] = useState<Tree[]>([]);


    const [
        inspections,
        setInspections,
    ] = useState<Inspection[]>([]);


    const [
        workOrders,
        setWorkOrders,
    ] = useState<WorkOrder[]>([]);


    const [
        loading,
        setLoading,
    ] = useState(true);


    const [
        error,
        setError,
    ] = useState<string | null>(null);


    useEffect(() => {

        let cancelled = false;


        async function loadData() {

            try {

                setLoading(true);

                setError(null);


                const treesResult =
                    await getTrees();


                if (cancelled) {
                    return;
                }


                setTrees(
                    treesResult
                );


                if (!treeId) {

                    setInspections([]);

                    setWorkOrders([]);

                    setLoading(false);

                    return;
                }


                const [
                    inspectionsResult,
                    workOrdersResult,
                ] = await Promise.all([

                    getTreeInspections(
                        treeId
                    ),

                    getTreeWorkOrders(
                        treeId
                    ),

                ]);


                if (cancelled) {
                    return;
                }


                setInspections(
                    inspectionsResult
                );


                setWorkOrders(
                    workOrdersResult
                );


            } catch (err) {

                if (!cancelled) {

                    console.error(
                        "Failed to load map data:",
                        err
                    );


                    setError(
                        "Unable to load map data."
                    );

                }

            } finally {

                if (!cancelled) {

                    setLoading(false);

                }

            }

        }


        loadData();


        return () => {

            cancelled = true;

        };


    }, [treeId]);


    if (loading) {

        return (

            <div
                className="
                    flex
                    h-full
                    items-center
                    justify-center
                    text-muted-foreground
                "
            >
                Loading map...
            </div>

        );

    }


    if (error) {

        return (

            <div
                className="
                    flex
                    h-full
                    items-center
                    justify-center
                    text-destructive
                "
            >
                {error}
            </div>

        );

    }


    return (

        <TreeMapContainer

            trees={trees}

            selectedTreeId={treeId}

            inspections={inspections}

            workOrders={workOrders}

        />

    );

}