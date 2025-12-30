import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ShiftTable } from '@/components/shifts/ShiftTable';
import { MyShiftsFilterBar } from '@/components/shifts/MyShiftsFilterBar';
import { ShiftRequestDialog } from '@/components/dialogs/ShiftRequestDialog';
import { ShiftProvider, useShiftContext } from '@/contexts/ShiftContext';
import { useCallback, useMemo } from 'react';
import { useAuth } from "@/contexts/AuthContext";

const MyShiftsContent = () => {
    const { 
        shifts,
        isLoading, 
        setDialogMode,
        handleEdit,
        handleDelete,
        selectedMonth,
        selectedStatus,
        setSelectedMonth,
        setSelectedStatus,
        applyFilters,
        clearFilters,
    } = useShiftContext();
    
    const { isHourlyEmployee } = useAuth();

    // Convert shifts array to paginated format for the table
    const paginatedShifts = useMemo(() => ({
        items: shifts,
        totalCount: shifts.length,
        pageNumber: 1,
        pageSize: shifts.length,
        totalPages: 1,
        hasPreviousPage: false,
        hasNextPage: false
    }), [shifts]);

    const paginationRequest = useMemo(() => ({
        pageNumber: 1,
        pageSize: shifts.length,
    }), [shifts.length]);

    const onPaginationChange = useCallback(() => {
        // Pagination disabled for this view since we show all shifts for the month
    }, []);

    const onSortingChange = useCallback(() => {
        // Sorting handled client-side if needed
    }, []);

    const onFiltersChange = useCallback(() => {
        // Filters handled by filter bar
    }, []);

    const handleMonthChange = useCallback((date: Date | undefined) => {
        if (date) {
            setSelectedMonth(date);
        }
    }, [setSelectedMonth]);

    return (
        <div>
            <PageHeader
                title="My Shifts"
                description="Manage your shift schedules"
                action={
                    isHourlyEmployee && (
                        <Button onClick={() => setDialogMode('create')}>
                            <Plus className="w-4 h-4 mr-2" />
                            Request Shift
                        </Button>
                    )
                }
            />

            <MyShiftsFilterBar
                selectedMonth={selectedMonth}
                selectedStatus={selectedStatus}
                onMonthChange={handleMonthChange}
                onStatusChange={setSelectedStatus}
                onApplyFilters={applyFilters}
                onClearFilters={clearFilters}
            />

            <div className="mt-6">
                <ShiftTable
                    paginatedShifts={paginatedShifts}
                    paginationRequest={paginationRequest}
                    isLoading={isLoading}
                    showEmployeeInfo={false}
                    showActions={true}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                    onPaginationChange={onPaginationChange}
                    onSortingChange={onSortingChange}
                    onFiltersChange={onFiltersChange}
                />
            </div>
            
            <ShiftRequestDialog />
        </div>
    );
};

export const MyShifts = () => {
    return (
        <ShiftProvider>
            <MyShiftsContent />
        </ShiftProvider>
    );
};
