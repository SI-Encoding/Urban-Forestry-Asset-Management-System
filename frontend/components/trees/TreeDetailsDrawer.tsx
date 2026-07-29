"use client";

import {
    Sheet,
    SheetContent,
    SheetDescription,
    SheetHeader,
    SheetTitle,
} from "@/components/ui/sheet";

import { Button } from "@/components/ui/button";
import { HealthBadge } from "@/components/ui/HealthBadge";

import type { Tree } from "@/types/tree";

import { useRouter } from "next/navigation";

interface TreeDetailsDrawerProps {
    tree: Tree | null;
    open: boolean;
    onOpenChange: (open: boolean) => void;
}

export function TreeDetailsDrawer({
    tree,
    open,
    onOpenChange,
}: TreeDetailsDrawerProps) {

    const router = useRouter();

    if (!tree) {
        return null;
    }

    return (

        <Sheet
            open={open}
            onOpenChange={onOpenChange}
        >

            <SheetContent className="w-[420px] sm:max-w-[420px] p-0">

                <div className="h-full overflow-y-auto">

                    <SheetHeader className="border-b px-6 py-6">

                        <SheetTitle>
                            {tree.assetTag}
                        </SheetTitle>

                        <SheetDescription>
                            Tree Asset Details
                        </SheetDescription>

                    </SheetHeader>

                    <div className="space-y-6 p-6">

                        <div>

                            <p className="text-sm text-muted-foreground">
                                Species
                            </p>

                            <p className="font-medium">
                                {tree.speciesName}
                            </p>

                        </div>

                        <div>

                            <p className="text-sm text-muted-foreground">
                                Park
                            </p>

                            <p className="font-medium">
                                {tree.parkName}
                            </p>

                        </div>

                        <div>

                            <p className="text-sm text-muted-foreground">
                                Health
                            </p>

                            <HealthBadge
                                status={tree.healthStatus}
                            />

                        </div>

                        <div>

                            <p className="text-sm text-muted-foreground">
                                Location
                            </p>

                            <p className="font-mono text-sm">
                                {tree.location.latitude},{" "}
                                {tree.location.longitude}
                            </p>

                        </div>

                        <Button
                            className="w-full"
                            onClick={() =>
                                router.push(
                                    `/map?treeId=${tree.id}`
                                )
                            }
                        >
                            View on Map
                        </Button>

                    </div>

                </div>

            </SheetContent>

        </Sheet>

    );

}