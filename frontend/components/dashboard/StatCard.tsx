import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

interface StatCardProps {
    title: string;
    value: number;
}

export function StatCard({
    title,
    value,
}: StatCardProps) {
    return (
        <Card>
            <CardHeader>
                <CardTitle className="text-sm text-muted-foreground">
                    {title}
                </CardTitle>
            </CardHeader>

            <CardContent>
                <p className="text-3xl font-bold">
                    {value}
                </p>
            </CardContent>
        </Card>
    );
}