import { PageHeader } from "@/components/PageHeader";
import { ShiftTable } from '@/components/shifts/ShiftTable';
import { AllShiftsFilterBar } from '@/components/shifts/AllShiftsFilterBar';
import { EditShiftDialog } from '@/components/shifts/dialogs/EditShiftDialog';
import { ShiftManagementProvider, useShiftManagementContext } from '@/contexts/ShiftManagementContext';
import { useCallback } from "react";

const AllShiftsContent = () => {
    const {
        allPaginationRequest,
        allPaginatedShifts,
        isLoading,
        employees,
        filters,
        setFilters,
        handleEditShiftClick,
        setAllPaginationRequest,
    } = useShiftManagementContext();

    const onAllPaginationChange = (pageNumber: number, pageSize: number) => {
        setAllPaginationRequest(prev => ({
            ...prev,
            pageNumber,
            pageSize,
        }));
    };

    const onAllSortingChange = useCallback((sorting: any) => {
        setAllPaginationRequest(prev => ({
            ...prev,
            sortBy: sorting.length > 0 ? sorting[0].id : undefined,
            sortOrder: sorting.length > 0 ? (sorting[0].desc ? 'desc' : 'asc') : undefined,
            pageNumber: 1, // Reset to first page when sorting changes
        }));
    }, [setAllPaginationRequest]);

    const handleMonthChange = (date: Date | undefined) => {
        if (date) {
            setFilters(prev => ({
                ...prev,
                month: date.getMonth() + 1,
                year: date.getFullYear(),
            }));
        }
    };

    const handleEmployeeIdsChange = (employeeIds: string[]) => {
        setFilters(prev => ({
            ...prev,
            employeeIds,
        }));
    };

    const handleApplyFilters = () => {
        setAllPaginationRequest(prev => ({
            ...prev,
            pageNumber: 1, // Reset to first page when applying filters
        }));
    };

    const handleClearFilters = () => {
        const today = new Date();
        setFilters({
            employeeIds: [],
            month: today.getMonth() + 1,
            year: today.getFullYear(),
        });
        setAllPaginationRequest(prev => ({
            ...prev,
            pageNumber: 1,
        }));
    };

    return (
        <div>
            <div className="mt-6">
                <div className="mb-6 space-y-4">
                    <AllShiftsFilterBar
                        selectedMonth={new Date(filters.year, filters.month - 1, 1)}
                        selectedEmployeeIds={filters.employeeIds}
                        employees={employees}
                        onMonthChange={handleMonthChange}
                        onEmployeeIdsChange={handleEmployeeIdsChange}
                        onApplyFilters={handleApplyFilters}
                        onClearFilters={handleClearFilters}
                        isLoading={isLoading}
                    />
                </div>
                
                {
                    allPaginatedShifts && (
                        <ShiftTable
                            paginatedShifts={allPaginatedShifts}
                            paginationRequest={allPaginationRequest}
                            isLoading={isLoading}
                            showEmployeeInfo={true}
                            showActions={true}
                            onEdit={handleEditShiftClick}
                            onPaginationChange={onAllPaginationChange}
                            onSortingChange={onAllSortingChange}
                        />
                    )
                }
            </div>

            <AllShiftsDialogs />
        </div>
    );
};

const AllShiftsDialogs = () => {
    const {
        editShiftDialogOpen,
        setEditShiftDialogOpen,
        selectedShift,
    } = useShiftManagementContext();

    return (
        <>
            {selectedShift && (
                <EditShiftDialog
                    open={editShiftDialogOpen}
                    onOpenChange={setEditShiftDialogOpen}
                    shift={selectedShift}
                />
            )}
        </>
    );
};

export const AllShifts = () => {
    return (
        <ShiftManagementProvider>
            <AllShiftsContent />
        </ShiftManagementProvider>
    );
};

