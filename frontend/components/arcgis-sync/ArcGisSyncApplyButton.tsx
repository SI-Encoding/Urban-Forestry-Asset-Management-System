"use client";

import { Button } from "@/components/ui/button";


interface Props {
    disabled?: boolean;
    loading?: boolean;
    onClick: () => void;
}


export function ArcGisSyncApplyButton({
    disabled = false,
    loading = false,
    onClick,
}: Props) {

    return (

        <Button
            disabled={disabled || loading}
            onClick={onClick}
        >

            {
                loading
                    ? "Applying..."
                    : "Apply Sync"
            }

        </Button>

    );

}