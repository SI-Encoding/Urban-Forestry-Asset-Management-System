import { DashboardHeader } from "@/components/dashboard/DashboardHeader";
import { DashboardStats } from "@/components/dashboard/DashboardStats";
import { DashboardCharts } from "@/components/dashboard/DashboardCharts";

import { getTrees } from "@/services/trees";
import { getParks } from "@/services/parks";
import { getSpecies } from "@/services/species";
import { getInspections } from "@/services/inspections";
import { getWorkOrders } from "@/services/workOrders";


export default async function Dashboard() {


    const [
        trees,
        parks,
        species,
        inspections,
        workOrders,

    ] = await Promise.all([

        getTrees(),

        getParks(),

        getSpecies(),

        getInspections(),

        getWorkOrders(),

    ]);



    const treesNeedingAttention =

        trees.filter(tree =>

            tree.healthStatus === "Fair"

            ||

            tree.healthStatus === "Poor"

            ||

            tree.healthStatus === "Dead"

        ).length;



    const openWorkOrders =

        workOrders.filter(workOrder =>

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


        </main>

    );

}