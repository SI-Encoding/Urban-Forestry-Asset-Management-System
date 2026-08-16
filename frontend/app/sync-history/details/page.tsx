import { Suspense } from "react";

import { AppLayout } from "@/components/layout/AppLayout";
import SyncHistoryDetailsClient from "./SyncHistoryDetailsClient";

export default function SyncHistoryDetailsPage() {
    return (
        <AppLayout>
            <Suspense
                fallback={
                    <div
                        className="
                            flex
                            h-full
                            items-center
                            justify-center
                            text-muted-foreground
                        "
                    >
                        Loading synchronization details...
                    </div>
                }
            >
                <SyncHistoryDetailsClient />
            </Suspense>
        </AppLayout>
    );
}