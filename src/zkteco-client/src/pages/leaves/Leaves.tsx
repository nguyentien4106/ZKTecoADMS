import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { LeavesTable } from '@/components/leaves/LeavesTable';
import { LeaveRequestDialog } from '@/components/dialogs/LeaveRequestDialog';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { LeaveProvider, useLeaveContext } from '@/contexts/LeaveContext';
import { useCallback } from "react";

const LeavesHeader = () => {
  const { setDialogMode } = useLeaveContext();

  return (
    <PageHeader
      title="Leaves Management"
      description="View and manage leave requests"
      action={
        <Button onClick={() => setDialogMode('create')}>
          <Plus className="mr-2 h-4 w-4" />
          Request Leave
        </Button>
      }
    />
  );
};

const LeavesTableSection = () => {
  const {
    paginationRequest,
    paginatedLeaves,
    isLoading,
    setPaginationRequest
  } = useLeaveContext();

  const onPaginationChange = useCallback((pageNumber: number, pageSize: number) => {
    setPaginationRequest(prev => ({
      ...prev,
      pageNumber: pageNumber,
      pageSize: pageSize
    }));
  }, [setPaginationRequest]);

  const onAllSortingChange = useCallback((sorting: any) => {
    setPaginationRequest(prev => ({
      ...prev,
      sortBy: sorting.length > 0 ? sorting[0].id : undefined,
      sortOrder: sorting.length > 0 ? (sorting[0].desc ? 'desc' : 'asc') : undefined,
      pageNumber: 1,
    }));
  }, [setPaginationRequest]);

  if (!paginatedLeaves) {
    return null;
  }

  return (
    <div className="mt-6">
      <LeavesTable
        paginatedLeaves={paginatedLeaves}
        isLoading={isLoading}
        showActions={true}
        onPaginationChange={onPaginationChange}
        paginationRequest={paginationRequest}
        onSortingChange={onAllSortingChange}
      />
    </div>
  );
};

const LeavesDialogs = () => {
  const {
    approveDialogOpen,
    rejectDialogOpen,
    cancelDialogOpen,
    selectedLeave,
    rejectionReason,
    setApproveDialogOpen,
    setRejectDialogOpen,
    setCancelDialogOpen,
    setRejectionReason,
    handleApprove,
    handleReject,
    handleCancel,
  } = useLeaveContext();

  return (
    <>
      <LeaveRequestDialog />

      {/* Cancel Leave Dialog */}
      <AlertDialog open={cancelDialogOpen} onOpenChange={setCancelDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Cancel Leave Request</AlertDialogTitle>
            <AlertDialogDescription>
              Are you sure you want to cancel this leave request? This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>No, keep it</AlertDialogCancel>
            <AlertDialogAction onClick={() => selectedLeave && handleCancel(selectedLeave.id)}>
              Yes, cancel request
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>

      {/* Approve Leave Dialog */}
      <AlertDialog open={approveDialogOpen} onOpenChange={setApproveDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Approve Leave Request</AlertDialogTitle>
            <AlertDialogDescription>
              Are you sure you want to approve this leave request?
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction onClick={() => selectedLeave && handleApprove(selectedLeave.id)}>
              Approve
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>

      {/* Reject Leave Dialog */}
      <AlertDialog open={rejectDialogOpen} onOpenChange={setRejectDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Reject Leave Request</AlertDialogTitle>
            <AlertDialogDescription>
              Please provide a reason for rejecting this leave request.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <div className="py-4">
            <Label htmlFor="rejection-reason" className="text-sm font-medium">
              Rejection Reason
            </Label>
            <Textarea
              id="rejection-reason"
              placeholder="Enter the reason for rejection..."
              value={rejectionReason}
              onChange={(e) => setRejectionReason(e.target.value)}
              className="mt-2"
              rows={4}
            />
          </div>
          <AlertDialogFooter>
            <AlertDialogCancel onClick={() => setRejectionReason('')}>
              Cancel
            </AlertDialogCancel>
            <AlertDialogAction 
              onClick={() => selectedLeave && handleReject(selectedLeave.id, rejectionReason)}
              disabled={!rejectionReason.trim()}
            >
              Reject
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
};

const LeavesContent = () => {
  return (
    <div>
      <LeavesHeader />
      <LeavesTableSection />
      <LeavesDialogs />
    </div>
  );
};

export const Leaves = () => {
  return (
    <LeaveProvider>
      <LeavesContent />
    </LeaveProvider>
  );
};
