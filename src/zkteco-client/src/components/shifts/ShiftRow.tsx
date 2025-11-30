import { Shift } from '@/types/shift';
import { TableCell, TableRow } from '@/components/ui/table';
import { formatDateTime, calculateTotalHours } from '@/lib/utils';
import { StatusBadge } from './StatusBadge';
import { ShiftActions } from './ShiftActions';

interface ShiftRowProps {
    shift: Shift;
    showEmployeeInfo: boolean;
    showActions: boolean;
    onApprove?: (shift: Shift) => void;
    onReject?: (shift: Shift) => void;
    onEdit?: (shift: Shift) => void;
    onDelete?: (id: string) => void;
}

export const ShiftRow = ({
    shift,
    showEmployeeInfo,
    showActions,
    onApprove,
    onReject,
    onEdit,
    onDelete
}: ShiftRowProps) => {
    const totalHours = shift.totalHours || calculateTotalHours(shift.startTime, shift.endTime);

    return (
        <TableRow>
            <TableCell className="whitespace-nowrap">{shift.id.substring(30)}</TableCell>
        
            {showEmployeeInfo && (
                <TableCell className="font-medium whitespace-nowrap">
                    {shift.employeeName}
                </TableCell>
            )}
            <TableCell className="whitespace-nowrap">{formatDateTime(shift.startTime)}</TableCell>
            <TableCell className="whitespace-nowrap">{formatDateTime(shift.endTime)}</TableCell>
            <TableCell className="whitespace-nowrap">{totalHours.toFixed(2)}h</TableCell>
            <TableCell className="text-center">{shift.maximumAllowedLateMinutes}</TableCell>
            <TableCell className="text-center">{shift.maximumAllowedEarlyLeaveMinutes}</TableCell>
            <TableCell>{shift.description || '-'}</TableCell>
            <TableCell>
                <StatusBadge status={shift.status} rejectionReason={shift.rejectionReason} />
            </TableCell>
            <TableCell className="text-sm text-muted-foreground whitespace-nowrap">
                {formatDateTime(shift.createdAt)}
            </TableCell>
            <TableCell className="text-sm text-muted-foreground whitespace-nowrap">
                {shift.updatedAt ? formatDateTime(shift.updatedAt) : '-'}
            </TableCell>
            {showActions && (
                <TableCell className="text-right">
                    <ShiftActions
                        shift={shift}
                        onApprove={onApprove}
                        onReject={onReject}
                        onEdit={onEdit}
                        onDelete={onDelete}
                    />
                </TableCell>
            )}
        </TableRow>
    );
};
