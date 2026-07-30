import { notFound } from "next/navigation";


import { AppLayout } from "@/components/layout/AppLayout";


import { getWorkOrder } from "@/services/workOrders";


import { WorkOrderStatusBadge } from "@/components/ui/WorkOrderStatusBadge";


import { formatDate } from "@/components/utils/date-format";



interface WorkOrderDetailsPageProps {

    params: {
        id: string;
    };

}



export default async function WorkOrderDetailsPage({

    params,

}: WorkOrderDetailsPageProps) {


    const workOrder = await getWorkOrder(

        params.id

    )
    .catch(

        () => null

    );



    if (!workOrder) {

        notFound();

    }



    return (

        <AppLayout>


            <div
                className="
                    space-y-6
                "
            >


                <div>

                    <h1
                        className="
                            text-3xl
                            font-bold
                        "
                    >

                        Work Order

                    </h1>


                    <p
                        className="
                            text-muted-foreground
                        "
                    >

                        {workOrder.description}

                    </p>


                </div>



                <div
                    className="
                        rounded-lg
                        border
                        p-6
                        space-y-6
                    "
                >


                    <div>

                        <p className="text-sm text-muted-foreground">

                            Status

                        </p>


                        <WorkOrderStatusBadge

                            status={
                                workOrder.status
                            }

                        />


                    </div>



                    <div>

                        <p className="text-sm text-muted-foreground">

                            Asset

                        </p>


                        <p className="font-medium">

                            {workOrder.assetTag}

                        </p>


                    </div>



                    <div>

                        <p className="text-sm text-muted-foreground">

                            Assigned Employee

                        </p>


                        <p>

                            {
                                workOrder.assignedEmployeeName
                                ??
                                "Unassigned"
                            }

                        </p>


                    </div>



                    <div>

                        <p className="text-sm text-muted-foreground">

                            Created

                        </p>


                        <p>

                            {
                                formatDate(
                                    workOrder.createdDate
                                )
                            }

                        </p>


                    </div>



                    <div>

                        <p className="text-sm text-muted-foreground">

                            Due Date

                        </p>


                        <p>

                            {
                                formatDate(
                                    workOrder.dueDate
                                )
                            }

                        </p>


                    </div>


                </div>


            </div>


        </AppLayout>

    );

}