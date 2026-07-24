import { DashboardHeader } from "@/components/dashboard/DashboardHeader";
import { DashboardStats } from "@/components/dashboard/DashboardStats";

import { getTrees } from "@/services/trees";
import { getParks } from "@/services/parks";
import { getSpecies } from "@/services/species";

export default async function Dashboard() {
    const [trees, parks, species] =
        await Promise.all([
            getTrees(),
            getParks(),
            getSpecies(),
        ]);

    return (
        <main className="space-y-8">
            <DashboardHeader />

            <DashboardStats
                trees={trees.length}
                parks={parks.length}
                species={species.length}
            />
        </main>
    );
}