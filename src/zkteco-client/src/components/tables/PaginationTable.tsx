import * as React from "react"
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  useReactTable,
  PaginationState,
  SortingState,
  ColumnFiltersState,
  RowSelectionState,
} from "@tanstack/react-table"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { Checkbox } from "@/components/ui/checkbox"
import { ChevronLeft, ChevronRight, ChevronsLeft, ChevronsRight } from "lucide-react"
import { PaginationRequest } from "@/types"
import { LoadingSpinner } from "../LoadingSpinner"

interface PaginationTableProps<TData> {
  columns: ColumnDef<TData>[]
  data: TData[]
  totalCount: number
  pageNumber: number
  pageSize: number
  paginationRequest: PaginationRequest
  onPaginationChange: (pageNumber: number, pageSize: number) => void
  isLoading?: boolean
  onSortingChange?: (sorting: SortingState) => void
  onFiltersChange?: (filters: ColumnFiltersState) => void
  manualSorting?: boolean
  manualFiltering?: boolean
  emptyMessage?: string
  className?: string
  containerHeight?: string
  // Row selection props
  enableRowSelection?: boolean
  onRowSelectionChange?: (selectedRows: TData[]) => void
  rowSelection?: RowSelectionState
  getRowId?: (row: TData, index: number) => string
}

export function PaginationTable<TData>({
  columns,
  data,
  totalCount,
  pageNumber,
  pageSize,
  onPaginationChange,
  isLoading = false,
  onSortingChange,
  onFiltersChange,
  paginationRequest,
  manualSorting = true,
  manualFiltering = true,
  emptyMessage = "No results found.",
  className,
  containerHeight = "calc(100vh - 270px)",
  enableRowSelection = false,
  onRowSelectionChange,
  rowSelection: controlledRowSelection,
  getRowId,
}: PaginationTableProps<TData>) {
  const [sorting, setSorting] = React.useState<SortingState>(paginationRequest.sortBy ? [{
    id: paginationRequest.sortBy,
    desc: paginationRequest.sortOrder === "desc"
  }] : []);
  const [columnFilters, setColumnFilters] = React.useState<ColumnFiltersState>([]);
  const [rowSelection, setRowSelection] = React.useState<RowSelectionState>({});

  // Calculate total pages
  const totalPages = Math.ceil(totalCount / pageSize)

  // Pagination state for TanStack Table
  const [{ pageIndex, pageSize: tablPageSize }, setPagination] =
    React.useState<PaginationState>({
      pageIndex: pageNumber - 1, // Convert to 0-based index
      pageSize: pageSize,
    })

  const pagination = React.useMemo(
    () => ({
      pageIndex,
      pageSize: tablPageSize,
    }),
    [pageIndex, tablPageSize]
  )

  // Sync pagination state with props
  React.useEffect(() => {
    setPagination({
      pageIndex: pageNumber - 1,
      pageSize: pageSize,
    })
  }, [pageNumber, pageSize])

  // Handle pagination change
  React.useEffect(() => {
    // Only trigger if the values actually changed
    if (pageIndex !== pageNumber - 1 || tablPageSize !== pageSize) {
      onPaginationChange(pageIndex + 1, tablPageSize)
    }
  }, [pageIndex, tablPageSize])

  // Sync sorting state with paginationRequest
  React.useEffect(() => {
    const newSorting: SortingState = paginationRequest.sortBy ? [{
      id: paginationRequest.sortBy,
      desc: paginationRequest.sortOrder === "desc"
    }] : [];
    
    setSorting(newSorting);
  }, [paginationRequest.sortBy, paginationRequest.sortOrder]);

  // Handle sorting change
  React.useEffect(() => {
    if (onSortingChange && sorting.length > 0) {
      onSortingChange(sorting)
    }
  }, [sorting, onSortingChange])

  // Handle filters change
  React.useEffect(() => {
    if (onFiltersChange && columnFilters.length > 0) {
      onFiltersChange(columnFilters)
    }
  }, [columnFilters, onFiltersChange])

  // Handle row selection change
  React.useEffect(() => {
    if (enableRowSelection && onRowSelectionChange) {
      const selectedRows = Object.keys(rowSelection)
        .filter(key => rowSelection[key])
        .map(key => {
          const index = parseInt(key)
          return data[index]
        })
        .filter(Boolean)
      
      onRowSelectionChange(selectedRows)
    }
  }, [rowSelection, enableRowSelection, onRowSelectionChange, data])

  // Create selection column
  const selectionColumn: ColumnDef<TData> = React.useMemo(() => ({
    id: 'select',
    header: ({ table }) => (
      <Checkbox
        checked={
          table.getIsAllPageRowsSelected() ||
          (table.getIsSomePageRowsSelected() && "indeterminate")
        }
        onCheckedChange={(value) => table.toggleAllPageRowsSelected(!!value)}
        aria-label="Select all"
        className="translate-y-[2px]"
      />
    ),
    cell: ({ row }) => (
      <Checkbox
        checked={row.getIsSelected()}
        onCheckedChange={(value) => row.toggleSelected(!!value)}
        aria-label="Select row"
        className="translate-y-[2px]"
      />
    ),
    enableSorting: false,
    enableHiding: false,
  }), [])

  // Combine columns with selection column if enabled
  const tableColumns = React.useMemo(
    () => enableRowSelection ? [selectionColumn, ...columns] : columns,
    [enableRowSelection, selectionColumn, columns]
  )

  const table = useReactTable({
    data,
    columns: tableColumns,
    pageCount: totalPages,
    state: {
      pagination,
      sorting,
      columnFilters,
      rowSelection: controlledRowSelection ?? rowSelection,
    },
    onPaginationChange: setPagination,
    onSortingChange: setSorting,
    onColumnFiltersChange: setColumnFilters,
    onRowSelectionChange: setRowSelection,
    getCoreRowModel: getCoreRowModel(),
    manualPagination: true,
    manualSorting,
    manualFiltering,
    enableRowSelection,
    getRowId,
  })

  return (
    <div className={`flex flex-col space-y-4 ${className || ""}`} style={{ height: containerHeight }}>
      <div className="flex-1 rounded-md border overflow-hidden flex flex-col">
        <div className="overflow-auto flex-1">
          <Table>
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
                  <TableCell
                    colSpan={tableColumns.length}
                    className="h-64 text-center"
                  >
                    <div className="flex flex-col items-center justify-center py-12 space-y-3">
                      <LoadingSpinner />
                      <span className="text-sm text-muted-foreground">Loading data...</span>
                    </div>
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
                        {flexRender(
                          cell.column.columnDef.cell,
                          cell.getContext()
                        )}
                      </TableCell>
                    ))}
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell
                    colSpan={tableColumns.length}
                    className="h-24 text-center"
                  >
                    {emptyMessage}
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </div>
      </div>

      {/* Pagination Controls */}
      <div className="flex flex-col gap-4 px-2 sm:flex-row sm:items-center sm:justify-between">
        {/* Info Section */}
        <div className="flex-1 text-sm text-muted-foreground order-2 sm:order-1">
          {enableRowSelection && table.getFilteredSelectedRowModel().rows.length > 0 ? (
            <span>
              {table.getFilteredSelectedRowModel().rows.length} of{" "}
              {table.getFilteredRowModel().rows.length} row(s) selected.
            </span>
          ) : (
            <span className="hidden sm:inline">
              Showing {data.length > 0 ? (pageNumber - 1) * pageSize + 1 : 0} to{" "}
              {Math.min(pageNumber * pageSize, totalCount)} of {totalCount} results
            </span>
          )}
        </div>

        {/* Controls Section */}
        <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:gap-6 lg:gap-8 order-1 sm:order-2">
          {/* Rows per page - Mobile optimized */}
          <div className="flex items-center justify-between sm:justify-start gap-2">
            <p className="text-sm font-medium">Rows per page</p>
            <Select
              value={`${pageSize}`}
              onValueChange={(value) => {
                table.setPageSize(Number(value))
              }}
            >
              <SelectTrigger className="h-8 w-[70px]">
                <SelectValue placeholder={pageSize} />
              </SelectTrigger>
              <SelectContent side="top">
                {[10, 20, 30, 40, 50].map((pageSize) => (
                  <SelectItem key={pageSize} value={`${pageSize}`}>
                    {pageSize}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {/* Page info and navigation - Mobile optimized */}
          <div className="flex items-center justify-between sm:justify-start gap-3 sm:gap-6">
            {/* Page indicator */}
            <div className="flex items-center justify-center text-sm font-medium min-w-[100px]">
              Page {pageNumber} of {totalPages || 1}
            </div>

            {/* Navigation buttons */}
            <div className="flex items-center gap-1 sm:gap-2">
              <Button
                variant="outline"
                className="hidden h-8 w-8 p-0 lg:flex"
                onClick={() => table.setPageIndex(0)}
                disabled={!table.getCanPreviousPage() || isLoading}
              >
                <span className="sr-only">Go to first page</span>
                <ChevronsLeft className="h-4 w-4" />
              </Button>
              <Button
                variant="outline"
                className="h-8 w-8 p-0"
                onClick={() => table.previousPage()}
                disabled={!table.getCanPreviousPage() || isLoading}
              >
                <span className="sr-only">Go to previous page</span>
                <ChevronLeft className="h-4 w-4" />
              </Button>
              <Button
                variant="outline"
                className="h-8 w-8 p-0"
                onClick={() => table.nextPage()}
                disabled={!table.getCanNextPage() || isLoading}
              >
                <span className="sr-only">Go to next page</span>
                <ChevronRight className="h-4 w-4" />
              </Button>
              <Button
                variant="outline"
                className="hidden h-8 w-8 p-0 lg:flex"
                onClick={() => table.setPageIndex(totalPages - 1)}
                disabled={!table.getCanNextPage() || isLoading}
              >
                <span className="sr-only">Go to last page</span>
                <ChevronsRight className="h-4 w-4" />
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
