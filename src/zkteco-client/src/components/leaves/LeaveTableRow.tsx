import { TableRow, TableCell } from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { format } from 'date-fns';
import { LeaveRequest, getLeaveTypeLabel } from '@/types/leave';
import { formatDateTime } from '@/lib/utils';
import { LeaveStatusBadge } from './LeaveStatusBadge';
import { LeaveActions } from './LeaveActions';

interface LeaveTableRowProps {
  leave: LeaveRequest;
  showActions?: boolean;
}

export const LeaveTableRow = ({ leave, showActions = true }: LeaveTableRowProps) => {
  return (
    <TableRow>
      <TableCell className="font-medium">{leave.employeeName}</TableCell>
      <TableCell>{getLeaveTypeLabel(leave.type)}</TableCell>
      <TableCell>{formatDateTime(leave.startDate)}</TableCell>
      <TableCell>{formatDateTime(leave.endDate)}</TableCell>
      <TableCell>
        <Badge variant="outline">
          {leave.isHalfShift ? 'Half shift' : 'Full shift'}
        </Badge>
      </TableCell>
      <TableCell className="max-w-[200px] truncate" title={leave.reason}>
        {leave.reason}
      </TableCell>
      <TableCell>
        <LeaveStatusBadge status={leave.status} rejectionReason={leave.rejectionReason} />
      </TableCell>
      <TableCell>{format(new Date(leave.createdAt), 'PP')}</TableCell>
      {showActions && (
        <TableCell className="text-right">
          <LeaveActions leave={leave} />
        </TableCell>
      )}
    </TableRow>
  );
};
