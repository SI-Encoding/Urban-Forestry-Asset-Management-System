"use client";

import { useState } from "react";

import {
    getArcGisSyncPreview,
    applyArcGisSync,
} from "@/services/arcgis-sync-service";

import type {
    SpatialSyncResult,
} from "@/types/arcgisSync";


import {
    ArcGisSyncSummary,
} from "./ArcGisSyncSummary";


import {
    ArcGisSyncActionsTable,
} from "./ArcGisSyncActionsTable";


import {
    ArcGisSyncPreviewButton,
} from "./ArcGisSyncPreviewButton";


import {
    ArcGisSyncApplyButton,
} from "./ArcGisSyncApplyButton";



export function ArcGisSyncPageClient() {

    const [result, setResult] =
        useState<SpatialSyncResult | null>(null);


    const [loading, setLoading] =
        useState(false);


    const [applying, setApplying] =
        useState(false);



    async function previewSync() {

        setLoading(true);

        try {

            const response =
                await getArcGisSyncPreview();


            setResult(response);

        }
        finally {

            setLoading(false);

        }

    }





    async function applySync() {
console.log("Apply clicked");

        setApplying(true);

        try {

            await applyArcGisSync();


            const refreshed =
                await getArcGisSyncPreview();


            setResult(refreshed);

        }
        finally {

            setApplying(false);

        }

    }





    return (

        <div className="space-y-6">


            <div>

                <h1 className="text-3xl font-bold">
                    ArcGIS Synchronization
                </h1>


                <p className="text-muted-foreground">
                    Preview synchronization between
                    ArcGIS Online and UFAMS.
                </p>

            </div>




            <div className="flex gap-3">


                <ArcGisSyncPreviewButton

                    loading={loading}

                    onClick={previewSync}

                />



                <ArcGisSyncApplyButton

                    disabled={
                        !result ||
                        loading
                    }

                    loading={applying}

                    onClick={applySync}

                />


            </div>





            {
                result && (

                    <>

                        <ArcGisSyncSummary
                            result={result}
                        />



                        <ArcGisSyncActionsTable
                            actions={result.actions}
                        />

                    </>

                )
            }




        </div>

    );

}