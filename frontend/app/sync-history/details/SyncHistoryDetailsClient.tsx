"use client";

import Link from "next/link";

import {
    useEffect,
    useState,
} from "react";

import {
    useSearchParams,
} from "next/navigation";

import {
    getSyncAudits,
} from "@/services/sync-audit";

import type {
    SyncAudit,
} from "@/services/sync-audit";

import {
    Card,
    CardContent,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";


export default function SyncHistoryDetailsClient() {

    const searchParams =
        useSearchParams();


    const id =
        searchParams.get("id");


    const [
        audit,
        setAudit,
    ] =
        useState<SyncAudit | null>(null);


    const [
        loading,
        setLoading,
    ] =
        useState(true);


    const [
        error,
        setError,
    ] =
        useState<string | null>(null);


    useEffect(() => {

        let cancelled = false;


        async function loadAudit() {

            if (!id) {

                if (!cancelled) {

                    setError(
                        "No synchronization audit ID was provided."
                    );

                    setLoading(false);

                }

                return;
            }


            const auditId =
                Number(id);


            if (
                !Number.isInteger(auditId)
                ||
                auditId <= 0
            ) {

                if (!cancelled) {

                    setError(
                        "Invalid synchronization audit ID."
                    );

                    setLoading(false);

                }

                return;
            }


            try {

                setLoading(true);

                setError(null);


                const audits =
                    await getSyncAudits();


                if (cancelled) {
                    return;
                }


                const foundAudit =
                    audits.find(
                        item =>
                            item.id === auditId
                    );


                if (!foundAudit) {

                    setError(
                        "Synchronization audit not found."
                    );

                    setAudit(null);

                    return;
                }


                setAudit(
                    foundAudit
                );


            } catch (err) {

                if (cancelled) {
                    return;
                }


                console.error(
                    "Failed to load synchronization audit:",
                    err
                );


                setError(
                    "Unable to load synchronization audit."
                );


                setAudit(null);


            } finally {

                if (!cancelled) {

                    setLoading(false);

                }

            }

        }


        loadAudit();


        return () => {

            cancelled = true;

        };

    }, [id]);


    return (

        <div
            className="
                flex
                flex-col
                h-full
                min-h-0
                space-y-6
            "
        >

            {/* Header */}

            <div className="shrink-0">

                <Link
                    href="/sync-history"
                    className="
                        inline-flex
                        items-center
                        rounded-md
                        border
                        px-3
                        py-2
                        text-sm
                        font-medium
                        hover:bg-muted
                        transition
                    "
                >
                    ← Back to Sync History
                </Link>


                <div className="mt-4">

                    <h1
                        className="
                            text-3xl
                            font-bold
                        "
                    >
                        Synchronization Details
                    </h1>


                    <p
                        className="
                            text-muted-foreground
                        "
                    >
                        Details of an ArcGIS
                        synchronization operation.
                    </p>

                </div>

            </div>


            {/* Loading */}

            {loading && (

                <Card>

                    <CardContent
                        className="
                            py-8
                            text-center
                            text-muted-foreground
                        "
                    >
                        Loading synchronization details...
                    </CardContent>

                </Card>

            )}


            {/* Error */}

            {!loading && error && (

                <Card>

                    <CardContent
                        className="
                            py-8
                            text-center
                            text-destructive
                        "
                    >
                        {error}
                    </CardContent>

                </Card>

            )}


            {/* Audit Details */}

            {!loading && audit && (

                <>

                    {/* Summary Cards */}

                    <div
                        className="
                            grid
                            grid-cols-2
                            md:grid-cols-5
                            gap-4
                            shrink-0
                        "
                    >

                        {/* Status */}

                        <Card>

                            <CardHeader>

                                <CardTitle
                                    className="text-sm"
                                >
                                    Status
                                </CardTitle>

                            </CardHeader>


                            <CardContent>

                                <div
                                    className="
                                        text-lg
                                        font-semibold
                                    "
                                >
                                    {audit.status}
                                </div>

                            </CardContent>

                        </Card>


                        {/* Created */}

                        <Card>

                            <CardHeader>

                                <CardTitle
                                    className="text-sm"
                                >
                                    Created
                                </CardTitle>

                            </CardHeader>


                            <CardContent>

                                <div
                                    className="
                                        text-lg
                                        font-semibold
                                    "
                                >
                                    {audit.createdCount}
                                </div>

                            </CardContent>

                        </Card>


                        {/* Updated */}

                        <Card>

                            <CardHeader>

                                <CardTitle
                                    className="text-sm"
                                >
                                    Updated
                                </CardTitle>

                            </CardHeader>


                            <CardContent>

                                <div
                                    className="
                                        text-lg
                                        font-semibold
                                    "
                                >
                                    {audit.updatedCount}
                                </div>

                            </CardContent>

                        </Card>


                        {/* Failed */}

                        <Card>

                            <CardHeader>

                                <CardTitle
                                    className="text-sm"
                                >
                                    Failed
                                </CardTitle>

                            </CardHeader>


                            <CardContent>

                                <div
                                    className="
                                        text-lg
                                        font-semibold
                                    "
                                >
                                    {audit.failedCount}
                                </div>

                            </CardContent>

                        </Card>


                        {/* Ignored */}

                        <Card>

                            <CardHeader>

                                <CardTitle
                                    className="text-sm"
                                >
                                    Ignored
                                </CardTitle>

                            </CardHeader>


                            <CardContent>

                                <div
                                    className="
                                        text-lg
                                        font-semibold
                                    "
                                >
                                    {audit.ignoredCount}
                                </div>

                            </CardContent>

                        </Card>

                    </div>


                    {/* Operations */}

                    <Card
                        className="
                            flex-1
                            min-h-0
                        "
                    >

                        <CardHeader>

                            <CardTitle>
                                Synchronization Operations
                            </CardTitle>

                        </CardHeader>


                        <CardContent
                            className="
                                h-[calc(100%-5rem)]
                            "
                        >

                            {audit.entries.length === 0 ? (

                                <div
                                    className="
                                        py-8
                                        text-center
                                        text-muted-foreground
                                    "
                                >
                                    No synchronization
                                    operations were recorded.
                                </div>

                            ) : (

                                <div
                                    className="
                                        h-full
                                        overflow-auto
                                        rounded-md
                                        border
                                    "
                                >

                                    <Table>

                                        <TableHeader>

                                            <TableRow>

                                                <TableHead>
                                                    Asset Tag
                                                </TableHead>

                                                <TableHead>
                                                    Action
                                                </TableHead>

                                                <TableHead>
                                                    Reason
                                                </TableHead>

                                                <TableHead>
                                                    Date
                                                </TableHead>

                                            </TableRow>

                                        </TableHeader>


                                        <TableBody>

                                            {audit.entries.map(
                                                entry => (

                                                    <TableRow
                                                        key={
                                                            entry.id
                                                        }
                                                    >

                                                        <TableCell>
                                                            {
                                                                entry.assetTag
                                                            }
                                                        </TableCell>


                                                        <TableCell>
                                                            {
                                                                entry.action
                                                            }
                                                        </TableCell>


                                                        <TableCell>
                                                            {
                                                                entry.reason
                                                            }
                                                        </TableCell>


                                                        <TableCell>

                                                            {new Date(
                                                                entry.createdAt
                                                            ).toLocaleString(
                                                                "en-US",
                                                                {
                                                                    year: "numeric",
                                                                    month: "numeric",
                                                                    day: "numeric",
                                                                    hour: "numeric",
                                                                    minute: "2-digit",
                                                                    second: "2-digit",
                                                                    hour12: true,
                                                                }
                                                            )}

                                                        </TableCell>

                                                    </TableRow>

                                                )
                                            )}

                                        </TableBody>

                                    </Table>

                                </div>

                            )}

                        </CardContent>

                    </Card>

                </>

            )}

        </div>

    );

}