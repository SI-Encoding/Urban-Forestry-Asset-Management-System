"use client";

import {
    useEffect,
    useState,
} from "react";

import { DashboardHeader } from "@/components/dashboard/DashboardHeader";
import { DashboardStats } from "@/components/dashboard/DashboardStats";
import { DashboardCharts } from "@/components/dashboard/DashboardCharts";
import { DashboardActivity } from "@/components/dashboard/DashboardActivity";

import { getTrees } from "@/services/trees";
import { getParks } from "@/services/parks";
import { getSpecies } from "@/services/species";
import { getInspections } from "@/services/inspections";
import { getWorkOrders } from "@/services/workOrders";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";
import type { Park } from "@/types/park";
import type { Species } from "@/types/species";

export default function Dashboard() {

    const [trees, setTrees] =
        useState<Tree[]>([]);

    const [parks, setParks] =
    useState<Park[]>([]);

    const [species, setSpecies] =
        useState<Species[]>([]);

    const [inspections, setInspections] =
        useState<Inspection[]>([]);

    const [workOrders, setWorkOrders] =
        useState<WorkOrder[]>([]);

    const [loading, setLoading] =
        useState(true);

    const [error, setError] =
        useState<string | null>(null);


    useEffect(() => {

        let cancelled = false;


        async function loadDashboard() {

            try {

                setLoading(true);

                setError(null);


                const [
                    treesResult,
                    parksResult,
                    speciesResult,
                    inspectionsResult,
                    workOrdersResult,
                ] = await Promise.all([

                    getTrees(),

                    getParks(),

                    getSpecies(),

                    getInspections(),

                    getWorkOrders(),

                ]);


                if (cancelled) {
                    return;
                }


                setTrees(
                    treesResult
                );

                setParks(
                    parksResult
                );

                setSpecies(
                    speciesResult
                );

                setInspections(
                    inspectionsResult
                );

                setWorkOrders(
                    workOrdersResult
                );


            } catch (err) {

                if (!cancelled) {

                    console.error(
                        "Failed to load dashboard:",
                        err
                    );

                    setError(
                        "Unable to load dashboard data."
                    );

                }

            } finally {

                if (!cancelled) {

                    setLoading(false);

                }

            }

        }


        loadDashboard();


        return () => {

            cancelled = true;

        };

    }, []);


    if (loading) {

        return (

            <main
                className="
                    flex
                    h-full
                    items-center
                    justify-center
                    text-muted-foreground
                "
            >

                Loading dashboard...

            </main>

        );

    }


    if (error) {

        return (

            <main
                className="
                    flex
                    h-full
                    items-center
                    justify-center
                    text-destructive
                "
            >

                {error}

            </main>

        );

    }


    const treesNeedingAttention =

        trees.filter(
            tree =>
                tree.healthStatus === "Fair"
                ||
                tree.healthStatus === "Poor"
                ||
                tree.healthStatus === "Dead"
        ).length;


    const openWorkOrders =

        workOrders.filter(
            workOrder =>
                workOrder.status === "Open"
                ||
                workOrder.status === "InProgress"
        ).length;


    return (

        <main
            className="
                space-y-8
            "
        >

            <DashboardHeader />


            <DashboardStats

                trees={
                    trees.length
                }

                parks={
                    parks.length
                }

                species={
                    species.length
                }

                inspections={
                    inspections.length
                }

                openWorkOrders={
                    openWorkOrders
                }

                treesNeedingAttention={
                    treesNeedingAttention
                }

            />


            <DashboardCharts

                trees={
                    trees
                }

                workOrders={
                    workOrders
                }

            />


            <DashboardActivity

                inspections={
                    inspections
                }

                workOrders={
                    workOrders
                }

            />

        </main>

    );

}