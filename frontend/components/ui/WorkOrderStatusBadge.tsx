import type {
    WorkOrderStatus
} from "@/types/workOrder";


interface Props {

    status: WorkOrderStatus;

}


const colors:
Record<WorkOrderStatus,string> = {

    Open:
        "bg-blue-100 text-blue-700",

    Assigned:
        "bg-purple-100 text-purple-700",

    InProgress:
        "bg-yellow-100 text-yellow-700",

    Completed:
        "bg-green-100 text-green-700",

    Cancelled:
        "bg-red-100 text-red-700",

};



const dots:
Record<WorkOrderStatus,string> = {

    Open:
        "bg-blue-500",

    Assigned:
        "bg-purple-500",

    InProgress:
        "bg-yellow-500",

    Completed:
        "bg-green-500",

    Cancelled:
        "bg-red-500",

};



export function WorkOrderStatusBadge({
    status
}: Props) {


    return (

        <span
            className={`
                inline-flex
                items-center
                gap-2
                rounded-full
                px-3
                py-1
                text-sm
                ${colors[status]}
            `}
        >

            <span
                className={`
                    h-2
                    w-2
                    rounded-full
                    ${dots[status]}
                `}
            />

            {status}

        </span>

    );

}