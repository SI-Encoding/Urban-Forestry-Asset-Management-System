import { Button } from "@/components/ui/button";


interface Props {
    loading: boolean;
    onClick: () => void;
}


export function ArcGisSyncPreviewButton({
    loading,
    onClick,
}: Props) {

    return (

        <Button
            onClick={onClick}
            disabled={loading}
        >

            {
                loading
                    ? "Loading..."
                    : "Preview Sync"
            }

        </Button>

    );

}