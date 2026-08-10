"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

import {
    LayoutDashboard,
    Map,
    Trees,
    TreesIcon,
    ClipboardCheck,
    Wrench,
    RefreshCw,
    History,
} from "lucide-react";


const links = [
    {
        href: "/",
        label: "Dashboard",
        icon: LayoutDashboard,
    },
    {
        href: "/map",
        label: "Map",
        icon: Map,
    },
    {
        href: "/trees",
        label: "Trees",
        icon: Trees,
    },
    {
        href: "/parks",
        label: "Parks",
        icon: TreesIcon,
    },
    {
        href: "/species",
        label: "Species",
        icon: TreesIcon,
    },
    {
        href: "/inspections",
        label: "Inspections",
        icon: ClipboardCheck,
    },
    {
        href: "/work-orders",
        label: "Work Orders",
        icon: Wrench,
    },
    {
        href: "/arcgis-sync",
        label: "ArcGIS Sync",
        icon: RefreshCw,
    },
    {
        href: "/sync-history",
        label: "Sync History",
        icon: History,
    },
];


export function Sidebar() {

    const pathname =
        usePathname();


    return (

        <aside
            className="
                flex
                w-64
                flex-col
                border-r
                bg-white
                shadow-sm
            "
        >

            <div
                className="
                    border-b
                    p-6
                "
            >

                <h1
                    className="
                        text-2xl
                        font-bold
                        text-green-700
                    "
                >
                    UFAMS
                </h1>

                <p
                    className="
                        mt-1
                        text-sm
                        text-muted-foreground
                    "
                >
                    Urban Forest Asset Management
                </p>

            </div>


            <nav
                className="
                    flex
                    flex-col
                    p-3
                "
            >

                {links.map(
                    (link) => {

                        const Icon =
                            link.icon;


                        return (

                            <Link
                                key={
                                    link.href
                                }
                                href={
                                    link.href
                                }
                                className={`
                                    flex
                                    items-center
                                    gap-3
                                    rounded-lg
                                    px-4
                                    py-3
                                    transition

                                    ${
                                        pathname ===
                                        link.href
                                            ? "bg-green-600 text-white"
                                            : "hover:bg-gray-100"
                                    }
                                `}
                            >

                                <Icon
                                    size={18}
                                />

                                {
                                    link.label
                                }

                            </Link>

                        );

                    }
                )}

            </nav>

        </aside>

    );

}
