"use client";

import {
Fragment,
useState,
} from "react";

import type {
SpatialSyncAction,
} from "@/types/arcgisSync";

import {
ArcGisSyncActionBadge,
} from "./ArcGisSyncActionBadge";

import {
applySingleArcGisSync,
} from "@/services/arcgis-sync-service";

interface Props {
actions: SpatialSyncAction[];
}

export function ArcGisSyncActionsTable({
actions,
}: Props) {

const [visibleActions, setVisibleActions] =
    useState<SpatialSyncAction[]>(actions);

const [applying, setApplying] =
    useState<string | null>(null);

const [expanded, setExpanded] =
    useState<string | null>(null);


function toggle(assetTag: string) {

    setExpanded(
        current =>
            current === assetTag
                ? null
                : assetTag
    );

}


function removeAction(
    assetTag: string
) {

    setVisibleActions(
        current =>
            current.filter(
                action =>
                    action.assetTag !== assetTag
            )
    );

    setExpanded(
        current =>
            current === assetTag
                ? null
                : current
    );

}


async function handleApply(
    assetTag: string
) {

    setApplying(assetTag);

    try {

        await applySingleArcGisSync(
            assetTag
        );

        removeAction(
            assetTag
        );

    }
    catch (error) {

        console.error(
            "Failed to apply ArcGIS changes:",
            error
        );

    }
    finally {

        setApplying(null);

    }

}


function handleIgnore(
    assetTag: string
) {

    removeAction(
        assetTag
    );

}


return (

    <div className="space-y-4">

        {
            visibleActions.length === 0 ? (

                <div
                    className="
                        rounded-lg
                        border
                        bg-white
                        p-8
                        text-center
                    "
                >

                    <p className="text-lg font-semibold">
                        No synchronization actions remaining.
                    </p>

                    <p className="mt-2 text-sm text-muted-foreground">
                        All displayed synchronization items
                        have been reviewed.
                    </p>

                </div>

            ) : (

                <div className="rounded-lg border bg-white">

                    <table className="w-full">

                        <thead>

                            <tr className="border-b">

                                <th className="p-4 text-left">
                                    Action
                                </th>

                                <th className="p-4 text-left">
                                    Asset Tag
                                </th>

                                <th className="p-4 text-left">
                                    Reason
                                </th>

                                <th className="p-4 text-right">
                                </th>

                            </tr>

                        </thead>


                        <tbody>

                            {
                                visibleActions.map(
                                    action => (

                                        <Fragment
                                            key={
                                                action.assetTag
                                            }
                                        >

                                            <tr
                                                className="
                                                    border-b
                                                "
                                            >

                                                <td className="p-4">

                                                    <ArcGisSyncActionBadge
                                                        action={
                                                            action.action
                                                        }
                                                    />

                                                </td>


                                                <td className="p-4">

                                                    {
                                                        action.assetTag
                                                    }

                                                </td>


                                                <td className="p-4">

                                                    {
                                                        action.reason
                                                    }

                                                </td>


                                                <td
                                                    className="
                                                        p-4
                                                        text-right
                                                    "
                                                >

                                                    {
                                                        action.action ===
                                                            "Update" && (

                                                            <button
                                                                type="button"
                                                                className="
                                                                    text-sm
                                                                    underline
                                                                "
                                                                onClick={() =>
                                                                    toggle(
                                                                        action.assetTag
                                                                    )
                                                                }
                                                            >

                                                                {
                                                                    expanded ===
                                                                        action.assetTag
                                                                        ? "Hide Details"
                                                                        : "View Details"
                                                                }

                                                            </button>

                                                        )
                                                    }

                                                </td>

                                            </tr>


                                            {
                                                expanded ===
                                                    action.assetTag && (

                                                    <tr>

                                                        <td
                                                            colSpan={4}
                                                            className="
                                                                bg-muted/30
                                                                p-6
                                                            "
                                                        >

                                                            <SyncComparison
                                                                action={
                                                                    action
                                                                }
                                                                applying={
                                                                    applying ===
                                                                    action.assetTag
                                                                }
                                                                onApply={
                                                                    handleApply
                                                                }
                                                                onIgnore={
                                                                    handleIgnore
                                                                }
                                                            />

                                                        </td>

                                                    </tr>

                                                )
                                            }

                                        </Fragment>

                                    )
                                )
                            }

                        </tbody>

                    </table>

                </div>

            )
        }

    </div>

);

}

function SyncComparison({
action,
applying,
onApply,
onIgnore,
}: {
action: SpatialSyncAction;
applying: boolean;
onApply: (
assetTag: string
) => void;
onIgnore: (
assetTag: string
) => void;
}) {

return (

    <div className="space-y-6">

        <h3 className="text-lg font-semibold">

            {action.assetTag} Comparison

        </h3>


        <div className="space-y-4">

            <ComparisonRow
                title="Species"
                ufams={
                    action.ufamsSpecies
                }
                arcgis={
                    action.arcGisSpecies
                }
            />


            <ComparisonRow
                title="Park"
                ufams={
                    action.ufamsPark
                }
                arcgis={
                    action.arcGisPark
                }
            />


            <ComparisonRow
                title="Health Status"
                ufams={
                    action.ufamsHealthStatus
                }
                arcgis={
                    action.arcGisHealthStatus
                }
            />


            <ComparisonRow
                title="Latitude"
                ufams={
                    action.ufamsLatitude?.toString()
                }
                arcgis={
                    action.arcGisLatitude?.toString()
                }
            />


            <ComparisonRow
                title="Longitude"
                ufams={
                    action.ufamsLongitude?.toString()
                }
                arcgis={
                    action.arcGisLongitude?.toString()
                }
            />

        </div>


        <div className="flex gap-3">

            <button
                type="button"
                className="
                    rounded-md
                    bg-black
                    px-4
                    py-2
                    text-white
                    disabled:cursor-not-allowed
                    disabled:opacity-50
                "
                disabled={applying}
                onClick={() =>
                    onApply(
                        action.assetTag
                    )
                }
            >

                {
                    applying
                        ? "Applying..."
                        : "Apply ArcGIS Changes"
                }

            </button>


            <button
                type="button"
                className="
                    rounded-md
                    border
                    px-4
                    py-2
                    disabled:cursor-not-allowed
                    disabled:opacity-50
                "
                disabled={applying}
                onClick={() =>
                    onIgnore(
                        action.assetTag
                    )
                }
            >

                Ignore

            </button>

        </div>

    </div>

);

}

function ComparisonRow({
title,
ufams,
arcgis,
}: {
title: string;
ufams?: string;
arcgis?: string;
}) {

return (

    <div className="grid grid-cols-3 gap-4">

        <div className="font-medium">

            {title}

        </div>


        <div>

            <p className="text-sm text-muted-foreground">
                UFAMS
            </p>

            <p>
                {ufams ?? "-"}
            </p>

        </div>


        <div>

            <p className="text-sm text-muted-foreground">
                ArcGIS
            </p>

            <p>
                {arcgis ?? "-"}
            </p>

        </div>

    </div>

);

}
