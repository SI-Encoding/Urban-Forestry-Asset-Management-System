"use client";

import { useState } from "react";

import type { Tree } from "@/types/tree";
import type { Inspection } from "@/types/inspection";
import type { WorkOrder } from "@/types/workOrder";
import ReactMarkdown from "react-markdown";
import {
    startWorkOrder,
    completeWorkOrder,
    cancelWorkOrder,
} from "@/services/workOrders";

import {
    generateTreeSummary,
    type TreeAiSummary,
} from "@/services/ai";

import {
    TreeDetailsTabs,
    type TreeDetailsTab,
} from "./TreeDetailsTabs";

import { HealthBadge } from "../ui/HealthBadge";
import { Button } from "../ui/button";

import { EditInspectionForm } from "./EditInspectionForm";
import { CreateInspectionForm } from "./CreateInspectionForm";
import { CreateWorkOrderForm } from "./CreateWorkOrderForm";

interface TreeDetailsPanelProps {
    tree?: Tree;
    inspections: Inspection[];
    workOrders: WorkOrder[];
}

export function TreeDetailsPanel({
    tree,
    inspections,
    workOrders,
}: TreeDetailsPanelProps) {
    const [
        activeTab,
        setActiveTab,
    ] = useState<TreeDetailsTab>("overview");

    const [
        editingInspectionId,
        setEditingInspectionId,
    ] = useState<string | null>(null);

    const [
        aiAssessment,
        setAiAssessment,
    ] = useState<TreeAiSummary | null>(null);

    const [
        aiLoading,
        setAiLoading,
    ] = useState(false);

    const [
        aiError,
        setAiError,
    ] = useState<string | null>(null);

    if (!tree) {
        return (
            <div
                className="
                    w-[420px]
                    shrink-0
                    rounded-lg
                    border
                    bg-white
                    p-6
                    shadow-sm
                "
            >
                <p className="text-gray-500">
                    Select a tree
                </p>
            </div>
        );
    }

    // Explicitly narrow tree so TypeScript knows it exists
    // throughout the remainder of the component.
    const selectedTree = tree;

    async function handleStartWorkOrder(
        workOrderId: string
    ) {
        await startWorkOrder(workOrderId);

        window.location.reload();
    }

    async function handleCompleteWorkOrder(
        workOrderId: string
    ) {
        await completeWorkOrder(workOrderId);

        window.location.reload();
    }

    async function handleCancelWorkOrder(
        workOrderId: string
    ) {
        await cancelWorkOrder(workOrderId);

        window.location.reload();
    }

    async function handleGenerateAiAssessment() {
        setAiLoading(true);
        setAiError(null);

        try {
            const result = await generateTreeSummary(
                selectedTree.id
            );

            setAiAssessment(result);
        } catch (error) {
            setAiError(
                error instanceof Error
                    ? error.message
                    : "Unable to generate AI assessment."
            );
        } finally {
            setAiLoading(false);
        }
    }

    const sortedInspections =
        [...inspections].sort(
            (a, b) =>
                new Date(
                    b.inspectionDate
                ).getTime() -
                new Date(
                    a.inspectionDate
                ).getTime()
        );

    return (
        <div
            className="
                w-[420px]
                shrink-0
                h-full
                overflow-y-auto
                rounded-lg
                border
                bg-white
                p-6
                shadow-sm
            "
        >
            <h2
                className="
                    mb-1
                    text-lg
                    font-semibold
                "
            >
                {selectedTree.assetTag}
            </h2>

            <p
                className="
                    mb-6
                    text-sm
                    text-gray-500
                "
            >
                {selectedTree.speciesName}
            </p>

            <TreeDetailsTabs
                active={activeTab}
                onChange={setActiveTab}
            />

            {/* ========================= */}
            {/* OVERVIEW */}
            {/* ========================= */}

            {activeTab === "overview" && (
                <div className="space-y-4">
                    <div>
                        <p
                            className="
                                text-xs
                                uppercase
                                text-gray-500
                            "
                        >
                            Asset Tag
                        </p>

                        <p className="font-medium">
                            {selectedTree.assetTag}
                        </p>
                    </div>

                    <div>
                        <p
                            className="
                                text-xs
                                uppercase
                                text-gray-500
                            "
                        >
                            Species
                        </p>

                        <p>
                            {selectedTree.speciesName}
                        </p>
                    </div>

                    <div>
                        <p
                            className="
                                text-xs
                                uppercase
                                text-gray-500
                            "
                        >
                            Park
                        </p>

                        <p>
                            {selectedTree.parkName}
                        </p>
                    </div>

                    <div>
                        <p
                            className="
                                text-xs
                                uppercase
                                text-gray-500
                            "
                        >
                            Current Health
                        </p>

                        <HealthBadge
                            status={
                                selectedTree.healthStatus
                            }
                        />
                    </div>
                </div>
            )}

            {/* ========================= */}
            {/* INSPECTIONS */}
            {/* ========================= */}

            {activeTab === "inspections" && (
                <>
                    <CreateInspectionForm
                        treeId={selectedTree.id}
                        onCreated={() =>
                            window.location.reload()
                        }
                    />

                    <hr className="my-6" />

                    <h3
                        className="
                            mb-4
                            font-semibold
                        "
                    >
                        Inspection History
                    </h3>

                    {sortedInspections.length === 0 ? (
                        <p
                            className="
                                text-sm
                                text-gray-500
                            "
                        >
                            No inspections found.
                        </p>
                    ) : (
                        <div className="space-y-4">
                            {sortedInspections.map(
                                (inspection) => (
                                    <div
                                        key={
                                            inspection.id
                                        }
                                        className="
                                            rounded-md
                                            border
                                            p-4
                                        "
                                    >
                                        <div
                                            className="
                                                flex
                                                items-center
                                                justify-between
                                            "
                                        >
                                            <span
                                                className="
                                                    font-medium
                                                "
                                            >
                                                {
                                                    inspection.inspectionDate
                                                }
                                            </span>

                                            <HealthBadge
                                                status={
                                                    inspection.observedHealth
                                                }
                                            />
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Notes
                                            </p>

                                            <p>
                                                {
                                                    inspection.notes
                                                }
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Recommendation
                                            </p>

                                            <p>
                                                {
                                                    inspection.recommendation
                                                }
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Next Inspection
                                            </p>

                                            <p>
                                                {
                                                    inspection.nextInspectionDate ??
                                                    "Not scheduled"
                                                }
                                            </p>
                                        </div>

                                        <div className="mt-4">
                                            {editingInspectionId ===
                                            inspection.id ? (
                                                <EditInspectionForm
                                                    inspection={
                                                        inspection
                                                    }
                                                    onSaved={() => {
                                                        setEditingInspectionId(
                                                            null
                                                        );

                                                        window.location.reload();
                                                    }}
                                                    onCancel={() =>
                                                        setEditingInspectionId(
                                                            null
                                                        )
                                                    }
                                                />
                                            ) : (
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                    onClick={() =>
                                                        setEditingInspectionId(
                                                            inspection.id
                                                        )
                                                    }
                                                >
                                                    Edit
                                                </Button>
                                            )}
                                        </div>
                                    </div>
                                )
                            )}
                        </div>
                    )}
                </>
            )}

            {/* ========================= */}
            {/* WORK ORDERS */}
            {/* ========================= */}

            {activeTab === "workorders" && (
                <>
                    <CreateWorkOrderForm
                        treeId={selectedTree.id}
                        onCreated={() =>
                            window.location.reload()
                        }
                    />

                    <hr className="my-6" />

                    <h3
                        className="
                            mb-4
                            font-semibold
                        "
                    >
                        Work Orders
                    </h3>

                    {workOrders.length === 0 ? (
                        <p
                            className="
                                text-sm
                                text-gray-500
                            "
                        >
                            No work orders found.
                        </p>
                    ) : (
                        <div className="space-y-4">
                            {workOrders.map(
                                (workOrder) => (
                                    <div
                                        key={
                                            workOrder.id
                                        }
                                        className="
                                            rounded-md
                                            border
                                            p-4
                                        "
                                    >
                                        <div
                                            className="
                                                flex
                                                justify-between
                                            "
                                        >
                                            <span
                                                className="
                                                    font-medium
                                                "
                                            >
                                                {
                                                    workOrder.createdDate
                                                }
                                            </span>

                                            <span
                                                className="
                                                    rounded-full
                                                    border
                                                    px-3
                                                    py-1
                                                    text-xs
                                                    font-medium
                                                "
                                            >
                                                {
                                                    workOrder.status
                                                }
                                            </span>
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Description
                                            </p>

                                            <p>
                                                {
                                                    workOrder.description
                                                }
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Due Date
                                            </p>

                                            <p>
                                                {
                                                    workOrder.dueDate
                                                }
                                            </p>
                                        </div>

                                        <div className="mt-3">
                                            <p
                                                className="
                                                    text-xs
                                                    uppercase
                                                    text-gray-500
                                                "
                                            >
                                                Completed
                                            </p>

                                            <p>
                                                {
                                                    workOrder.completedDate ??
                                                    "Not completed"
                                                }
                                            </p>
                                        </div>

                                        <div
                                            className="
                                                mt-4
                                                flex
                                                gap-2
                                            "
                                        >
                                            {workOrder.status ===
                                                "Open" && (
                                                <>
                                                    <Button
                                                        size="sm"
                                                        onClick={() =>
                                                            handleStartWorkOrder(
                                                                workOrder.id
                                                            )
                                                        }
                                                    >
                                                        Start
                                                    </Button>

                                                    <Button
                                                        variant="destructive"
                                                        size="sm"
                                                        onClick={() =>
                                                            handleCancelWorkOrder(
                                                                workOrder.id
                                                            )
                                                        }
                                                    >
                                                        Cancel
                                                    </Button>
                                                </>
                                            )}

                                            {workOrder.status ===
                                                "InProgress" && (
                                                <>
                                                    <Button
                                                        size="sm"
                                                        onClick={() =>
                                                            handleCompleteWorkOrder(
                                                                workOrder.id
                                                            )
                                                        }
                                                    >
                                                        Complete
                                                    </Button>

                                                    <Button
                                                        variant="destructive"
                                                        size="sm"
                                                        onClick={() =>
                                                            handleCancelWorkOrder(
                                                                workOrder.id
                                                            )
                                                        }
                                                    >
                                                        Cancel
                                                    </Button>
                                                </>
                                            )}
                                        </div>
                                    </div>
                                )
                            )}
                        </div>
                    )}
                </>
            )}

            {/* ========================= */}
            {/* AI ASSESSMENT */}
            {/* ========================= */}

            {activeTab === "ai" && (
                <div className="space-y-5">
                    <div>
                        <h3
                            className="
                                text-lg
                                font-semibold
                            "
                        >
                            AI-Assisted Assessment
                        </h3>

                        <p
                            className="
                                mt-1
                                text-sm
                                text-gray-500
                            "
                        >
                            Generate an AI-assisted
                            assessment using the
                            tree&apos;s current UFAMS
                            information.
                        </p>
                    </div>

                    <div
                        className="
                            rounded-md
                            border
                            bg-gray-50
                            p-4
                        "
                    >
                        <p
                            className="
                                text-xs
                                uppercase
                                text-gray-500
                            "
                        >
                            Tree
                        </p>

                        <p
                            className="
                                mt-1
                                font-medium
                            "
                        >
                            {selectedTree.assetTag}
                        </p>
                    </div>

                    <Button
                        className="w-full"
                        disabled={aiLoading}
                        onClick={
                            handleGenerateAiAssessment
                        }
                    >
                        {aiLoading
                            ? "Generating Assessment..."
                            : aiAssessment
                                ? "Regenerate Assessment"
                                : "Generate AI Assessment"}
                    </Button>

                    {aiError && (
                        <div
                            className="
                                rounded-md
                                border
                                border-red-200
                                bg-red-50
                                p-4
                                text-sm
                                text-red-700
                            "
                        >
                            <p className="font-medium">
                                Unable to generate
                                assessment
                            </p>

                            <p className="mt-1">
                                {aiError}
                            </p>
                        </div>
                    )}

                    {aiAssessment && (
                        <div className="space-y-4">
                            <div
                                className="
                                    rounded-md
                                    border
                                    p-4
                                "
                            >
                                <div
                                    className="
                                        mb-3
                                        flex
                                        items-center
                                        justify-between
                                    "
                                >
                                    <h4
                                        className="
                                            font-semibold
                                        "
                                    >
                                        Assessment
                                    </h4>

                                    <span
                                        className="
                                            rounded-full
                                            border
                                            px-2
                                            py-1
                                            text-xs
                                            text-gray-600
                                        "
                                    >
                                        AI Generated
                                    </span>
                                </div>

                                <div
                                    className="
                                        text-sm
                                        leading-6
                                        prose
                                        prose-sm
                                        max-w-none
                                    "
                                >
                                    <ReactMarkdown>
                                        {aiAssessment.summary}
                                    </ReactMarkdown>
                                </div>
                            </div>

                            <div
                                className="
                                    rounded-md
                                    border
                                    border-amber-200
                                    bg-amber-50
                                    p-4
                                    text-xs
                                    leading-5
                                    text-amber-900
                                "
                            >
                                <p className="font-semibold">
                                    Decision-support notice
                                </p>

                                <p className="mt-1">
                                    This assessment is
                                    AI-generated and is
                                    intended for
                                    decision support only.
                                    It should be reviewed
                                    by qualified personnel
                                    before operational,
                                    maintenance, or safety
                                    decisions are made.
                                </p>
                            </div>
                        </div>
                    )}

                    {!aiAssessment &&
                        !aiLoading &&
                        !aiError && (
                            <div
                                className="
                                    rounded-md
                                    border
                                    border-dashed
                                    p-6
                                    text-center
                                    text-sm
                                    text-gray-500
                                "
                            >
                                <p>
                                    No AI assessment has
                                    been generated for
                                    this tree yet.
                                </p>

                                <p className="mt-1">
                                    Click the button above
                                    to analyze this tree.
                                </p>
                            </div>
                        )}
                </div>
            )}
        </div>
    );
}