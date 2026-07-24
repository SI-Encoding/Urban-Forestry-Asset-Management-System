import { getTrees } from "@/services/trees";

import { AppLayout } from "@/components/layout/AppLayout";
import { TreeMapContainer } from "@/components/maps/TreeMapContainer";

export default async function MapPage() {

    const trees =
        await getTrees();

    return (

        <AppLayout>

            <TreeMapContainer
                trees={trees}
            />

        </AppLayout>

    );
}