import { getTrees } from "@/services/trees";

import { AppLayout } from "@/components/layout/AppLayout";

import { TreesPageClient } from "@/components/trees/TreesPageClient";


export default async function TreesPage() {

    const trees =
        await getTrees();


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
                        Trees
                    </h1>

                    <p className="text-muted-foreground">
                        Urban forest tree inventory.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <TreesPageClient
                        trees={trees}
                    />

                </div>


            </div>

        </AppLayout>

    );

}