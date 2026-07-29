"use client";

interface NativeBadgeProps {
    isNative: boolean;
}

export function NativeBadge({
    isNative,
}: NativeBadgeProps) {

    return (

        <span
            className={`
                inline-flex
                items-center
                rounded-full
                px-3
                py-1
                text-sm
                font-medium

                ${
                    isNative
                        ? "bg-green-100 text-green-700"
                        : "bg-gray-100 text-gray-700"
                }
            `}
        >

            {isNative
                ? "Native"
                : "Non-Native"}

        </span>

    );

}