import { useState } from 'react';
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from '@/components/ui/dialog';
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from '@/components/ui/select';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Button } from '@/components/ui/button';
import { Shift } from '@/types/shift';
import { Calendar, Clock, User } from 'lucide-react';
import { useEmployees } from '@/hooks/useEmployee';
import { HourlyEmployeeId } from '@/constants';
import { useAuth } from '@/contexts/AuthContext';
import { format } from 'date-fns';

/**
 * ShiftExchangeDialog Component
 * 
 * A dialog that allows employees to request a shift exchange with another team member.
 * 
 * @example
 * // In your parent component (e.g., MyShifts.tsx):
 * 
 * import { useEmployees } from '@/hooks/useEmployees';
 * 
 * const MyShifts = () => {
 *   const { data: employees = [] } = useEmployees(); // Fetch all employees
 *   
 *   const handleExchangeRequest = async (shiftId: string, targetEmployeeId: string, reason: string) => {
 *     try {
 *       // Call your API endpoint to create shift exchange request
 *       await shiftExchangeService.createRequest({
 *         shiftId,
 *         targetEmployeeId,
 *         reason
 *       });
 *       toast.success('Shift exchange request sent!');
 *     } catch (error) {
 *       toast.error('Failed to send request');
 *     }
 *   };
 * 
 *   return (
 *     <ShiftTable
 *       // ... other props
 *       onExchangeRequest={handleExchangeRequest}
 *       employees={employees}
 *     />
 *   );
 * };
 */

interface ShiftExchangeDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    shift: Shift;
    onSubmit: (targetEmployeeId: string, reason: string) => void;
    isLoading?: boolean;
}

export const ShiftExchangeDialog = ({
    open,
    onOpenChange,
    shift,
    onSubmit,
    isLoading = false,
}: ShiftExchangeDialogProps) => {
    const [selectedEmployeeId, setSelectedEmployeeId] = useState<string>('');
    const [reason, setReason] = useState<string>('');
    const { data: employees = [] } = useEmployees({ employmentType : HourlyEmployeeId })
    const { employeeId } = useAuth()

    // Filter out current employee from the list
    const availableEmployees = employees.filter(emp => emp.id !== employeeId);

    const handleSubmit = () => {
        if (selectedEmployeeId && reason.trim()) {
            onSubmit(selectedEmployeeId, reason);
            handleClose();
        }
    };

    const handleClose = () => {
        setSelectedEmployeeId('');
        setReason('');
        onOpenChange(false);
    };

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent className="sm:max-w-[500px]">
                <DialogHeader>
                    <DialogTitle>Request Shift Exchange</DialogTitle>
                    <DialogDescription>
                        Request to exchange your shift with another team member. They will need to approve the exchange.
                    </DialogDescription>
                </DialogHeader>

                <div className="space-y-6 py-4">
                    {/* Current Shift Info */}
                    <div className="rounded-lg border p-4 bg-muted/50">
                        <h4 className="text-sm font-semibold mb-3">Your Current Shift</h4>
                        <div className="space-y-2 text-sm">
                            <div className="flex items-center gap-2">
                                <Calendar className="w-4 h-4 text-muted-foreground" />
                                <span className="text-muted-foreground">Date:</span>
                                <span className="font-medium">
                                    {format(new Date(shift.date), 'MMM dd, yyyy')}
                                </span>
                            </div>
                            <div className="flex items-center gap-2">
                                <Clock className="w-4 h-4 text-muted-foreground" />
                                <span className="text-muted-foreground">Time:</span>
                                <span className="font-medium">
                                    {shift.startTime} - {shift.endTime}
                                </span>
                            </div>
                        </div>
                    </div>

                    {/* Select Employee */}
                    <div className="space-y-2">
                        <Label htmlFor="employee-select" className="text-sm font-medium">
                            <div className="flex items-center gap-2">
                                <User className="w-4 h-4" />
                                Exchange With
                            </div>
                        </Label>
                        <Select
                            value={selectedEmployeeId}
                            onValueChange={setSelectedEmployeeId}
                            disabled={isLoading}
                        >
                            <SelectTrigger id="employee-select">
                                <SelectValue placeholder="Select a team member..." />
                            </SelectTrigger>
                            <SelectContent className="max-h-[300px]">
                                {availableEmployees.map((employee) => (
                                    <SelectItem key={employee.id} value={employee.id}>
                                        <div className="flex items-center gap-2">
                                            <span className="font-medium">
                                                {employee.firstName} {employee.lastName}
                                            </span>
                                            <span className="text-xs text-muted-foreground">
                                                ({employee.employeeCode})
                                            </span>
                                        </div>
                                    </SelectItem>
                                ))}
                            </SelectContent>
                        </Select>
                        <p className="text-xs text-muted-foreground">
                            Choose the team member you'd like to exchange shifts with
                        </p>
                    </div>

                    {/* Reason */}
                    <div className="space-y-2">
                        <Label htmlFor="reason" className="text-sm font-medium">
                            Reason for Exchange
                        </Label>
                        <Textarea
                            id="reason"
                            placeholder="Please explain why you need to exchange this shift..."
                            value={reason}
                            onChange={(e) => setReason(e.target.value)}
                            disabled={isLoading}
                            rows={4}
                            className="resize-none"
                        />
                        <p className="text-xs text-muted-foreground">
                            Provide a clear reason to help your team member make a decision
                        </p>
                    </div>
                </div>

                <DialogFooter>
                    <Button
                        variant="outline"
                        onClick={handleClose}
                        disabled={isLoading}
                    >
                        Cancel
                    </Button>
                    <Button
                        onClick={handleSubmit}
                        disabled={!selectedEmployeeId || !reason.trim() || isLoading}
                    >
                        {isLoading ? 'Sending Request...' : 'Send Request'}
                    </Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
};
