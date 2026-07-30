interface TreeDetailsPageProps {
    params: {
        id: string;
    };
}


export default async function TreeDetailsPage({
    params,
}: TreeDetailsPageProps) {

    return (
        <div>
            <h1>
                Tree Details Page Works
            </h1>

            <p>
                ID: {params.id}
            </p>
        </div>
    );
}