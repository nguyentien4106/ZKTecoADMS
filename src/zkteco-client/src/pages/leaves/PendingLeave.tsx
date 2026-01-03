import { PageHeader } from "@/components/PageHeader";
import { Badge } from "@/components/ui/badge";
import { LeavesTable } from '@/components/leaves/LeavesTable';
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

const PendingLeaveContent = () => {
  const {
    pendingPaginationRequest,
    paginatedPendingLeaves,
    isLoading,
    setPendingPaginationRequest,
    approveDialogOpen,
    rejectDialogOpen,
    selectedLeave,
    rejectionReason,
    setApproveDialogOpen,
    setRejectDialogOpen,
    setRejectionReason,
    handleApprove,
    handleReject,
  } = useLeaveContext();

  const onPendingPaginationChange = useCallback((pageNumber: number, pageSize: number) => {
    setPendingPaginationRequest(prev => ({
      ...prev,
      pageNumber: pageNumber,
      pageSize: pageSize
    }));
  }, [setPendingPaginationRequest]);

  const onPendingSortingChange = useCallback((sorting: any) => {
    setPendingPaginationRequest(prev => ({
      ...prev,
      sortBy: sorting.length > 0 ? sorting[0].id : undefined,
      sortOrder: sorting.length > 0 ? (sorting[0].desc ? 'desc' : 'asc') : undefined,
      pageNumber: 1,
    }));
  }, [setPendingPaginationRequest]);

  return (
    <div className="space-y-6">
      <PageHeader
        title="Pending Leaves"
        description="Review and approve pending leave requests"
        action={
          (paginatedPendingLeaves?.totalCount ?? 0) > 0 && (
            <Badge variant="destructive" className="text-lg px-4 py-2">
              {paginatedPendingLeaves?.totalCount} Pending
            </Badge>
          )
        }
      />

      {paginatedPendingLeaves && (
        <LeavesTable
          paginatedLeaves={paginatedPendingLeaves}
          isLoading={isLoading}
          showActions={true}
          onPaginationChange={onPendingPaginationChange}
          paginationRequest={pendingPaginationRequest}
          onSortingChange={onPendingSortingChange}
        />
      )}

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
    </div>
  );
};

export const PendingLeave = () => {
  return (
    <LeaveProvider>
      <PendingLeaveContent />
    </LeaveProvider>
  );
};

export default PendingLeave;
