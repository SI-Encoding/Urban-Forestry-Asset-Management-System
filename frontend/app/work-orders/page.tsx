import { AppLayout } from "@/components/layout/AppLayout";

import { EntityTable } from "@/components/ui/EntityTable";

import { workOrderColumns } from "@/components/work-orders/WorkOrderColumns";

import { getWorkOrders } from "@/services/workOrders";


export default async function WorkOrdersPage() {

    const workOrders =
        await getWorkOrders();


    return (

        <AppLayout>

            <div
                className="
                    flex
                    flex-col
                    h-full
                    min-h-0
                    space-y-6
                "
            >

                <div className="shrink-0">

                    <h1 className="text-3xl font-bold">
                        Work Orders
                    </h1>

                    <p className="text-muted-foreground">
                        Manage tree maintenance tasks.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <EntityTable
                        columns={workOrderColumns}
                        data={workOrders}
                        emptyMessage="No work orders found."
                    />

                </div>


            </div>

        </AppLayout>

    );

}