"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";

import {
    TreeDetailsTabs,
    type TreeDetailsTab,
} from "./TreeDetailsTabs";
import { HealthBadge } from "./HealthBadge";

interface TreeDetailsPanelProps {
    tree?: Tree;
    inspections: Inspection[];
}

export function TreeDetailsPanel({
    tree,
    inspections,
}: TreeDetailsPanelProps) {

    const [activeTab, setActiveTab] =
        useState<TreeDetailsTab>("overview");

    if (!tree) {
        return (
            <div className="rounded-lg border bg-white p-6 shadow-sm">
                <p className="text-gray-500">
                    Select a tree
                </p>
            </div>
        );
    }

    return (

        <div className="h-full overflow-y-auto rounded-lg border bg-white p-6 shadow-sm">

            <h2 className="mb-1 text-lg font-semibold">
                {tree.assetTag}
            </h2>

            <p className="mb-6 text-sm text-gray-500">
                {tree.speciesName}
            </p>

            <TreeDetailsTabs
                active={activeTab}
                onChange={setActiveTab}
            />

            {activeTab === "overview" && (

                <div className="space-y-4">

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Asset Tag
                        </p>

                        <p className="font-medium">
                            {tree.assetTag}
                        </p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Species
                        </p>

                        <p>
                            {tree.speciesName}
                        </p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Park
                        </p>

                        <p>
                            {tree.parkName}
                        </p>
                    </div>

                    <div>
                        <p className="text-xs uppercase text-gray-500">
                            Current Health
                        </p>

                        <HealthBadge
                            status={tree.healthStatus}
                        />
                    </div>

                </div>

            )}

            {activeTab === "inspections" && (

                <>

                    <h3 className="mb-4 mt-2 font-semibold">
                        Inspection History
                    </h3>

                    {inspections.length === 0 ? (

                        <p className="text-sm text-gray-500">
                            No inspections found.
                        </p>

                    ) : (

                        <div className="space-y-4">

                            {inspections.map((inspection) => (

                                <div
                                    key={inspection.id}
                                    className="rounded-md border p-4"
                                >

                                    <div className="flex justify-between">

                                        <span className="font-medium">
                                            {inspection.inspectionDate}
                                        </span>

                                        <span className="text-sm text-gray-500">
                                            {inspection.observedHealth}
                                        </span>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Notes
                                        </p>

                                        <p>
                                            {inspection.notes}
                                        </p>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Recommendation
                                        </p>

                                        <p>
                                            {inspection.recommendation}
                                        </p>

                                    </div>

                                    <div className="mt-3">

                                        <p className="text-xs uppercase text-gray-500">
                                            Next Inspection
                                        </p>

                                        <p>
                                            {inspection.nextInspectionDate ??
                                                "Not scheduled"}
                                        </p>

                                    </div>

                                </div>

                            ))}

                        </div>

                    )}

                </>

            )}

        </div>

    );

}