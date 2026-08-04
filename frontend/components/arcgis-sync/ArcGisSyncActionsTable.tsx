import type {
    SpatialSyncAction,
} from "@/types/arcgisSync";

import {
    ArcGisSyncActionBadge,
} from "./ArcGisSyncActionBadge";


interface Props {
    actions: SpatialSyncAction[];
}


export function ArcGisSyncActionsTable({
    actions,
}: Props) {

    return (

        <div className="rounded-lg border bg-white">

            <table className="w-full">

                <thead>

                    <tr className="border-b">

                        <th className="p-4 text-left">
                            Action
                        </th>

                        <th className="p-4 text-left">
                            Asset Tag
                        </th>

                        <th className="p-4 text-left">
                            Reason
                        </th>

                    </tr>

                </thead>


                <tbody>

                    {actions.map(action => (

                        <tr
                            key={action.assetTag}
                            className="border-b"
                        >

                            <td className="p-4">

                                <ArcGisSyncActionBadge
                                    action={action.action}
                                />

                            </td>


                            <td className="p-4">
                                {action.assetTag}
                            </td>


                            <td className="p-4">
                                {action.reason}
                            </td>

                        </tr>

                    ))}

                </tbody>

            </table>

        </div>

    );

}