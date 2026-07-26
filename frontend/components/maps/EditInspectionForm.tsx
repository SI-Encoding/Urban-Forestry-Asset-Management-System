"use client";

import { useState } from "react";

import type { Inspection } from "@/types/inspection";

import {
    updateInspectionNotes,
    updateInspectionRecommendation,
    scheduleFollowUp,
} from "@/services/inspections";


interface EditInspectionFormProps {

    inspection: Inspection;

    onSaved: () => void;

    onCancel: () => void;

}


export function EditInspectionForm({
    inspection,
    onSaved,
    onCancel,
}: EditInspectionFormProps) {


    const [notes, setNotes] =
        useState(
            inspection.notes
        );


    const [recommendation, setRecommendation] =
        useState(
            inspection.recommendation
        );


    const [nextDate, setNextDate] =
        useState(
            inspection.nextInspectionDate ?? ""
        );


    const [error, setError] =
        useState<string | null>(null);


    const [isSaving, setIsSaving] =
        useState(false);



    async function save() {

        if (!notes.trim()) {

            setError(
                "Notes are required."
            );

            return;
        }


        if (!recommendation.trim()) {

            setError(
                "Recommendation is required."
            );

            return;
        }


        try {

            setIsSaving(true);

            setError(null);


            await updateInspectionNotes(
                inspection.id,
                notes.trim()
            );


            await updateInspectionRecommendation(
                inspection.id,
                recommendation.trim()
            );


            await scheduleFollowUp(
                inspection.id,
                nextDate || undefined
            );


            onSaved();


        } catch (error) {

            console.error(
                "Failed to update inspection:",
                error
            );


            setError(
                "Unable to update inspection."
            );


        } finally {

            setIsSaving(false);

        }

    }



    return (

        <div className="space-y-4 rounded-md border p-4">

            <h4 className="font-semibold">
                Edit Inspection
            </h4>


            {error && (

                <p
                    className="
                        rounded
                        bg-red-50
                        p-2
                        text-sm
                        text-red-600
                    "
                >
                    {error}
                </p>

            )}


            <div>

                <label
                    className="
                        mb-1
                        block
                        text-xs
                        uppercase
                        text-gray-500
                    "
                >
                    Notes
                </label>


                <textarea
                    className="
                        w-full
                        rounded
                        border
                        p-2
                    "
                    value={notes}
                    onChange={(e) =>
                        setNotes(
                            e.target.value
                        )
                    }
                />

            </div>



            <div>

                <label
                    className="
                        mb-1
                        block
                        text-xs
                        uppercase
                        text-gray-500
                    "
                >
                    Recommendation
                </label>


                <textarea
                    className="
                        w-full
                        rounded
                        border
                        p-2
                    "
                    value={recommendation}
                    onChange={(e) =>
                        setRecommendation(
                            e.target.value
                        )
                    }
                />

            </div>



            <div>

                <label
                    className="
                        mb-1
                        block
                        text-xs
                        uppercase
                        text-gray-500
                    "
                >
                    Next Inspection
                </label>


                <input
                    type="date"
                    className="
                        w-full
                        rounded
                        border
                        p-2
                    "
                    value={nextDate}
                    onChange={(e) =>
                        setNextDate(
                            e.target.value
                        )
                    }
                />

            </div>



            <div className="flex gap-2">

                <button
                    onClick={save}
                    disabled={isSaving}
                    className="
                        rounded
                        bg-blue-600
                        px-4
                        py-2
                        text-white
                        disabled:opacity-50
                    "
                >
                    {
                        isSaving
                            ? "Saving..."
                            : "Save"
                    }
                </button>


                <button
                    onClick={onCancel}
                    disabled={isSaving}
                    className="
                        rounded
                        border
                        px-4
                        py-2
                    "
                >
                    Cancel
                </button>

            </div>


        </div>

    );

}