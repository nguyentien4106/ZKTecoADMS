import { useState } from 'react';
import { ExchangeShiftRequest, Shift, ShiftStatus } from '@/types/shift';
import { Button } from '@/components/ui/button';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { CheckCircle, XCircle, Trash2, Pencil, ArrowLeftRight } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import { ShiftExchangeDialog } from '@/components/dialogs/ShiftExchangeDialog';

interface ShiftActionsProps {
    shift: Shift;
    onApprove?: (shift: Shift) => void;
    onReject?: (shift: Shift) => void;
    onEdit?: (shift: Shift) => void;
    onDelete?: (id: string) => void;
    onExchangeRequest?: (data: ExchangeShiftRequest) => void;
}

export const ShiftActions = ({
    shift,
    onApprove,
    onReject,
    onDelete,
    onEdit,
    onExchangeRequest,
}: ShiftActionsProps) => {
    const isPending = shift.status === ShiftStatus.Pending;
    const isApproved = shift.status === ShiftStatus.Approved;
    const [exchangeDialogOpen, setExchangeDialogOpen] = useState(false);

    const { isManager, isHourlyEmployee } = useAuth();

    const handleExchangeSubmit = (targetEmployeeId: string, reason: string) => {
        if (onExchangeRequest) {
            onExchangeRequest({ shiftId: shift.id, targetEmployeeId, reason });
        }
    };
    
    return (
        <>
            <div className="flex justify-end gap-2">
                {/* Edit button - show for all shifts if onEdit callback is provided */}
                {isManager && shift.status === ShiftStatus.Approved && onEdit && (
                    <Button
                        size="sm"
                        variant="outline"
                        onClick={() => onEdit(shift)}
                    >
                        <Pencil className="w-4 h-4 mr-1" />
                        Edit Times
                    </Button>
                )}

                {/* Exchange button - show for approved shifts */}
                {isApproved && isHourlyEmployee && onExchangeRequest && (
                    <Button
                        size="sm"
                        variant="outline"
                        onClick={() => setExchangeDialogOpen(true)}
                    >
                        <ArrowLeftRight className="w-4 h-4 mr-1" />
                        Exchange Shift
                    </Button>
                )}
                
                {isPending && onApprove && onReject && (
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
                {isPending && onDelete && (
                    <AlertDialog>
                        <AlertDialogTrigger asChild>
                            <Button
                                size="sm"
                                variant="destructive"
                            >
                                <Trash2 className="w-4 h-4 mr-1" />
                                Delete
                            </Button>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                            <AlertDialogHeader>
                                <AlertDialogTitle>Delete Shift</AlertDialogTitle>
                                <AlertDialogDescription>
                                    Are you sure you want to delete this shift? This action cannot be undone.
                                </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                                <AlertDialogCancel>Cancel</AlertDialogCancel>
                                <AlertDialogAction
                                    onClick={() => onDelete(shift.id)}
                                    className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                                >
                                    Delete
                                </AlertDialogAction>
                            </AlertDialogFooter>
                        </AlertDialogContent>
                    </AlertDialog>
                )}
            </div>

            {/* Shift Exchange Dialog */}
            {onExchangeRequest && (
                <ShiftExchangeDialog
                    open={exchangeDialogOpen}
                    onOpenChange={setExchangeDialogOpen}
                    shift={shift}
                    onSubmit={handleExchangeSubmit}
                />
            )}
        </>
    );
};
