import { getTrees } from "@/services/trees";
import { getTreeInspections } from "@/services/inspections";
import { getTreeWorkOrders } from "@/services/workOrders";

import { AppLayout } from "@/components/layout/AppLayout";
import { TreeMapContainer } from "@/components/maps/TreeMapContainer";

interface MapPageProps {
    searchParams: Promise<{
        treeId?: string;
    }>;
}

export default async function MapPage({
    searchParams,
}: MapPageProps) {

    const { treeId } =
        await searchParams;

    const trees =
        await getTrees();

    const [
        inspections,
        workOrders,
    ] =
        treeId
            ? await Promise.all([
                getTreeInspections(treeId),
                getTreeWorkOrders(treeId),
            ])
            : [[], []];

    return (

        <AppLayout>

            <TreeMapContainer
                trees={trees}
                selectedTreeId={treeId}
                inspections={inspections}
                workOrders={workOrders}
            />

        </AppLayout>

    );

}