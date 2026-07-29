"use client";


import {

    BarChart,

    Bar,

    XAxis,

    YAxis,

    CartesianGrid,

    Tooltip,

    ResponsiveContainer,

} from "recharts";


import {

    Card,

    CardContent,

    CardHeader,

    CardTitle,

} from "@/components/ui/card";


import type { WorkOrder } from "@/types/workOrder";



interface WorkOrderStatusChartProps {

    workOrders:
        WorkOrder[];

}



export function WorkOrderStatusChart({

    workOrders,

}: WorkOrderStatusChartProps) {



    const statuses = [

        "Open",

        "InProgress",

        "Completed",

        "Cancelled",

    ];



    const data =

        statuses.map(status => ({


            name:

                status,


            total:

                workOrders.filter(

                    workOrder =>

                        workOrder.status === status

                ).length,


        }));



    return (

        <Card>


            <CardHeader>


                <CardTitle>

                    Work Order Status

                </CardTitle>


            </CardHeader>



            <CardContent>


                <ResponsiveContainer

                    width="100%"

                    height={350}

                >


                    <BarChart

                        data={
                            data
                        }

                    >


                        <CartesianGrid

                            strokeDasharray="3 3"

                        />


                        <XAxis

                            dataKey="name"

                        />


                        <YAxis />


                        <Tooltip />


                        <Bar

                            dataKey="total"

                        />


                    </BarChart>


                </ResponsiveContainer>


            </CardContent>


        </Card>

    );

}