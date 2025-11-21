import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { format } from 'date-fns';
import { LeaveRequest, LeaveStatus, getLeaveTypeLabel, getLeaveStatusLabel } from '@/types/leave';
import { LoadingSpinner } from '../LoadingSpinner';
import { DateTimeFormat } from '@/constants';
import { Button } from '@/components/ui/button';
import { Trash2, CheckCircle, XCircle } from 'lucide-react';

interface LeavesTableProps {
  leaves: LeaveRequest[] | undefined;
  isLoading: boolean;
  onCancel?: (id: string) => void;
  onApprove?: (id: string) => void;
  onReject?: (id: string) => void;
  showActions?: boolean;
}

const getStatusColor = (status: LeaveStatus) => {
  switch (status) {
    case LeaveStatus.APPROVED:
      return 'bg-green-500';
    case LeaveStatus.REJECTED:
      return 'bg-red-500';
    case LeaveStatus.CANCELLED:
      return 'bg-gray-500';
    default:
      return 'bg-yellow-500';
  }
};

export const LeavesTable = ({ leaves, isLoading, onCancel, onApprove, onReject, showActions }: LeavesTableProps) => {
  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (!leaves || leaves.length === 0) {
    return (
      <div className="text-center py-8 text-muted-foreground">
        No leave requests found.
      </div>
    );
  }

  const hasActions = onCancel || showActions;

  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Employee</TableHead>
            <TableHead>Type</TableHead>
            <TableHead>Start Date</TableHead>
            <TableHead>End Date</TableHead>
            <TableHead>Duration</TableHead>
            <TableHead>Reason</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Created At</TableHead>
            {hasActions && <TableHead className="text-right">Actions</TableHead>}
          </TableRow>
        </TableHeader>
        <TableBody>
          {leaves.map((leave) => (
            <TableRow key={leave.id}>
              <TableCell className="font-medium">{leave.employeeName}</TableCell>
              <TableCell>{getLeaveTypeLabel(leave.type)}</TableCell>
              <TableCell>{format(new Date(leave.startDate), DateTimeFormat)}</TableCell>
              <TableCell>{format(new Date(leave.endDate), DateTimeFormat)}</TableCell>
              <TableCell>
                <Badge variant="outline">
                  {leave.isFullDay ? 'Full Day' : 'Partial'}
                </Badge>
              </TableCell>
              <TableCell className="max-w-[200px] truncate" title={leave.reason}>
                {leave.reason}
              </TableCell>
              <TableCell>
                <Badge className={getStatusColor(leave.status)}>
                  {getLeaveStatusLabel(leave.status)}
                </Badge>
                {leave.status === LeaveStatus.REJECTED && leave.rejectionReason && (
                  <div className="text-xs text-red-500 mt-1">
                    {leave.rejectionReason}
                  </div>
                )}
              </TableCell>
              <TableCell>{format(new Date(leave.createdAt), 'PP')}</TableCell>
              {hasActions && (
                <TableCell className="text-right">
                  <div className="flex items-center justify-end gap-2">
                    {leave.status === LeaveStatus.PENDING && showActions && onApprove && onReject && (
                      <>
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => onApprove(leave.id)}
                          className="text-green-600 hover:text-green-700 hover:bg-green-50"
                          title="Approve"
                        >
                          <CheckCircle className="h-4 w-4 mr-1" />
                          Approve
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => onReject(leave.id)}
                          className="text-red-600 hover:text-red-700 hover:bg-red-50"
                          title="Reject"
                        >
                          <XCircle className="h-4 w-4 mr-1" />
                          Reject
                        </Button>
                      </>
                    )}
                    {leave.status === LeaveStatus.PENDING && onCancel && (
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onCancel(leave.id)}
                        title="Cancel Request"
                      >
                        <Trash2 className="h-4 w-4 text-red-500" />
                      </Button>
                    )}
                  </div>
                </TableCell>
              )}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
};
