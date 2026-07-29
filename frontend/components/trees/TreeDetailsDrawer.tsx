"use client";

import {
    Sheet,
    SheetContent,
    SheetHeader,
    SheetTitle,
    SheetDescription,
} from "@/components/ui/sheet";

import type { Tree } from "@/types/tree";
import { HealthBadge } from "@/components/ui/HealthBadge";
import { Button } from "@/components/ui/button";
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
            <SheetContent>

                <SheetHeader>

                    <SheetTitle>
                        {tree.assetTag}
                    </SheetTitle>

                    <SheetDescription>
                        Tree Asset Details
                    </SheetDescription>

                </SheetHeader>


                <div className="mt-6 space-y-4">


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
                            {tree.location.latitude},
                            {" "}
                            {tree.location.longitude}
                        </p>
                    </div>
                    <div className="pt-4">

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