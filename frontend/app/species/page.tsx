import { AppLayout } from "@/components/layout/AppLayout";

import { EntityTable } from "@/components/ui/EntityTable";

import { speciesColumns } from "@/components/species/SpeciesColumns";

import { getSpecies } from "@/services/species";


export default async function SpeciesPage() {

    const species =
        await getSpecies();


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
                        Species
                    </h1>

                    <p className="text-muted-foreground">
                        Browse all tree species.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <EntityTable
                        columns={speciesColumns}
                        data={species}
                        emptyMessage="No species found."
                    />

                </div>


            </div>

        </AppLayout>

    );

}