import { PageHeader } from "@/components/PageHeader";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Plus, UserPlus } from "lucide-react";
import { ShiftTable } from '@/components/shifts/ShiftTable';
import { ManagementShiftFilterBar } from '@/components/shifts/ManagementShiftFilterBar';
import { ApproveShiftDialog } from '@/components/dialogs/ApproveShiftDialog';
import { RejectShiftDialog } from '@/components/dialogs/RejectShiftDialog';
import { EditShiftDialog } from '@/components/dialogs/EditShiftDialog';
import { ShiftTemplateDialog } from '@/components/dialogs/ShiftTemplateDialog';
import { AssignShiftDialog } from '@/components/dialogs/AssignShiftDialog';
import { ShiftManagementProvider, useShiftManagementContext } from '@/contexts/ShiftManagementContext';
import { useCallback, useEffect, useState } from "react";
import { ShiftTemplateList } from "@/components/shiftTemplates/ShiftTemplateList";
import { ShiftManagementFilter } from "@/types/shift";

const defaultFilter: ShiftManagementFilter = {
    employeeIds: [],
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear(),
};
const today = new Date();
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
        pendingPaginationRequest,
        allPaginationRequest,
        pendingPaginatedShifts,
        allPaginatedShifts,
        templates,
        isLoading,
        employees,
        filters,
        setFilters,
        handleApproveClick,
        handleRejectClick,
        handleEditShiftClick,
        setCreateTemplateDialogOpen,
        handleEditTemplateClick,
        handleDeleteTemplate,
        setAssignShiftDialogOpen,
        setPendingPaginationRequest,
        setAllPaginationRequest,
    } = useShiftManagementContext();

    const [activeTab, setActiveTab] = useState<string>("all");
    
    useEffect(() => {
        if (pendingPaginatedShifts && pendingPaginatedShifts?.items?.length > 0) {
            setActiveTab("pending");
        }
    }, [pendingPaginatedShifts])

    const onAllPaginationChange = (pageNumber: number, pageSize: number) => {
        setAllPaginationRequest(prev => ({
            ...prev,
            pageNumber,
            pageSize,
        }));
    };

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
        <div className="mt-6">
            <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
                <TabsList>
                    <TabsTrigger value="pending">
                        Pending
                        {
                            (pendingPaginatedShifts?.totalCount ?? 0) > 0 && (
                                <Badge
                                    variant="destructive"
                                    className="ml-2"
                                >
                                    {pendingPaginatedShifts?.totalCount}
                                </Badge>
                            )
                        }
                    </TabsTrigger>
                    <TabsTrigger value="all">
                        All Shifts
                    </TabsTrigger>
                    <TabsTrigger value="templates">
                        Templates ({templates.length})
                    </TabsTrigger>
                </TabsList>

                <TabsContent value="pending" className="mt-6">
                    {
                        pendingPaginatedShifts && (
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
                            />
                        )
                    }
                </TabsContent>

                <TabsContent value="all" className="mt-6">
                    <div className="mb-6 space-y-4">
                        <ManagementShiftFilterBar
                            selectedMonth={new Date(filters.year, filters.month - 1, 1)}
                            selectedEmployeeIds={filters.employeeIds}
                            employees={employees}
                            onMonthChange={handleMonthChange}
                            onEmployeeIdsChange={handleEmployeeIdsChange}
                            onApplyFilters={handleApplyFilters}
                            onClearFilters={handleClearFilters}
                            isLoading={isLoading}
                        />
                        
                        <div className="flex justify-end">
                            <Button onClick={() => setAssignShiftDialogOpen(true)} className="w-full sm:w-auto">
                                <UserPlus className="h-4 w-4 mr-2" />
                                Assign Shift
                            </Button>
                        </div>
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
        assignShiftDialogOpen,
        setAssignShiftDialogOpen,
        handleCreateShift,
        editShiftDialogOpen,
        setEditShiftDialogOpen,
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
                    <EditShiftDialog
                        open={editShiftDialogOpen}
                        onOpenChange={setEditShiftDialogOpen}
                        shift={selectedShift}
                    />
                </>
            )}
            <ShiftTemplateDialog />
            <AssignShiftDialog 
                open={assignShiftDialogOpen}
                onOpenChange={setAssignShiftDialogOpen}
                onSubmit={handleCreateShift}
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

