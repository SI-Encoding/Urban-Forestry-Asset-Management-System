interface AreaBadgeProps {
    hectares: number;
}

export function AreaBadge({
    hectares,
}: AreaBadgeProps) {

    let classes =
        "bg-green-100 text-green-700";

    if (hectares >= 100) {
        classes =
            "bg-blue-100 text-blue-700";
    }

    if (hectares >= 400) {
        classes =
            "bg-purple-100 text-purple-700";
    }

    return (

        <span
            className={`
                rounded-full
                px-3
                py-1
                text-sm
                font-medium
                ${classes}
            `}
        >
            {hectares} ha
        </span>

    );

}