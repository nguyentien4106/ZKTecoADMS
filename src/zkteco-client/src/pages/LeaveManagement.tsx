import { useState } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { useAllLeaves, usePendingLeaves, useApproveLeave, useRejectLeave } from '@/hooks/useLeaves';
import { LeavesTable } from '@/components/leaves/LeavesTable';
import { RejectLeaveDialog } from '@/components/leaves/RejectLeaveDialog';
import { Card } from '@/components/ui/card';
import { CheckCircle, XCircle, Clock } from 'lucide-react';
import { LeaveStatus } from '@/types/leave';

export const LeaveManagement = () => {
  const { data: allLeaves = [], isLoading: allLoading } = useAllLeaves();
  const { data: pendingLeaves = [], isLoading: pendingLoading } = usePendingLeaves();
  const approveLeave = useApproveLeave();
  const rejectLeave = useRejectLeave();
  
  const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
  const [selectedLeaveId, setSelectedLeaveId] = useState<string>('');

  const handleApprove = async (id: string) => {
    if (window.confirm('Are you sure you want to approve this leave request?')) {
      await approveLeave.mutateAsync(id);
    }
  };

  const handleRejectClick = (id: string) => {
    setSelectedLeaveId(id);
    setRejectDialogOpen(true);
  };

  const handleRejectSubmit = async (reason: string) => {
    await rejectLeave.mutateAsync({ id: selectedLeaveId, data: { reason } });
    setRejectDialogOpen(false);
  };

  const approvedLeaves = allLeaves.filter(l => l.status === LeaveStatus.APPROVED);
  const rejectedLeaves = allLeaves.filter(l => l.status === LeaveStatus.REJECTED);

  return (
    <div className="space-y-6">
      <PageHeader
        title="Leave Management"
        description="Review and manage employee leave requests"
      />

      <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
        <Card className="p-4">
          <div className="flex items-center gap-3">
            <div className="p-3 bg-yellow-100 rounded-full">
              <Clock className="h-6 w-6 text-yellow-600" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Pending</p>
              <p className="text-2xl font-bold">{pendingLeaves.length}</p>
            </div>
          </div>
        </Card>
        <Card className="p-4">
          <div className="flex items-center gap-3">
            <div className="p-3 bg-green-100 rounded-full">
              <CheckCircle className="h-6 w-6 text-green-600" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Approved</p>
              <p className="text-2xl font-bold">{approvedLeaves.length}</p>
            </div>
          </div>
        </Card>
        <Card className="p-4">
          <div className="flex items-center gap-3">
            <div className="p-3 bg-red-100 rounded-full">
              <XCircle className="h-6 w-6 text-red-600" />
            </div>
            <div>
              <p className="text-sm text-muted-foreground">Rejected</p>
              <p className="text-2xl font-bold">{rejectedLeaves.length}</p>
            </div>
          </div>
        </Card>
      </div>

      <Tabs defaultValue="pending" className="w-full">
        <TabsList className="grid w-full grid-cols-3">
          <TabsTrigger value="pending">
            Pending ({pendingLeaves.length})
          </TabsTrigger>
          <TabsTrigger value="approved">
            Approved ({approvedLeaves.length})
          </TabsTrigger>
          <TabsTrigger value="rejected">
            Rejected ({rejectedLeaves.length})
          </TabsTrigger>
        </TabsList>
        
        <TabsContent value="pending" className="mt-6">
          <LeavesTable
            leaves={pendingLeaves}
            isLoading={pendingLoading}
            onApprove={handleApprove}
            onReject={handleRejectClick}
            showActions
          />
        </TabsContent>
        
        <TabsContent value="approved" className="mt-6">
          <LeavesTable
            leaves={approvedLeaves}
            isLoading={allLoading}
          />
        </TabsContent>
        
        <TabsContent value="rejected" className="mt-6">
          <LeavesTable
            leaves={rejectedLeaves}
            isLoading={allLoading}
          />
        </TabsContent>
      </Tabs>

      <RejectLeaveDialog
        open={rejectDialogOpen}
        onOpenChange={setRejectDialogOpen}
        onSubmit={handleRejectSubmit}
      />
    </div>
  );
};
