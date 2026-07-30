"use client";

import Link from "next/link";

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import {
    HealthBadge
} from "@/components/ui/HealthBadge";

import type { Inspection } from "@/types/inspection";


interface RecentInspectionsProps {

    inspections: Inspection[];

}


export function RecentInspections({

    inspections,

}: RecentInspectionsProps) {


    const recentInspections =

        [...inspections]

            .sort(

                (a, b) =>

                    new Date(
                        b.inspectionDate
                    ).getTime()

                    -

                    new Date(
                        a.inspectionDate
                    ).getTime()

            )

            .slice(
                0,
                5
            );


    return (

        <Card>

            <CardHeader>

                <CardTitle>
                    Recent Inspections
                </CardTitle>

            </CardHeader>


            <CardContent>

                {
                    recentInspections.length === 0

                    ?

                    (

                        <p
                            className="
                                text-sm
                                text-muted-foreground
                            "
                        >
                            No inspections found.
                        </p>

                    )

                    :

                    (

                        <div
                            className="
                                space-y-4
                            "
                        >

                            {
                                recentInspections.map(

                                    inspection => (

                                        <Link

                                            key={
                                                inspection.id
                                            }

                                            href={
                                                `/map?treeId=${inspection.treeId}`
                                            }

                                            className="
                                                block
                                                rounded-md
                                                border
                                                p-4
                                                transition
                                                hover:bg-muted
                                            "

                                        >

                                            <div

                                                className="
                                                    flex
                                                    justify-between
                                                    items-center
                                                "

                                            >

                                                <p
                                                    className="
                                                        font-medium
                                                    "
                                                >
                                                    🌲 {inspection.assetTag}
                                                </p>


                                                <HealthBadge

                                                    status={
                                                        inspection.observedHealth
                                                    }

                                                />

                                            </div>


                                            <p

                                                className="
                                                    mt-2
                                                    text-sm
                                                    text-muted-foreground
                                                "

                                            >

                                                {
                                                    inspection.inspectionDate
                                                }

                                            </p>


                                        </Link>

                                    )

                                )
                            }


                        </div>

                    )
                }


            </CardContent>


        </Card>

    );

}