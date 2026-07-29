"use client";

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import type { WorkOrder } from "@/types/workOrder";


interface UpcomingWorkOrdersProps {
    workOrders: WorkOrder[];
}


export function UpcomingWorkOrders({
    workOrders,
}: UpcomingWorkOrdersProps) {


    const upcoming = [...workOrders]
        .filter(
            workOrder =>
                workOrder.status !== "Completed" &&
                workOrder.status !== "Cancelled" &&
                workOrder.dueDate !== null
        )
        .sort(
            (a, b) =>
                new Date(a.dueDate!).getTime() -
                new Date(b.dueDate!).getTime()
        )
        .slice(
            0,
            5
        );


    return (

        <Card>

            <CardHeader>

                <CardTitle>
                    Upcoming Work Orders
                </CardTitle>

            </CardHeader>


            <CardContent>


                {
                    upcoming.length === 0

                    ? (

                        <p
                            className="
                                text-sm
                                text-muted-foreground
                            "
                        >
                            No upcoming work orders.
                        </p>

                    )

                    :

                    (

                        <div
                            className="
                                space-y-4
                            "
                        >

                            {
                                upcoming.map(
                                    workOrder => (

                                        <div
                                            key={
                                                workOrder.id
                                            }

                                            className="
                                                rounded-md
                                                border
                                                p-4
                                            "
                                        >

                                            <div
                                                className="
                                                    flex
                                                    justify-between
                                                    gap-4
                                                "
                                            >

                                                <p
                                                    className="
                                                        font-medium
                                                    "
                                                >
                                                    {
                                                        workOrder.description
                                                    }
                                                </p>


                                                <span
                                                    className="
                                                        rounded-full
                                                        bg-blue-100
                                                        px-3
                                                        py-1
                                                        text-xs
                                                        font-medium
                                                        text-blue-800
                                                    "
                                                >
                                                    {
                                                        workOrder.status
                                                    }
                                                </span>

                                            </div>



                                            <p
                                                className="
                                                    mt-2
                                                    text-sm
                                                    text-muted-foreground
                                                "
                                            >

                                                Due:

                                                {" "}

                                                {
                                                    new Date(
                                                        workOrder.dueDate!
                                                    ).toLocaleDateString(
                                                        "en-US",
                                                        {
                                                            year: "numeric",
                                                            month: "short",
                                                            day: "numeric",
                                                        }
                                                    )
                                                }

                                            </p>


                                        </div>

                                    )
                                )
                            }


                        </div>

                    )
                }


            </CardContent>


        </Card>

    );
}