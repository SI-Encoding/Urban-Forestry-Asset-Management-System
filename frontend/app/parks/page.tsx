import { AppLayout } from "@/components/layout/AppLayout";

import { EntityTable } from "@/components/ui/EntityTable";

import { parkColumns } from "@/components/parks/ParkColumns";

import { getParks } from "@/services/parks";


export default async function ParksPage() {

    const parks =
        await getParks();


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
                        Parks
                    </h1>

                    <p className="text-muted-foreground">
                        Browse all managed parks.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <EntityTable
                        columns={parkColumns}
                        data={parks}
                        emptyMessage="No parks found."
                    />

                </div>


            </div>

        </AppLayout>

    );

}