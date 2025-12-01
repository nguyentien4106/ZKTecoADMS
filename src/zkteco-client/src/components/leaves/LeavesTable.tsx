import {
  Table,
  TableBody,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { LeaveRequest } from '@/types/leave';
import { LoadingSpinner } from '../LoadingSpinner';
import { LeaveTableRow } from './LeaveTableRow';

interface LeavesTableProps {
  leaves: LeaveRequest[] | undefined;
  isLoading: boolean;
  showActions?: boolean;
}

export const LeavesTable = ({ leaves, isLoading, showActions = true }: LeavesTableProps) => {
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
            {showActions && <TableHead className="text-right">Actions</TableHead>}
          </TableRow>
        </TableHeader>
        <TableBody>
          {leaves.map((leave) => (
            <LeaveTableRow key={leave.id} leave={leave} showActions={showActions} />
          ))}
        </TableBody>
      </Table>
    </div>
  );
};
