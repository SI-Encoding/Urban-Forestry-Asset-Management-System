"use client";

import { Button } from "@/components/ui/button";

import { getGeoJson } from "@/services/geojson";

export function ExportGeoJsonButton() {

    async function handleExport() {

        const geojson =
            await getGeoJson();

        const blob =
            new Blob(
                [
                    JSON.stringify(
                        geojson,
                        null,
                        2
                    )
                ],
                {
                    type: "application/geo+json",
                }
            );

        const url =
            URL.createObjectURL(blob);

        const link =
            document.createElement("a");

        link.href = url;

        link.download =
            "trees.geojson";

        link.click();

        URL.revokeObjectURL(url);

    }

    return (

        <Button
            variant="outline"
            onClick={handleExport}
        >
            Export GeoJSON
        </Button>

    );

}