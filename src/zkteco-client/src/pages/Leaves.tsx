import { PageHeader } from "@/components/PageHeader";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import { Plus } from "lucide-react";
import { LeavesTable } from '@/components/leaves/LeavesTable';
import { LeaveRequestDialog } from '@/components/leaves/LeaveRequestDialog';
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
import { useEffect, useState } from "react";
import { useAuth } from "@/contexts/AuthContext";

const LeavesHeader = () => {
  const { setCreateDialogOpen } = useLeaveContext();

  return (
    <PageHeader
      title="Leaves Management"
      description="View and manage leave requests"
      action={
        <Button onClick={() => setCreateDialogOpen(true)}>
          <Plus className="mr-2 h-4 w-4" />
          Request Leave
        </Button>
      }
    />
  );
};

const LeavesTabs = () => {
  const {
    pendingLeaves,
    allLeaves,
    isLoading,
  } = useLeaveContext();

  const { isManager } = useAuth();
  const [activeTab, setActiveTab] = useState<string>("all");
  
  useEffect(() => {
    if (pendingLeaves.length > 0 && isManager) {
      setActiveTab("pending");
    }
  }, [pendingLeaves, isManager]);

  return (
    <div className="mt-6">
      <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
        <TabsList>
          {
            isManager && (
              <TabsTrigger value="pending">
                Pending Leaves
                {pendingLeaves.length > 0 && (
                  <Badge className="ml-2 bg-yellow-500 text-white">{pendingLeaves.length}</Badge>
                )}
              </TabsTrigger>
            ) 
          }
          <TabsTrigger value="all">
            All Leaves
          </TabsTrigger>
        </TabsList>

        {
          isManager && (
            <TabsContent value="pending" className="mt-6">
              <LeavesTable
                leaves={pendingLeaves}
                isLoading={isLoading}
                showActions={true}
              />
            </TabsContent>
          )
        }

        <TabsContent value="all" className="mt-6">
          <LeavesTable
            leaves={allLeaves}
            isLoading={isLoading}
            showActions={true}
          />
        </TabsContent>
      </Tabs>
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
      <LeavesTabs />
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
