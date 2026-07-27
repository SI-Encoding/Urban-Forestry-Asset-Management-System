"use client";

export type TreeDetailsTab =
    | "overview"
    | "inspections"
    | "workorders";

interface Props {
    active: TreeDetailsTab;

    onChange: (
        tab: TreeDetailsTab
    ) => void;
}

export function TreeDetailsTabs({
    active,
    onChange,
}: Props) {

    const tabs: TreeDetailsTab[] = [
        "overview",
        "inspections",
        "workorders",
    ];

    return (

        <div className="mb-6 flex border-b">

            {tabs.map((tab) => (

                <button
                    key={tab}
                    onClick={() =>
                        onChange(tab)
                    }
                    className={`
                        px-4 py-2 text-sm capitalize
                        border-b-2 transition

                        ${
                            active === tab
                                ? "border-blue-600 font-semibold"
                                : "border-transparent text-gray-500"
                        }
                    `}
                >
                    {tab === "workorders"
                        ? "Work Orders"
                        : tab === "overview"
                            ? "Overview"
                            : "Inspections"}
                </button>

            ))}

        </div>

    );

}