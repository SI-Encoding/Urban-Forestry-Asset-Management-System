"use client";

import type { TreeHealthStatus } from "@/types/tree";

import { ExportGeoJsonButton } from "./ExportGeoJsonButton";

interface MapToolbarProps {
    selected: TreeHealthStatus[];
    onChange: (health: TreeHealthStatus) => void;

    search: string;
    onSearchChange: (value: string) => void;

    showHeatMap: boolean;
    onHeatMapChange: (value: boolean) => void;
}

const statuses: TreeHealthStatus[] = [
    "Excellent",
    "Good",
    "Fair",
    "Poor",
    "Dead",
];

const colors: Record<TreeHealthStatus, string> = {
    Excellent: "bg-blue-100 text-blue-700",
    Good: "bg-green-100 text-green-700",
    Fair: "bg-yellow-100 text-yellow-700",
    Poor: "bg-orange-100 text-orange-700",
    Dead: "bg-red-100 text-red-700",
};

const statusDots: Record<TreeHealthStatus, string> = {
    Excellent: "bg-blue-500",
    Good: "bg-green-500",
    Fair: "bg-yellow-500",
    Poor: "bg-orange-500",
    Dead: "bg-red-500",
};

export function MapToolbar({
    selected,
    onChange,
    search,
    onSearchChange,
    showHeatMap,
    onHeatMapChange,
}: MapToolbarProps) {

    return (
        <div className="flex items-center gap-4 rounded-lg border bg-white p-3 shadow-sm">

            <input
                value={search}
                onChange={(e) =>
                    onSearchChange(e.target.value)
                }
                placeholder="Search trees..."
                className="h-9 w-64 rounded-md border px-3 text-sm"
            />

            <div className="flex items-center gap-2">

                <span className="text-sm font-medium text-muted-foreground">
                    Health:
                </span>

                {statuses.map((status) => {

                    const active =
                        selected.includes(status);

                    return (
                        <button
                            key={status}
                            type="button"
                            onClick={() => onChange(status)}
                            className={`
                                flex items-center gap-2
                                rounded-full px-3 py-1 text-sm
                                transition
                                ${
                                    active
                                        ? colors[status]
                                        : "bg-gray-100 text-gray-400"
                                }
                            `}
                        >
                            <span
                                className={`h-2 w-2 rounded-full ${statusDots[status]}`}
                            />

                            {status}
                        </button>
                    );

                })}

                <button
                    type="button"
                    onClick={() =>
                        onHeatMapChange(!showHeatMap)
                    }
                    className={`
                        rounded-md
                        border
                        px-3
                        py-1.5
                        text-sm
                        font-medium
                        transition
                        ${
                            showHeatMap
                                ? "bg-green-600 text-white border-green-600"
                                : "bg-white text-gray-700 hover:bg-gray-100"
                        }
                    `}
                >
                    {showHeatMap
                        ? "Hide Heat Map"
                        : "Show Heat Map"}
                </button>

                <ExportGeoJsonButton />

            </div>

        </div>
    );
}
