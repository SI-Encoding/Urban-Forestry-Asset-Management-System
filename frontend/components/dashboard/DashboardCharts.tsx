"use client";


import type { Tree } from "@/types/tree";
import type { WorkOrder } from "@/types/workOrder";


import { HealthChart } from "./HealthChart";
import { WorkOrderStatusChart } from "./WorkOrderStatusChart";



interface DashboardChartsProps {

    trees:
        Tree[];

    workOrders:
        WorkOrder[];

}



export function DashboardCharts({

    trees,

    workOrders,

}: DashboardChartsProps) {


    return (

        <div
            className="
                grid
                gap-6
                lg:grid-cols-2
            "
        >


            <HealthChart

                trees={
                    trees
                }

            />



            <WorkOrderStatusChart

                workOrders={
                    workOrders
                }

            />


        </div>

    );

}