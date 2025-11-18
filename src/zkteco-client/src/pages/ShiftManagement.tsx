// ==========================================
// src/pages/ShiftManagement.tsx
// ==========================================
import { PageHeader } from "@/components/PageHeader";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { ShiftList } from '@/components/shifts/ShiftList';
import { ShiftTemplateList } from '@/components/shifts/ShiftTemplateList';
import { ApproveShiftDialog } from '@/components/dialogs/ApproveShiftDialog';
import { RejectShiftDialog } from '@/components/dialogs/RejectShiftDialog';
import { CreateShiftTemplateDialog } from '@/components/dialogs/CreateShiftTemplateDialog';
import { UpdateShiftTemplateDialog } from '@/components/dialogs/UpdateShiftTemplateDialog';
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
        templates,
        isLoading,
        handleApproveClick,
        handleRejectClick,
        setCreateTemplateDialogOpen,
        handleEditTemplateClick,
        handleDeleteTemplate,
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
                    <TabsTrigger value="templates">
                        Templates ({templates.length})
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

                <TabsContent value="templates" className="mt-6">
                    <div className="mb-4 flex justify-end">
                        <Button onClick={() => setCreateTemplateDialogOpen(true)}>
                            <Plus className="h-4 w-4 mr-2" />
                            Create Template
                        </Button>
                    </div>
                    <ShiftTemplateList
                        templates={templates}
                        isLoading={isLoading}
                        onEdit={handleEditTemplateClick}
                        onDelete={handleDeleteTemplate}
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
        createTemplateDialogOpen,
        updateTemplateDialogOpen,
        selectedTemplate,
        setCreateTemplateDialogOpen,
        setUpdateTemplateDialogOpen,
        handleCreateTemplate,
        handleUpdateTemplate,
    } = useShiftManagementContext();

    return (
        <>
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
            <CreateShiftTemplateDialog
                open={createTemplateDialogOpen}
                onOpenChange={setCreateTemplateDialogOpen}
                onCreate={handleCreateTemplate}
            />
            <UpdateShiftTemplateDialog
                open={updateTemplateDialogOpen}
                onOpenChange={setUpdateTemplateDialogOpen}
                template={selectedTemplate}
                onUpdate={handleUpdateTemplate}
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

