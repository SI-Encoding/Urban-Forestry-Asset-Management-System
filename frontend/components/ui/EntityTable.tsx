"use client";

import {
    ColumnDef,
    flexRender,
    getCoreRowModel,
    getFilteredRowModel,
    getPaginationRowModel,
    useReactTable,
} from "@tanstack/react-table";

import {
    useState,
} from "react";


import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";


interface EntityTableProps<TData, TValue> {

    columns:
        ColumnDef<TData, TValue>[];

    data:
        TData[];

    emptyMessage?:
        string;

    onRowClick?:
        (row: TData) => void;
}


export function EntityTable<TData, TValue>({
    columns,
    data,
    emptyMessage = "No records found.",
    onRowClick,
}: EntityTableProps<TData, TValue>) {


    const [
        globalFilter,
        setGlobalFilter
    ] = useState("");


    const table =
        useReactTable({

            data,

            columns,


            state: {
                globalFilter,
            },


            onGlobalFilterChange:
                setGlobalFilter,


            getCoreRowModel:
                getCoreRowModel(),


            getFilteredRowModel:
                getFilteredRowModel(),


            getPaginationRowModel:
                getPaginationRowModel(),


            initialState: {

                pagination: {

                    pageSize: 25,

                },

            },

        });



    return (

        <div className="
            flex
            flex-col
            h-full
            space-y-4
        ">


            {/* Toolbar */}

            <div
                className="
                    flex
                    justify-between
                    items-center
                    gap-4
                    shrink-0
                "
            >

                <input

                    value={globalFilter}

                    onChange={(event) =>
                        setGlobalFilter(
                            event.target.value
                        )
                    }

                    placeholder="Search..."

                    className="
                        border
                        rounded-md
                        px-3
                        py-2
                        w-full
                        max-w-sm
                    "

                />


                <select

                    value={
                        table.getState()
                            .pagination
                            .pageSize
                    }

                    onChange={(event) =>
                        table.setPageSize(
                            Number(event.target.value)
                        )
                    }

                    className="
                        border
                        rounded-md
                        px-3
                        py-2
                    "

                >

                    {[10, 25, 50, 100].map(size => (

                        <option
                            key={size}
                            value={size}
                        >
                            {size} rows
                        </option>

                    ))}

                </select>


            </div>



            {/* Scrollable Table */}

            <div
                className="
                    flex-1
                    rounded-md
                    border
                    overflow-auto
                    min-h-0
                "
            >

                <Table className="min-w-[900px]">


                    <TableHeader>

                        {table
                            .getHeaderGroups()
                            .map(headerGroup => (

                            <TableRow
                                key={headerGroup.id}
                            >

                                {headerGroup.headers.map(header => (

                                    <TableHead
                                        key={header.id}
                                    >

                                        {
                                            header.isPlaceholder

                                                ? null

                                                : flexRender(
                                                    header
                                                        .column
                                                        .columnDef
                                                        .header,

                                                    header.getContext()
                                                )
                                        }

                                    </TableHead>

                                ))}

                            </TableRow>

                        ))}


                    </TableHeader>



                    <TableBody>


                        {
                            table
                                .getRowModel()
                                .rows
                                .length

                            ?


                            table
                                .getRowModel()
                                .rows
                                .map(row => (

                                <TableRow

                                    key={row.id}

                                    onClick={() =>
                                        onRowClick?.(
                                            row.original
                                        )
                                    }

                                    className={
                                        onRowClick

                                            ? "cursor-pointer hover:bg-muted"

                                            : undefined
                                    }

                                >

                                    {
                                        row
                                            .getVisibleCells()
                                            .map(cell => (

                                            <TableCell
                                                key={cell.id}
                                            >

                                                {
                                                    flexRender(
                                                        cell
                                                            .column
                                                            .columnDef
                                                            .cell,

                                                        cell.getContext()
                                                    )
                                                }

                                            </TableCell>

                                        ))
                                    }


                                </TableRow>


                            ))


                            :


                            (

                            <TableRow>

                                <TableCell

                                    colSpan={
                                        columns.length
                                    }

                                    className="
                                        h-24
                                        text-center
                                    "

                                >

                                    {emptyMessage}

                                </TableCell>

                            </TableRow>

                            )

                        }


                    </TableBody>


                </Table>


            </div>





            {/* Pagination */}

            <div
                className="
                    flex
                    items-center
                    justify-between
                    shrink-0
                "
            >


                <div
                    className="
                        text-sm
                        text-muted-foreground
                    "
                >

                    Page{" "}

                    {
                        table.getState()
                            .pagination
                            .pageIndex + 1
                    }

                    {" "}of{" "}

                    {
                        table.getPageCount()
                    }

                </div>



                <div
                    className="
                        flex
                        gap-2
                    "
                >

                    <button

                        onClick={() =>
                            table.previousPage()
                        }

                        disabled={
                            !table.getCanPreviousPage()
                        }

                        className="
                            border
                            rounded-md
                            px-3
                            py-1
                            disabled:opacity-50
                        "

                    >

                        Previous

                    </button>



                    <button

                        onClick={() =>
                            table.nextPage()
                        }

                        disabled={
                            !table.getCanNextPage()
                        }

                        className="
                            border
                            rounded-md
                            px-3
                            py-1
                            disabled:opacity-50
                        "

                    >

                        Next

                    </button>


                </div>


            </div>


        </div>

    );

}