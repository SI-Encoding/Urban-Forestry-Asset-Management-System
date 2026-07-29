"use client";


import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";


import { RecentInspections } from "./RecentInspections";
import { UpcomingWorkOrders } from "./UpcomingWorkOrders";


interface DashboardActivityProps {

    inspections:
        Inspection[];

    workOrders:
        WorkOrder[];

}



export function DashboardActivity({

    inspections,

    workOrders,

}: DashboardActivityProps) {


    return (

        <div
            className="
                grid
                gap-6
                lg:grid-cols-2
            "
        >

            <RecentInspections

                inspections={
                    inspections
                }

            />


            <UpcomingWorkOrders

                workOrders={
                    workOrders
                }

            />

        </div>

    );

}