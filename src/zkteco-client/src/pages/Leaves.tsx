import { useState } from 'react';
import { PageHeader } from '@/components/PageHeader';
import { Button } from '@/components/ui/button';
import { Plus } from 'lucide-react';
import { useMyLeaves, useCreateLeave, useCancelLeave } from '@/hooks/useLeaves';
import { LeavesTable } from '@/components/leaves/LeavesTable';
import { LeaveRequestDialog } from '@/components/leaves/LeaveRequestDialog';

export const Leaves = () => {
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const { data: leaves, isLoading } = useMyLeaves();
  const createLeave = useCreateLeave();
  const cancelLeave = useCancelLeave();

  const handleCreateLeave = async (data: any) => {
    await createLeave.mutateAsync(data);
  };

  const handleCancelLeave = async (id: string) => {
    if (window.confirm('Are you sure you want to cancel this leave request?')) {
      await cancelLeave.mutateAsync(id);
    }
  };

  return (
    <div className="space-y-6">
      <PageHeader
        title="My Leaves"
        description="View and manage your leave requests"
        action={
          <Button onClick={() => setIsDialogOpen(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Request Leave
          </Button>
        }
      />

      <LeavesTable 
        leaves={leaves} 
        isLoading={isLoading} 
        onCancel={handleCancelLeave}
      />

      <LeaveRequestDialog
        open={isDialogOpen}
        onOpenChange={setIsDialogOpen}
        onSubmit={handleCreateLeave}
      />
    </div>
  );
};
