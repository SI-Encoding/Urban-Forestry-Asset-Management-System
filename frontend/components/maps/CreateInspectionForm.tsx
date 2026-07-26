"use client";

import { useState } from "react";

import type { TreeHealthStatus } from "@/types/tree";

import {
    createInspection
} from "@/services/inspections";


interface CreateInspectionFormProps {

    treeId: string;

    onCreated: () => void;

}


const healthStatuses: TreeHealthStatus[] = [
    "Excellent",
    "Good",
    "Fair",
    "Poor",
    "Dead",
];


export function CreateInspectionForm({
    treeId,
    onCreated,
}: CreateInspectionFormProps) {


    const [health, setHealth] =
        useState<TreeHealthStatus>("Good");


    const [notes, setNotes] =
        useState("");


    const [recommendation, setRecommendation] =
        useState("");


    const [nextDate, setNextDate] =
        useState("");

    const [isSubmitting, setIsSubmitting] =
    useState(false);

    const [success, setSuccess] =
    useState<string | null>(null);

    const [error, setError] =
        useState<string | null>(null);

    async function submit() {

        if (!notes.trim()) {
            setError(
                "Inspection notes are required."
            );
            return;
        }


        if (!recommendation.trim()) {
            setError(
                "Recommendation is required."
            );
            return;
        }


        if (
            nextDate &&
            nextDate <
            new Date()
                .toISOString()
                .split("T")[0]
        ) {
            setError(
                "Next inspection date cannot be in the past."
            );
            return;
        }


        try {

            setIsSubmitting(true);

            setError(null);

            setSuccess(null);

            await createInspection(
                treeId,
                {
                    inspectionDate:
                        new Date()
                        .toISOString()
                        .split("T")[0],

                    observedHealth:
                        health,

                    notes:
                        notes.trim(),

                    recommendation:
                        recommendation.trim(),

                    nextInspectionDate:
                        nextDate || undefined,
                }
            );


            setNotes("");
            setRecommendation("");
            setNextDate("");

            setSuccess(
            "Inspection created successfully."
            );
            onCreated();


        } catch (error) {

            console.error(
                "Failed to create inspection:",
                error
            );


            setError(
                "Unable to create inspection."
            );


        } finally {

            setIsSubmitting(false);

        }
    }


    return (

        <div className="space-y-4">

            <h3 className="font-semibold">
                New Inspection
            </h3>
            {success && (

                <p className="
                    rounded
                    bg-green-50
                    p-2
                    text-sm
                    text-green-700
                ">
                    {success}
                </p>

            )}
            {error && (
                <p className="
                    rounded
                    bg-red-50
                    p-2
                    text-sm
                    text-red-600
                ">
                    {error}
                </p>

            )}
            <select
                className="w-full rounded border p-2"
                value={health}
                onChange={(e) =>
                    setHealth(
                        e.target.value as TreeHealthStatus
                    )
                }
            >

                {healthStatuses.map(
                    (status) => (
                        <option
                            key={status}
                            value={status}
                        >
                            {status}
                        </option>
                    )
                )}

            </select>


            <textarea
                placeholder="Inspection notes"
                className="w-full rounded border p-2"
                value={notes}
                onChange={(e) =>
                    setNotes(e.target.value)
                }
            />


            <textarea
                placeholder="Recommendation"
                className="w-full rounded border p-2"
                value={recommendation}
                onChange={(e) =>
                    setRecommendation(
                        e.target.value
                    )
                }
            />


            <input
                type="date"
                className="w-full rounded border p-2"
                value={nextDate}
                onChange={(e) =>
                    setNextDate(
                        e.target.value
                    )
                }
            />


            <button
                onClick={submit}
                disabled={isSubmitting}
                className="
                    rounded
                    bg-green-600
                    px-4
                    py-2
                    text-white
                    disabled:opacity-50
                "
            >
                {
                    isSubmitting
                        ? "Saving..."
                        : "Save Inspection"
                }
            </button>

        </div>

    );
}