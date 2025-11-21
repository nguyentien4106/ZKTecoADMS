import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { CreateLeaveRequest, LeaveType } from '@/types/leave';
import { format, differenceInHours, isBefore, isAfter } from 'date-fns';
import { useMyShifts } from '@/hooks/useShifts';
import { ShiftStatus } from '@/types/shift';
import { Calendar, Clock, FileText, AlertCircle } from 'lucide-react';
import { Card } from '@/components/ui/card';
import { DateTimePicker } from '@/components/ui/datetime-picker';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { DateTimeFormat } from '@/constants';

interface LeaveRequestDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: CreateLeaveRequest) => Promise<void>;
}

export const LeaveRequestDialog = ({
  open,
  onOpenChange,
  onSubmit
}: LeaveRequestDialogProps) => {
  const { data: shifts = [] } = useMyShifts(ShiftStatus.Approved);
  const [selectedShiftId, setSelectedShiftId] = useState<string>('');
  const [type, setType] = useState<LeaveType>(LeaveType.PERSONAL);
  const [isFullDay, setIsFullDay] = useState(true);
  const [startDate, setStartDate] = useState<Date | undefined>(undefined);
  const [endDate, setEndDate] = useState<Date | undefined>(undefined);
  const [reason, setReason] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Filter approved shifts that are in the future or ongoing
  const approvedShifts = shifts.filter(shift => 
    shift.status === ShiftStatus.Approved && 
    new Date(shift.endTime) >= new Date()
  );

  const selectedShift = approvedShifts.find(s => s.id === selectedShiftId);

  // Auto-fill dates when shift is selected or when toggling full day
  useEffect(() => {
    if (selectedShift) {
      setStartDate(new Date(selectedShift.startTime));
      setEndDate(new Date(selectedShift.endTime));
    }
  }, [selectedShift?.id, isFullDay]);

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

    setIsSubmitting(true);
    try {
      await onSubmit({
        type,
        startDate: format(startDate, DateTimeFormat),
        endDate: format(endDate, DateTimeFormat),
        isFullDay,
        reason
      });
      // Reset form
      setSelectedShiftId('');
      setReason('');
      setType(LeaveType.PERSONAL);
      setIsFullDay(true);
      setStartDate(undefined);
      setEndDate(undefined);
    } catch (error) {
      console.error(error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setSelectedShiftId('');
    setReason('');
    setType(LeaveType.PERSONAL);
    setIsFullDay(true);
    setStartDate(undefined);
    setEndDate(undefined);
    onOpenChange(false);
  };

  const handleFullDayToggle = (checked: boolean) => {
    setIsFullDay(checked);
    if (checked && selectedShift) {
      // Reset to full shift times
      setStartDate(new Date(selectedShift.startTime));
      setEndDate(new Date(selectedShift.endTime));
    }
  };

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="flex items-center gap-2 text-xl">
            <Calendar className="h-5 w-5" />
            Request Leave
          </DialogTitle>
          <DialogDescription>
            Submit a leave request for your approved shift.
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit} className="space-y-6 mt-4">
          {/* Shift Selection */}
          <div className="space-y-3">
            <Label htmlFor="shift" className="text-base font-semibold flex items-center gap-2">
              <Clock className="h-4 w-4" />
              Select Shift
            </Label>
            <Select value={selectedShiftId} onValueChange={setSelectedShiftId}>
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
            <Select value={type.toString()} onValueChange={(v) => setType(parseInt(v) as LeaveType)}>
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

          {/* Full Day Toggle */}
          <div className="space-y-3">
            <Label className="text-base font-semibold">Leave Duration</Label>
            <div className="flex items-start space-x-3 p-4 rounded-lg border-2 bg-card hover:bg-accent/50 transition-colors">
              <input
                type="checkbox"
                id="isFullDay"
                className="h-5 w-5 rounded border-gray-300 text-primary focus:ring-2 focus:ring-primary cursor-pointer mt-0.5"
                checked={isFullDay}
                onChange={(e) => handleFullDayToggle(e.target.checked)}
              />
              <Label htmlFor="isFullDay" className="cursor-pointer flex-1">
                <span className="font-medium">Full Shift Leave</span>
                <span className="block text-sm text-muted-foreground font-normal mt-1">
                  Uncheck to specify a partial leave period
                </span>
              </Label>
            </div>
          </div>

          {/* Partial Leave Date/Time Pickers */}
          {!isFullDay && selectedShift && (
            <div className="space-y-4 p-5 border-2 rounded-lg bg-gradient-to-br from-accent/40 to-accent/20">
              <div className="flex items-center gap-2 font-semibold text-lg">
                <Clock className="h-5 w-5 text-primary" />
                Specify Partial Leave Period
              </div>
              
              <div className="space-y-4">
                {/* Leave Start Time */}
                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <Label className="font-semibold text-base">Leave Start Time</Label>
                    <span className="text-xs text-muted-foreground bg-background px-2 py-1 rounded">
                      Shift: {format(new Date(selectedShift.startTime), 'h:mm a')}
                    </span>
                  </div>
                  <DateTimePicker 
                    date={startDate} 
                    setDate={setStartDate}
                    timeOnly={true}
                  />
                </div>

                {/* Leave End Time */}
                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <Label className="font-semibold text-base">Leave End Time</Label>
                    <span className="text-xs text-muted-foreground bg-background px-2 py-1 rounded">
                      Shift: {format(new Date(selectedShift.endTime), 'h:mm a')}
                    </span>
                  </div>
                  <DateTimePicker 
                    date={endDate} 
                    setDate={setEndDate}
                    timeOnly={true}
                  />
                </div>
              </div>

              {/* Helper text */}
              <div className="flex items-start gap-2 p-3 bg-background/80 rounded-md border">
                <AlertCircle className="h-4 w-4 text-muted-foreground mt-0.5 flex-shrink-0" />
                <p className="text-xs text-muted-foreground">
                  Select the exact start and end times for your partial leave. Both times must fall within your shift period on {format(new Date(selectedShift.startTime), 'MMMM dd, yyyy')}.
                </p>
              </div>
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
                {!isFullDay && selectedShift && (
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
              onChange={(e) => setReason(e.target.value)}
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
              disabled={isSubmitting || !selectedShiftId || !reason.trim() || !validation.valid}
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
