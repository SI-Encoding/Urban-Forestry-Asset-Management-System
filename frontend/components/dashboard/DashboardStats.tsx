import { StatCard } from "./StatCard";


interface DashboardStatsProps {

    trees:
        number;

    parks:
        number;

    species:
        number;

    inspections:
        number;

    openWorkOrders:
        number;

    treesNeedingAttention:
        number;

}



export function DashboardStats({

    trees,

    parks,

    species,

    inspections,

    openWorkOrders,

    treesNeedingAttention,

}: DashboardStatsProps) {


    return (

        <div
            className="
                grid
                gap-6
                md:grid-cols-2
                xl:grid-cols-3
            "
        >


            <StatCard

                title="🌳 Trees"

                value={
                    trees
                }

            />



            <StatCard

                title="🌲 Parks"

                value={
                    parks
                }

            />



            <StatCard

                title="🍁 Species"

                value={
                    species
                }

            />



            <StatCard

                title="🔎 Inspections"

                value={
                    inspections
                }

            />



            <StatCard

                title="📋 Open Work Orders"

                value={
                    openWorkOrders
                }

            />



            <StatCard

                title="⚠ Trees Needing Attention"

                value={
                    treesNeedingAttention
                }

            />


        </div>

    );

}