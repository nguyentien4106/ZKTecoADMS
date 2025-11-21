import { Button } from '@/components/ui/button';
import { Trash2, CheckCircle, XCircle } from 'lucide-react';
import { LeaveRequest, LeaveStatus } from '@/types/leave';
import { useLeaveContext } from '@/contexts/LeaveContext';
import { useAuth } from '@/contexts/AuthContext';
import { UserRole } from '@/constants/roles';
import { JWT_CLAIMS } from '@/constants/auth';

interface LeaveActionsProps {
  leave: LeaveRequest;
}

export const LeaveActions = ({ leave }: LeaveActionsProps) => {
  const { handleApproveClick, handleRejectClick, handleCancelClick } = useLeaveContext();
  const { user } = useAuth();
  const isManager = user?.[JWT_CLAIMS.ROLE] === UserRole.MANAGER;

  if (leave.status !== LeaveStatus.PENDING) {
    return null;
  }

  return (
    <div className="flex items-center justify-end gap-2">
      {isManager && (
        <>
          <Button
            variant="ghost"
            size="sm"
            onClick={() => handleApproveClick(leave)}
            className="text-green-600 hover:text-green-700 hover:bg-green-50"
            title="Approve"
          >
            <CheckCircle className="h-4 w-4 mr-1" />
            Approve
          </Button>
          <Button
            variant="ghost"
            size="sm"
            onClick={() => handleRejectClick(leave)}
            className="text-red-600 hover:text-red-700 hover:bg-red-50"
            title="Reject"
          >
            <XCircle className="h-4 w-4 mr-1" />
            Reject
          </Button>
        </>
      )}
      {!isManager && (
        <Button
          variant="ghost"
          size="icon"
          onClick={() => handleCancelClick(leave)}
          title="Cancel Request"
        >
          <Trash2 className="h-4 w-4 text-red-500" />
        </Button>
      )}
    </div>
  );
};
