import { Shift } from '@/types/shift';
import {
    Table,
    TableBody,
    TableHead,
    TableHeader,
    TableRow,
} from '@/components/ui/table';
import { ShiftRow } from './ShiftRow';
import { LoadingSpinner } from '../LoadingSpinner';

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
        return <div className="text-center py-8">
            <LoadingSpinner />
        </div>;
    }

    if (shifts.length === 0) {
        return <div className="text-center py-8 text-muted-foreground">No shifts found</div>;
    }

    return (
        <div className="rounded-md border">
            <Table>
                <TableHeader>
                    <TableRow>
                        {showEmployeeInfo && <TableHead>Employee</TableHead>}
                        <TableHead>Start Time</TableHead>
                        <TableHead>End Time</TableHead>
                        <TableHead>Total Hours</TableHead>
                        <TableHead>Max Late (min)</TableHead>
                        <TableHead>Max Early Leave (min)</TableHead>
                        <TableHead>Description</TableHead>
                        <TableHead>Status</TableHead>
                        <TableHead>Submitted</TableHead>
                        <TableHead>Updated</TableHead>
                        {showActions && <TableHead className="text-right">Actions</TableHead>}
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {shifts.map((shift) => (
                        <ShiftRow
                            key={shift.id}
                            shift={shift}
                            showEmployeeInfo={showEmployeeInfo}
                            showActions={showActions}
                            onApprove={onApprove}
                            onReject={onReject}
                            onEdit={onEdit}
                            onDelete={onDelete}
                        />
                    ))}
                </TableBody>
            </Table>
        </div>
    );
};
