import { useState, useEffect } from 'react';
import { defaultLeaveDialogState, LeaveDialogState } from '@/constants/defaultValue';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { LeaveType } from '@/types/leave';
import { format, differenceInHours, isBefore, isAfter } from 'date-fns';
import { useMyShifts } from '@/hooks/useShifts';
import { ShiftStatus } from '@/types/shift';
import { Calendar, Clock, FileText, AlertCircle, UserPlus } from 'lucide-react';
import { Card } from '@/components/ui/card';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { DateTimeFormat } from '@/constants';
import { useLeaveContext } from '@/contexts/LeaveContext';
import { useAuth } from '@/contexts/AuthContext';
import { useEmployeesByManager } from '@/hooks/useAccount';

export const LeaveRequestDialog = () => {
  const { createDialogOpen, setCreateDialogOpen, handleCreate } = useLeaveContext();
  const { isManager } = useAuth();
  const { data: employees = [] } = useEmployeesByManager(isManager);
  const { data: shifts = [] } = useMyShifts(ShiftStatus.Approved);
  const [dialogState, setDialogState] = useState<LeaveDialogState>({ ...defaultLeaveDialogState });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const { selectedEmployeeId, selectedShiftId, type, isHalfShift, halfShiftType, startDate, endDate, reason } = dialogState;

  // Filter approved shifts that are in the future or ongoing
  const approvedShifts = shifts.filter(shift => 
    shift.status === ShiftStatus.Approved && 
    new Date(shift.endTime) >= new Date()
  );

  const selectedShift = approvedShifts.find(s => s.id === selectedShiftId);

  // Auto-fill dates when shift is selected or when toggling half shift
  useEffect(() => {
    if (selectedShift) {
      if (isHalfShift) {
        const shiftStart = new Date(selectedShift.startTime);
        const shiftEnd = new Date(selectedShift.endTime);
        const midTime = new Date(shiftStart.getTime() + (shiftEnd.getTime() - shiftStart.getTime()) / 2);
        if (halfShiftType === 'first') {
          setDialogState((prev: LeaveDialogState) => ({ ...prev, startDate: shiftStart, endDate: midTime }));
        } else if (halfShiftType === 'second') {
          setDialogState((prev: LeaveDialogState) => ({ ...prev, startDate: midTime, endDate: shiftEnd }));
        } else {
          setDialogState((prev: LeaveDialogState) => ({ ...prev, startDate: undefined, endDate: undefined }));
        }
      } else {
        setDialogState((prev: LeaveDialogState) => ({ ...prev, startDate: new Date(selectedShift.startTime), endDate: new Date(selectedShift.endTime) }));
      }
    }
  }, [selectedShift?.id, isHalfShift, halfShiftType]);
  
  // Validation logic
  const validateDates = () => {
    if (!startDate || !endDate || !selectedShift) {
      return { valid: false, message: '' };
    }
    
    const shiftStart = new Date(selectedShift.startTime);
    const shiftEnd = new Date(selectedShift.endTime);

    // Check if dates are within shift range
    if (isBefore(startDate, shiftStart)) {
      return { 
        valid: false, 
        message: 'Leave start time cannot be before shift start time' 
      };
    }

    if (isAfter(endDate, shiftEnd)) {
      return { 
        valid: false, 
        message: 'Leave end time cannot be after shift end time' 
      };
    }

    if (isAfter(startDate, endDate)) {
      return { 
        valid: false, 
        message: 'Leave end time must be after start time' 
      };
    }

    return { valid: true, message: '' };
  };

  const validation = validateDates();
  const duration = startDate && endDate ? differenceInHours(endDate, startDate) : 0;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedShift || !validation.valid || !startDate || !endDate) return;
    if (isManager && !selectedEmployeeId) return;

    setIsSubmitting(true);
    try {
      await handleCreate({
        type,
        isHalfShift,
        reason,
        employeeUserId: isManager ? selectedEmployeeId : null,
        shiftId: selectedShift.id,
        startDate: format(startDate, DateTimeFormat),
        endDate: format(endDate, DateTimeFormat),
      });
      handleClose();
    } catch (error) {
      console.error(error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setDialogState({ ...defaultLeaveDialogState });
    setCreateDialogOpen(false);
  };

  const handleHalfShiftToggle = (checked: boolean) => {
    setDialogState((prev) => ({
      ...prev,
      isHalfShift: checked,
      halfShiftType: '',
    }));
    if (!checked && selectedShift) {
      setDialogState((prev) => ({
        ...prev,
        startDate: new Date(selectedShift.startTime),
        endDate: new Date(selectedShift.endTime),
      }));
    }
  };

  return (
    <Dialog open={createDialogOpen} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2 text-xl">
            <Calendar className="h-5 w-5" />
            Request Leave
          </DialogTitle>
          <DialogDescription>
            {isManager 
              ? "Create a leave request for an employee." 
              : "Submit a leave request for your approved shift."}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-6 mt-4">
          {/* Employee Selection (Manager only) */}
          {isManager && (
            <div className="space-y-3">
              <Label htmlFor="employee" className="text-base font-semibold flex items-center gap-2">
                <UserPlus className="h-4 w-4" />
                Select Employee
              </Label>
              <Select value={selectedEmployeeId} onValueChange={v => setDialogState(prev => ({ ...prev, selectedEmployeeId: v }))}>
                <SelectTrigger id="employee">
                  <SelectValue placeholder="Choose an employee" />
                </SelectTrigger>
                <SelectContent>
                  {employees.length === 0 ? (
                    <div className="p-4 text-sm text-muted-foreground text-center">
                      No employees available
                    </div>
                  ) : (
                    employees.map((employee: any) => (
                      <SelectItem key={employee.id} value={employee.id}>
                        <div className="flex flex-col gap-1">
                          <span className="font-medium">{employee.fullName || employee.email}</span>
                          <span className="text-xs text-muted-foreground">{employee.email}</span>
                        </div>
                      </SelectItem>
                    ))
                  )}
                </SelectContent>
              </Select>
            </div>
          )}

          {/* Shift Selection */}
          <div className="space-y-3">
            <Label htmlFor="shift" className="text-base font-semibold flex items-center gap-2">
              <Clock className="h-4 w-4" />
              Select Shift
            </Label>
            <Select value={selectedShiftId} onValueChange={v => setDialogState(prev => ({ ...prev, selectedShiftId: v }))}>
              <SelectTrigger id="shift" className="h-auto">
                <SelectValue placeholder="Choose a shift" />
              </SelectTrigger>
              <SelectContent>
                {approvedShifts.length === 0 ? (
                  <div className="p-4 text-sm text-muted-foreground text-center">
                    No approved shifts available
                  </div>
                ) : (
                  approvedShifts.map((shift) => (
                    <SelectItem key={shift.id} value={shift.id} className="py-3">
                      <div className="flex flex-col gap-1">
                        <span className="font-medium">
                          {format(new Date(shift.startTime), 'MMM dd, yyyy, h:mm a')} - {format(new Date(shift.endTime), 'h:mm a')}
                        </span>
                        <span className="text-xs text-muted-foreground">
                          {shift.totalHours}h{shift.description && ` • ${shift.description}`}
                        </span>
                      </div>
                    </SelectItem>
                  ))
                )}
              </SelectContent>
            </Select>
          </div>

          {selectedShift && (
            <Card className="p-4 bg-accent/50 border-2">
              <div className="grid grid-cols-3 gap-4 text-sm">
                <div>
                  <p className="text-muted-foreground mb-1">Shift Start</p>
                  <p className="font-semibold">{format(new Date(selectedShift.startTime), 'MMM dd, h:mm a')}</p>
                </div>
                <div>
                  <p className="text-muted-foreground mb-1">Shift End</p>
                  <p className="font-semibold">{format(new Date(selectedShift.endTime), 'MMM dd, h:mm a')}</p>
                </div>
                <div>
                  <p className="text-muted-foreground mb-1">Duration</p>
                  <p className="font-semibold">{selectedShift.totalHours} hours</p>
                </div>
              </div>
            </Card>
          )}

          {/* Leave Type */}
          <div className="space-y-3">
            <Label htmlFor="type" className="text-base font-semibold flex items-center gap-2">
              <FileText className="h-4 w-4" />
              Leave Type
            </Label>
            <Select value={type.toString()} onValueChange={v => setDialogState(prev => ({ ...prev, type: parseInt(v) as LeaveType }))}>
              <SelectTrigger id="type">
                <SelectValue placeholder="Select leave type" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value={LeaveType.SICK.toString()}>Sick</SelectItem>
                <SelectItem value={LeaveType.VACATION.toString()}>Vacation</SelectItem>
                <SelectItem value={LeaveType.PERSONAL.toString()}>Personal</SelectItem>
                <SelectItem value={LeaveType.OTHER.toString()}>Other</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Half Shift Toggle */}
          <div className="space-y-3">
            <Label className="text-base font-semibold">Leave Duration</Label>
            <div className="flex items-start space-x-3 p-4 rounded-lg border-2 bg-card hover:bg-accent/50 transition-colors">
              <input
                type="checkbox"
                id="isHalfShift"
                className="h-5 w-5 rounded border-gray-300 text-primary focus:ring-2 focus:ring-primary cursor-pointer mt-0.5"
                checked={isHalfShift}
                onChange={e => handleHalfShiftToggle(e.target.checked)}
              />
              <Label htmlFor="isHalfShift" className="cursor-pointer flex-1">
                <span className="font-medium">Is Half Shift</span>
                <span className="block text-sm text-muted-foreground font-normal mt-1">
                  Check to request leave for half of your shift
                </span>
              </Label>
            </div>
          </div>

          {/* Half Shift Type Select */}
          {isHalfShift && selectedShift && (
            <div className="space-y-3">
              <Label htmlFor="halfShiftType" className="text-base font-semibold flex items-center gap-2">
                <Clock className="h-4 w-4" />
                Select Half Shift
              </Label>
              <Select value={halfShiftType} onValueChange={v => setDialogState(prev => ({ ...prev, halfShiftType: v as 'first' | 'second' }))}>
                <SelectTrigger id="halfShiftType">
                  <SelectValue placeholder="Choose half shift" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="first">1st Half</SelectItem>
                  <SelectItem value="second">2nd Half</SelectItem>
                </SelectContent>
              </Select>
            </div>
          )}

          {/* Duration Display */}
          {duration > 0 && (
            <Card className="p-4 bg-primary/5 border-primary/30">
              <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-muted-foreground mb-1">Leave Duration</p>
                  <p className="text-2xl font-bold text-primary">
                    {duration} hour{duration !== 1 ? 's' : ''}
                  </p>
                </div>
                {!isHalfShift && selectedShift && (
                  <div className="text-right">
                    <p className="text-sm text-muted-foreground mb-1">Percentage</p>
                    <p className="text-2xl font-bold">
                      {((duration / selectedShift.totalHours) * 100).toFixed(0)}%
                    </p>
                  </div>
                )}
              </div>
            </Card>
          )}

          {/* Validation Error */}
          {!validation.valid && validation.message && (
            <Alert variant="destructive" className="border-2">
              <AlertCircle className="h-5 w-5" />
              <AlertDescription className="font-medium">{validation.message}</AlertDescription>
            </Alert>
          )}

          {/* Reason */}
          <div className="space-y-3">
            <Label htmlFor="reason" className="text-base font-semibold">
              Reason for Leave <span className="text-destructive">*</span>
            </Label>
            <Textarea
              id="reason"
              value={reason}
              onChange={e => setDialogState(prev => ({ ...prev, reason: e.target.value }))}
              placeholder="Please provide a detailed reason for your leave request..."
              className="min-h-[120px] resize-none"
              required
            />
            <p className="text-xs text-muted-foreground">
              Required field - Please explain why you need this leave
            </p>
          </div>

          <DialogFooter className="gap-3 pt-4">
            <Button type="button" variant="outline" onClick={handleClose} className="min-w-[100px]">
              Cancel
            </Button>
            <Button 
              type="submit" 
              disabled={
                isSubmitting || 
                !selectedShiftId || 
                !reason.trim() || 
                !validation.valid ||
                (isManager && !selectedEmployeeId)
              }
              className="min-w-[140px]"
            >
              {isSubmitting ? 'Submitting...' : 'Submit Request'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
};
