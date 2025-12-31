"use client"

import * as React from "react"
import {
  ColumnDef,
  SortingState,
  VisibilityState,
  flexRender,
  getCoreRowModel,
  getSortedRowModel,
  useReactTable,
} from "@tanstack/react-table"

import {
  Table as UITable,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { LoadingSpinner } from "@/components/LoadingSpinner"

interface TableProps<TData, TValue> {
  columns: ColumnDef<TData, TValue>[]
  data: TData[]
  // Sorting props
  enableSorting?: boolean
  manualSorting?: boolean
  onSortingChange?: (sorting: SortingState) => void
  // Loading state
  isLoading?: boolean
  // Empty state
  emptyMessage?: string
  // Styling
  className?: string
  containerHeight?: string
  // Row selection
  enableRowSelection?: boolean
  onRowSelectionChange?: (selectedRows: TData[]) => void
}

export function Table<TData, TValue>({
  columns,
  data,
  enableSorting = true,
  manualSorting = false,
  onSortingChange,
  isLoading = false,
  emptyMessage = "No results found.",
  className,
  containerHeight = "calc(100vh - 270px)",
  enableRowSelection = false,
  onRowSelectionChange,
}: TableProps<TData, TValue>) {
  const [sorting, setSorting] = React.useState<SortingState>([])
  const [columnVisibility, setColumnVisibility] = React.useState<VisibilityState>({})
  const [rowSelection, setRowSelection] = React.useState({})

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    // Sorting
    enableSorting,
    manualSorting,
    onSortingChange: (updater) => {
      const newSorting = typeof updater === 'function' ? updater(sorting) : updater
      setSorting(newSorting)
      onSortingChange?.(newSorting)
    },
    getSortedRowModel: enableSorting && !manualSorting ? getSortedRowModel() : undefined,
    // Column visibility
    onColumnVisibilityChange: setColumnVisibility,
    // Row selection
    enableRowSelection,
    onRowSelectionChange: (updater) => {
      const newSelection = typeof updater === 'function' ? updater(rowSelection) : updater
      setRowSelection(newSelection)
      
      if (onRowSelectionChange) {
        const selectedRows = table.getSelectedRowModel().rows.map(row => row.original)
        onRowSelectionChange(selectedRows)
      }
    },
    state: {
      sorting,
      columnVisibility,
      rowSelection,
    },
  })

  return (
    <div className={className}>
      {/* Table */}
      <div 
        className="rounded-md border overflow-auto"
        style={{ height: containerHeight }}
      >
        <UITable>
          <TableHeader className="sticky top-0 bg-background z-10">
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id} className="whitespace-nowrap">
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext()
                          )}
                    </TableHead>
                  )
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={columns.length} className="h-24 text-center">
                  <LoadingSpinner />
                </TableCell>
              </TableRow>
            ) : table.getRowModel().rows?.length ? (
              table.getRowModel().rows.map((row) => (
                <TableRow
                  key={row.id}
                  data-state={row.getIsSelected() && "selected"}
                >
                  {row.getVisibleCells().map((cell) => (
                    <TableCell key={cell.id} className="whitespace-nowrap">
                      {flexRender(cell.column.columnDef.cell, cell.getContext())}
                    </TableCell>
                  ))}
                </TableRow>
              ))
            ) : (
              <TableRow>
                <TableCell colSpan={columns.length} className="h-24 text-center">
                  {emptyMessage}
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </UITable>
      </div>
      
      {/* Row Selection Info */}
      {enableRowSelection && table.getSelectedRowModel().rows.length > 0 && (
        <div className="flex items-center justify-between py-4">
          <div className="text-sm text-muted-foreground">
            {table.getSelectedRowModel().rows.length} of{" "}
            {table.getRowModel().rows.length} row(s) selected.
          </div>
        </div>
      )}
    </div>
  )
}
