import { Badge } from "@/components/ui/badge";

interface Props {
    action: string;
}

export function ArcGisSyncActionBadge({
    action,
}: Props) {

    return (
        <Badge>
            {action}
        </Badge>
    );
}