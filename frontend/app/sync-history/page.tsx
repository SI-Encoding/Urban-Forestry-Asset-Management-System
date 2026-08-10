import { getSyncAudits } from "@/services/sync-audit";

import { AppLayout } from "@/components/layout/AppLayout";

import { SyncHistoryPageClient } from "@/components/sync-history/SyncHistoryPageClient";


export default async function SyncHistoryPage() {

    const audits =
        await getSyncAudits();


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
                        Sync History
                    </h1>

                    <p className="text-muted-foreground">
                        History of ArcGIS synchronization operations.
                    </p>

                </div>


                <div
                    className="
                        flex-1
                        min-h-0
                    "
                >

                    <SyncHistoryPageClient
                        audits={audits}
                    />

                </div>

            </div>

        </AppLayout>

    );

}
