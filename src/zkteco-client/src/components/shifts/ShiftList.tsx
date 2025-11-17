import { Shift, ShiftStatus } from '@/types/shift';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import { CheckCircle, XCircle, Edit, Trash2 } from 'lucide-react';
import { formatDateTime } from '@/lib/utils';

interface ShiftListProps {
    shifts: Shift[];
    isLoading: boolean;
    onApprove?: (shift: Shift) => void;
    onReject?: (shift: Shift) => void;
    showEmployeeInfo?: boolean;
    showActions?: boolean;
    onEdit?: (shift: Shift) => void;
    onDelete?: (id: string) => void;
}

const getStatusBadge = (status: ShiftStatus) => {
    switch (status) {
        case ShiftStatus.Pending:
            return <Badge variant="outline" className="bg-yellow-50 text-yellow-700 border-yellow-200">Pending</Badge>;
        case ShiftStatus.Approved:
            return <Badge variant="outline" className="bg-green-50 text-green-700 border-green-200">Approved</Badge>;
        case ShiftStatus.Rejected:
            return <Badge variant="outline" className="bg-red-50 text-red-700 border-red-200">Rejected</Badge>;
        case ShiftStatus.Cancelled:
            return <Badge variant="outline" className="bg-gray-50 text-gray-700 border-gray-200">Cancelled</Badge>;
        default:
            return <Badge variant="outline">Unknown</Badge>;
    }
};

export const ShiftList = ({
    shifts,
    isLoading,
    onApprove,
    onReject,
    showEmployeeInfo = false,
    showActions = true,
    onEdit,
    onDelete
}: ShiftListProps) => {
        if (isLoading) {
        return <div className="text-center py-8">Loading shifts...</div>;
    }

    if (shifts.length === 0) {
        return <div className="text-center py-8 text-muted-foreground">No shifts found</div>;
    }

    return (
        <div className="rounded-md border">
            <Table>
                <TableHeader>
                    <TableRow>
                        {showEmployeeInfo && (
                            <>
                                <TableHead>Employee</TableHead>
                            </>
                        )}
                        <TableHead>Start Time</TableHead>
                        <TableHead>End Time</TableHead>
                        <TableHead>Total Hours</TableHead>
                        <TableHead>Description</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead>Submitted</TableHead>
                        <TableHead>Updated</TableHead>
                        {showActions && <TableHead className="text-right">Actions</TableHead>}
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {shifts.map((shift) => (
                        <TableRow key={shift.id}>
                            {showEmployeeInfo && (
                                <>
                                    <TableCell className="font-medium">{shift.employeeName}</TableCell>
                                </>
                            )}
                            <TableCell>{formatDateTime(shift.startTime)}</TableCell>
                            <TableCell>{formatDateTime(shift.endTime)}</TableCell>
                            <TableCell>{shift.totalHours}</TableCell>
                            <TableCell>{shift.description || '-'}</TableCell>
                            <TableCell>
                                {getStatusBadge(shift.status)}
                                {shift.rejectionReason && (
                                    <div className="text-xs text-red-600 mt-1">
                                        {shift.rejectionReason}
                                    </div>
                                )}
                            </TableCell>
                            <TableCell className="text-sm text-muted-foreground">
                                {formatDateTime(shift.createdAt)}
                            </TableCell>
                            <TableCell className="text-sm text-muted-foreground">
                                {shift.updatedAt ? formatDateTime(shift.updatedAt) : '-'}
                            </TableCell>
                            {showActions && (
                                <TableCell className="text-right">
                                    <div className="flex justify-end gap-2">
                                        {shift.status === ShiftStatus.Pending && onApprove && onReject && (
                                            <>
                                                <Button
                                                    size="sm"
                                                    variant="outline"
                                                    className="text-green-600 hover:text-green-700"
                                                    onClick={() => onApprove(shift)}
                                                >
                                                    <CheckCircle className="w-4 h-4 mr-1" />
                                                    Approve
                                                </Button>
                                                <Button
                                                    size="sm"
                                                    variant="outline"
                                                    className="text-red-600 hover:text-red-700"
                                                    onClick={() => onReject(shift)}
                                                >
                                                    <XCircle className="w-4 h-4 mr-1" />
                                                    Reject
                                                </Button>
                                            </>
                                        )}
                                        {shift.status === ShiftStatus.Pending && onEdit && (
                                            <Button
                                                size="sm"
                                                variant="outline"
                                                onClick={() => onEdit(shift)}
                                            >
                                                <Edit className="w-4 h-4 mr-1" />
                                                Edit
                                            </Button>
                                        )}
                                        {shift.status === ShiftStatus.Pending && onDelete && (
                                            <Button
                                                size="sm"
                                                variant="destructive"
                                                onClick={() => onDelete(shift.id)}
                                            >
                                                <Trash2 className="w-4 h-4 mr-1" />
                                                Delete
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
