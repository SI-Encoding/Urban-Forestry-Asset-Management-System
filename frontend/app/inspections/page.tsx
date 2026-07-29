import { AppLayout } from "@/components/layout/AppLayout";

import { getInspections } from "@/services/inspections";

import { InspectionTable } from "@/components/inspections/InspectionTable";


export default async function InspectionsPage() {

    const inspections =
        await getInspections();


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
                        Inspections
                    </h1>

                    <p className="text-muted-foreground">
                        Review tree health assessments,
                        observations, and recommendations.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <InspectionTable
                        data={inspections}
                    />

                </div>


            </div>

        </AppLayout>

    );

}