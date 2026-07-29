import {

    Card,

    CardContent,

    CardHeader,

    CardTitle,

} from "@/components/ui/card";



interface StatCardProps {

    title:
        string;

    value:
        number;

}



export function StatCard({

    title,

    value,

}: StatCardProps) {


    return (

        <Card

            className="
                transition-shadow
                hover:shadow-md
            "

        >


            <CardHeader

                className="
                    pb-2
                "

            >

                <CardTitle

                    className="
                        text-sm
                        font-medium
                        text-muted-foreground
                    "

                >

                    {title}

                </CardTitle>


            </CardHeader>



            <CardContent>


                <p

                    className="
                        text-4xl
                        font-bold
                    "

                >

                    {(value ?? 0).toLocaleString()}


                </p>


            </CardContent>


        </Card>

    );

}