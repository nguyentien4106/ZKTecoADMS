import { Shift } from '@/types/shift';
import { ColumnDef } from '@tanstack/react-table';
import { StatusBadge } from './StatusBadge';
import { ShiftActions } from './ShiftActions';
import { PaginatedResponse, PaginationRequest } from '@/types';
import { formatDate } from '@/lib/utils';
import { SortingHeader } from '../tables/SortingHeader';
import { PaginationTable } from '../tables/PaginationTable';
interface ShiftTableProps {
    paginatedShifts: PaginatedResponse<Shift>;
    paginationRequest: PaginationRequest;
    isLoading: boolean;
    onApprove?: (shift: Shift) => void;
    onReject?: (shift: Shift) => void;
    showEmployeeInfo?: boolean;
    showActions?: boolean;
    onEdit?: (shift: Shift) => void;
    onDelete?: (id: string) => void;
    pageCount?: number;
    totalRows?: number;
    onPaginationChange: (pageNumber: number, pageSize: number) => void;
    onSortingChange?: (sorting: any) => void;
    onFiltersChange?: (filters: any) => void;
    enableRowSelection?: boolean;
    onRowSelectionChange?: (selectedRows: Shift[]) => void;
}

export const ShiftTable = ({
    isLoading,
    onApprove,
    onReject,
    showEmployeeInfo = false,
    showActions = true,
    onEdit,
    onDelete,
    paginatedShifts,
    paginationRequest,
    onPaginationChange,
    onSortingChange,
    onFiltersChange,
    onRowSelectionChange,
    enableRowSelection = false,
}: ShiftTableProps) => {
    const columns : ColumnDef<Shift>[] = [
        ...(showEmployeeInfo ? [
            { 
                accessorKey: 'employeeName', 
                header: ({ column }: any) => <SortingHeader column={column} title="Employee" />,
                enableSorting: true
            },
            { 
                accessorKey: 'employeeCode', 
                header: ({ column }: any) => <SortingHeader column={column} title="Employee Code" />,
                enableSorting: true
            }] : []),
        {
            accessorKey: 'date',
            header: ({ column }) => <SortingHeader column={column} title="Date" />,
            cell: ({ row }) => formatDate(new Date(row.getValue('date'))),
            enableSorting: true,
        },
        { 
            accessorKey: 'startTime', 
            header: ({ column }) => <SortingHeader column={column} title="Start Time" />,
            cell: ({ row }) => row.getValue('startTime'),
            enableSorting: true,
            sortDescFirst: true
        },
        { 
            accessorKey: 'endTime', 
            header: ({ column }) => <SortingHeader column={column} title="End Time" />,
            cell: ({ row }) => row.getValue('endTime'),
            enableSorting: true,
        },
        { 
            accessorKey: 'totalHours', 
            header: 'Total Hours',
            cell: ({ row }) => `${row.getValue('totalHours')}h`
        },
        { 
            accessorKey: 'breakTimeMinutes', 
            header: 'Break Time',
            cell: ({ row }) => `${row.getValue('breakTimeMinutes')} min`
        },
        { 
            accessorKey: 'description', 
            header: 'Description',
            cell: ({ row }) => {
                const description = row.getValue('description') as string | undefined;
                return description || '-';
            }
        },  
        { 
            accessorKey: 'status', 
            header: ({ column }) => <SortingHeader column={column} title="Status" />,
            cell: ({ row }) => {
                const status = row.getValue('status') as Shift['status'];
                return <StatusBadge status={status} rejectionReason={row.original.rejectionReason} />
            },
            enableSorting: true,
        },
        ...(showActions ? [{ 
            id: 'actions', 
            header: 'Actions', 
            cell: ({ row }: { row: any }) => (
                <ShiftActions
                    shift={row.original}
                    onApprove={onApprove}
                    onReject={onReject}
                    onEdit={onEdit}
                    onDelete={onDelete}
                />
            ),
            enableSorting: false,
        }] : []),
    ]

    return (
        <PaginationTable<Shift>
            columns={columns}
            data={paginatedShifts.items}
            paginationRequest={paginationRequest}
            totalCount={paginatedShifts.totalCount}
            pageNumber={paginatedShifts.pageNumber}
            pageSize={paginatedShifts.pageSize}
            isLoading={isLoading}
            onPaginationChange={onPaginationChange}
            onSortingChange={onSortingChange}
            onFiltersChange={onFiltersChange}
            manualSorting={true}
            manualFiltering={true}
            containerHeight={"calc(100vh - 350px)"}
            onRowSelectionChange={onRowSelectionChange}
            enableRowSelection={enableRowSelection}   
        />
    );
};
