export function formatDate(
    value?: string | null
) {
    if (!value) {
        return "-";
    }

    const date =
        new Date(value);

    return [
        date.getFullYear(),
        String(date.getMonth() + 1)
            .padStart(2, "0"),
        String(date.getDate())
            .padStart(2, "0"),
    ].join("-");
}