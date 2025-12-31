import { PageHeader } from "@/components/PageHeader";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { ShiftTable } from '@/components/shifts/ShiftTable';
import { ApproveShiftDialog } from '@/components/shifts/dialogs/ApproveShiftDialog';
import { RejectShiftDialog } from '@/components/shifts/dialogs/RejectShiftDialog';
import { ShiftManagementProvider, useShiftManagementContext } from '@/contexts/ShiftManagementContext';
import { useCallback, useState } from "react";
import { Shift } from "@/types/shift";
import { CheckCircle, UserPlus, XCircle } from "lucide-react";
import { AssignShiftDialog } from "@/components/shifts/dialogs/AssignShiftDialog";

const PendingShiftsContent = () => {
    const {
        pendingPaginationRequest,
        pendingPaginatedShifts,
        isLoading,
        approveDialogOpen,
        rejectDialogOpen,
        selectedShift,
        assignShiftDialogOpen,
        handleAssignShift,
        setApproveDialogOpen,
        setRejectDialogOpen,
        handleApproveClick,
        handleRejectClick,
        handleApprove,
        handleReject,
        setPendingPaginationRequest,
        handleApproveSelected,
        handleRejectSelected,
        setAssignShiftDialogOpen
    } = useShiftManagementContext();
    const [shiftsSelected, setShiftsSelected] = useState<Shift[]>([]);

    const onPendingPaginationChange = (pageNumber: number, pageSize: number) => {
        setPendingPaginationRequest(prev => ({
            ...prev,
            pageNumber,
            pageSize,
        }));
    };

    const onPendingSortingChange = useCallback((sorting: any) => {
        setPendingPaginationRequest(prev => ({
            ...prev,
            sortBy: sorting.length > 0 ? sorting[0].id : undefined,
            sortOrder: sorting.length > 0 ? (sorting[0].desc ? 'desc' : 'asc') : undefined,
            pageNumber: 1, // Reset to first page when sorting changes
        }));
    }, [setPendingPaginationRequest]);

    return (
        <div className="space-y-6">
            <PageHeader
                title="Requesting Shifts"
                action={
                    <>
                    <div className="flex justify-end">
                        <Button onClick={() => setAssignShiftDialogOpen(true)} className="w-full sm:w-auto">
                            <UserPlus className="h-4 w-4 mr-2" />
                            Assign Shift
                        </Button>
                    </div>
                    </>
                    
                }
            />
            {
                shiftsSelected.length > 0 && (
                    <div className="flex items-center gap-4 p-4 bg-muted rounded-lg border">
                        <Badge variant="secondary" className="text-lg px-4 py-2">
                            {shiftsSelected.length} Selected
                        </Badge>
                        <div className="flex gap-2">
                            <Button
                                variant="default"
                                className="bg-green-600 hover:bg-green-700"
                                onClick={async () => {
                                    const shiftIds = shiftsSelected.map(shift => shift.id);
                                    await handleApproveSelected(shiftIds);
                                    setShiftsSelected([]);
                                }}
                            >
                                <CheckCircle className="h-4 w-4 mr-2" />
                                Approve Selected
                            </Button>
                            <Button
                                variant="destructive"
                                onClick={async () => {
                                    const shiftIds = shiftsSelected.map(shift => shift.id);
                                    await handleRejectSelected(shiftIds, "Bulk rejection");
                                    setShiftsSelected([]);
                                }}
                            >
                                <XCircle className="h-4 w-4 mr-2" />
                                Reject Selected
                            </Button>
                        </div>
                    </div>
                )
            }
            {pendingPaginatedShifts && (
                <ShiftTable
                    paginatedShifts={pendingPaginatedShifts}
                    paginationRequest={pendingPaginationRequest}
                    isLoading={isLoading}
                    onApprove={handleApproveClick}
                    onReject={handleRejectClick}
                    showEmployeeInfo={true}
                    showActions={true}
                    onPaginationChange={onPendingPaginationChange}
                    onSortingChange={onPendingSortingChange}
                    onRowSelectionChange={setShiftsSelected}
                    enableRowSelection={true}
                />
            )}

            {selectedShift && (
                <>
                    <ApproveShiftDialog
                        open={approveDialogOpen}
                        onOpenChange={setApproveDialogOpen}
                        shift={selectedShift}
                        onConfirm={() => handleApprove(selectedShift.id)}
                    />
                    <RejectShiftDialog
                        open={rejectDialogOpen}
                        onOpenChange={setRejectDialogOpen}
                        shift={selectedShift}
                        onSubmit={(reason: string) => handleReject(selectedShift.id, reason)}
                    />
                </>
            )}

            {
                assignShiftDialogOpen && (
                    <AssignShiftDialog 
                        open={assignShiftDialogOpen}
                        onOpenChange={setAssignShiftDialogOpen}
                        onSubmit={handleAssignShift}
                    />
                )
            }
        </div>
    );
};

export const PendingShifts = () => {
    return (
        <ShiftManagementProvider>
            <PendingShiftsContent />
        </ShiftManagementProvider>
    );
};

export default PendingShifts;
