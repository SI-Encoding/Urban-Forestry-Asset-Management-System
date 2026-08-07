import { AppLayout } from "@/components/layout/AppLayout";

import {
    ArcGisSyncPageClient,
} from "@/components/arcgis-sync/ArcGisSyncPageClient";

import {
    ArcGisAuthGuard,
} from "@/components/auth/ArcGisAuthGuard";


export default function ArcGisSyncPage() {

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

                <ArcGisAuthGuard>

                    <ArcGisSyncPageClient />

                </ArcGisAuthGuard>

            </div>

        </AppLayout>

    );

}