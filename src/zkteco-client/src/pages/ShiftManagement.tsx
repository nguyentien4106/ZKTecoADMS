// ==========================================
// src/pages/ShiftManagement.tsx
// ==========================================
import { PageHeader } from "@/components/PageHeader";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ShiftList } from '@/components/shifts/ShiftList';
import { ApproveShiftDialog } from '@/components/dialogs/ApproveShiftDialog';
import { RejectShiftDialog } from '@/components/dialogs/RejectShiftDialog';
import { ShiftManagementProvider, useShiftManagementContext } from '@/contexts/ShiftManagementContext';

const ShiftManagementHeader = () => {
    return (
        <PageHeader
            title="Shift Management"
            description="Review and manage employee shift requests"
        />
    );
};

const ShiftManagementTabs = () => {
    const {
        pendingShifts,
        allShifts,
        isLoading,
        handleApproveClick,
        handleRejectClick,
    } = useShiftManagementContext();

    return (
        <div className="mt-6">
            <Tabs defaultValue="pending" className="w-full">
                <TabsList>
                    <TabsTrigger value="pending">
                        Pending ({pendingShifts.length})
                    </TabsTrigger>
                    <TabsTrigger value="all">
                        All Shifts
                    </TabsTrigger>
                </TabsList>

                <TabsContent value="pending" className="mt-6">
                    <ShiftList
                        shifts={pendingShifts}
                        isLoading={isLoading}
                        onApprove={handleApproveClick}
                        onReject={handleRejectClick}
                        showEmployeeInfo={true}
                        showActions={true}
                    />
                </TabsContent>

                <TabsContent value="all" className="mt-6">
                    <ShiftList
                        shifts={allShifts}
                        isLoading={isLoading}
                        showEmployeeInfo={true}
                        showActions={false}
                    />
                </TabsContent>
            </Tabs>
        </div>
    );
};

const ShiftManagementDialogs = () => {
    const {
        approveDialogOpen,
        rejectDialogOpen,
        selectedShift,
        setApproveDialogOpen,
        setRejectDialogOpen,
        handleApprove,
        handleReject,
    } = useShiftManagementContext();

    if (!selectedShift) return null;

    return (
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
    );
};

const ShiftManagementContent = () => {
    return (
        <div>
            <ShiftManagementHeader />
            <ShiftManagementTabs />
            <ShiftManagementDialogs />
        </div>
    );
};

export const ShiftManagement = () => {
    return (
        <ShiftManagementProvider>
            <ShiftManagementContent />
        </ShiftManagementProvider>
    );
};

