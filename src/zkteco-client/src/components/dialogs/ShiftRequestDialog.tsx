import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { useShiftTemplates } from '@/hooks/useShiftTemplate';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { format } from 'date-fns';
import { CalendarIcon, Clock } from 'lucide-react';
import { DateTimeFormat } from '@/constants';
import { CreateShiftRequest, UpdateShiftRequest } from '@/types/shift';
import { useShiftContext } from '@/contexts/ShiftContext';

interface ShiftDialogState {
    templateId: string;
    date: Date;
    description: string;
}

const defaultDialogState: ShiftDialogState = {
    templateId: '',
    date: new Date(),
    description: '',
};

const getDateString = (time: string, dateStr: Date) => {
    const [hours, minutes] = time.split(':');
    const date = new Date(dateStr);
    date.setHours(parseInt(hours), parseInt(minutes), 0, 0);
    return format(date, DateTimeFormat);
};

export const ShiftRequestDialog = () => {
    const [dialogState, setDialogState] = useState<ShiftDialogState>({ ...defaultDialogState });
    const [isSubmitting, setIsSubmitting] = useState(false);
    
    const {
        dialogMode,
        setDialogMode,
        selectedShift,
        handleCreateOrUpdate,
    } = useShiftContext();

    const { data: templates = [], isLoading: templatesLoading } = useShiftTemplates();
    const hasTemplates = templates.length > 0;

    const isEditMode = dialogMode === 'edit' && selectedShift !== null;

    const { templateId, date, description } = dialogState;

    // Get selected template
    const selectedTemplate = templates.find(t => t.id === templateId);

    // Handler to close dialog
    const handleClose = () => {
        setDialogMode(null);
        setDialogState({ ...defaultDialogState });
    };

    // Initialize dialog state when editing
    useEffect(() => {
        if (isEditMode && selectedShift) {
            // In edit mode, we don't use template, just show the shift details
            setDialogState({
                templateId: '',
                date: new Date(selectedShift.startTime),
                description: selectedShift.description || '',
            });
        } else if (!isEditMode) {
            setDialogState({ ...defaultDialogState });
        }
    }, [isEditMode, selectedShift]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (isEditMode && selectedShift) {
            // Update mode - just update description for now
            const updateData: UpdateShiftRequest = {
                startTime: selectedShift.startTime,
                endTime: selectedShift.endTime,
                maximumAllowedLateMinutes: selectedShift.maximumAllowedLateMinutes,
                maximumAllowedEarlyLeaveMinutes: selectedShift.maximumAllowedEarlyLeaveMinutes,
                description: description,
            };

            setIsSubmitting(true);
            try {
                await handleCreateOrUpdate(updateData, selectedShift.id);
                handleClose();
            } catch (error) {
                console.error('Failed to update shift:', error);
            } finally {
                setIsSubmitting(false);
            }
        } else {
            // Create mode - use template
            if (!templateId || !date || !selectedTemplate) return;

            const createData: CreateShiftRequest = {
                startTime: getDateString(selectedTemplate.startTime, date),
                endTime: getDateString(selectedTemplate.endTime, date),
                maximumAllowedLateMinutes: selectedTemplate.maximumAllowedLateMinutes,
                maximumAllowedEarlyLeaveMinutes: selectedTemplate.maximumAllowedEarlyLeaveMinutes,
                description: description,
            };

            setIsSubmitting(true);
            try {
                await handleCreateOrUpdate(createData);
                handleClose();
            } catch (error) {
                console.error('Failed to create shift:', error);
            } finally {
                setIsSubmitting(false);
            }
        }
    };

    const updateDialogState = (updates: Partial<ShiftDialogState>) => {
        setDialogState((prev) => ({ ...prev, ...updates }));
    };

    return (
        <Dialog open={dialogMode !== null} onOpenChange={handleClose}>
            <DialogContent className="sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
                <DialogHeader>
                    <DialogTitle className="flex items-center gap-2 text-xl">
                        <Clock className="h-5 w-5" />
                        {isEditMode ? 'Edit Shift Request' : 'Request New Shift'}
                    </DialogTitle>
                    <DialogDescription>
                        {isEditMode 
                            ? "Update your shift request details."
                            : "Submit a shift request using a template."}
                    </DialogDescription>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-6 mt-4">
                    {!isEditMode && (
                        <>
                            {/* Template Selection */}
                            <div className="space-y-2">
                                <Label htmlFor="template">Select Shift Template</Label>
                                {templatesLoading ? (
                                    <div className="text-sm text-muted-foreground">Loading templates...</div>
                                ) : !hasTemplates ? (
                                    <div className="text-sm text-muted-foreground">
                                        No templates available. Please contact your manager to create templates.
                                    </div>
                                ) : (
                                    <Select
                                        value={templateId}
                                        onValueChange={(value) => updateDialogState({ templateId: value })}
                                    >
                                        <SelectTrigger>
                                            <SelectValue placeholder="Choose a template..." />
                                        </SelectTrigger>
                                        <SelectContent>
                                            {templates.map((template) => (
                                                <SelectItem key={template.id} value={template.id}>
                                                    {template.name} ({template.startTime.substring(0, 5)} - {template.endTime.substring(0, 5)})
                                                </SelectItem>
                                            ))}
                                        </SelectContent>
                                    </Select>
                                )}
                            </div>

                            {/* Date Selection */}
                            <div className="space-y-2">
                                <Label htmlFor="date">Shift Date</Label>
                                <Popover>
                                    <PopoverTrigger asChild>
                                        <Button
                                            variant="outline"
                                            className={cn(
                                                "w-full justify-start text-left font-normal",
                                                !date && "text-muted-foreground"
                                            )}
                                        >
                                            <CalendarIcon className="mr-2 h-4 w-4" />
                                            {date ? format(date, "PPP") : <span>Pick a date</span>}
                                        </Button>
                                    </PopoverTrigger>
                                    <PopoverContent className="w-auto p-0">
                                        <Calendar
                                            mode="single"
                                            style={{ minWidth: "250px" }}
                                            selected={date}
                                            onSelect={(selectedDate) => updateDialogState({ date: selectedDate || new Date() })}
                                            disabled={(dateToCheck) => dateToCheck < new Date(new Date().setHours(0, 0, 0, 0))}
                                            initialFocus
                                        />
                                    </PopoverContent>
                                </Popover>
                            </div>

                            {/* Template Details Display */}
                            {selectedTemplate && (
                                <div className="rounded-lg border p-4 space-y-2 bg-muted/50">
                                    <div className="font-medium text-sm">Template Details</div>
                                    <div className="grid grid-cols-2 gap-2 text-sm">
                                        <div>
                                            <span className="text-muted-foreground">Start:</span>{' '}
                                            {selectedTemplate.startTime.substring(0, 5)}
                                        </div>
                                        <div>
                                            <span className="text-muted-foreground">End:</span>{' '}
                                            {selectedTemplate.endTime.substring(0, 5)}
                                        </div>
                                        <div>
                                            <span className="text-muted-foreground">Max Late:</span>{' '}
                                            {selectedTemplate.maximumAllowedLateMinutes} min
                                        </div>
                                        <div>
                                            <span className="text-muted-foreground">Max Early:</span>{' '}
                                            {selectedTemplate.maximumAllowedEarlyLeaveMinutes} min
                                        </div>
                                        <div className="col-span-2">
                                            <span className="text-muted-foreground">Total Hours:</span>{' '}
                                            {selectedTemplate.totalHours}h
                                        </div>
                                    </div>
                                </div>
                            )}
                        </>
                    )}

                    {/* Show shift details in edit mode */}
                    {isEditMode && selectedShift && (
                        <div className="rounded-lg border p-4 space-y-2 bg-muted/50">
                            <div className="font-medium text-sm">Shift Details</div>
                            <div className="grid grid-cols-2 gap-2 text-sm">
                                <div>
                                    <span className="text-muted-foreground">Start:</span>{' '}
                                    {format(new Date(selectedShift.startTime), 'PPp')}
                                </div>
                                <div>
                                    <span className="text-muted-foreground">End:</span>{' '}
                                    {format(new Date(selectedShift.endTime), 'PPp')}
                                </div>
                                <div>
                                    <span className="text-muted-foreground">Max Late:</span>{' '}
                                    {selectedShift.maximumAllowedLateMinutes} min
                                </div>
                                <div>
                                    <span className="text-muted-foreground">Max Early:</span>{' '}
                                    {selectedShift.maximumAllowedEarlyLeaveMinutes} min
                                </div>
                                <div className="col-span-2">
                                    <span className="text-muted-foreground">Total Hours:</span>{' '}
                                    {selectedShift.totalHours}h
                                </div>
                            </div>
                        </div>
                    )}

                    {/* Description */}
                    <div className="space-y-2">
                        <Label htmlFor="description">Description (Optional)</Label>
                        <Textarea
                            id="description"
                            value={description}
                            onChange={(e) => updateDialogState({ description: e.target.value })}
                            placeholder="Add any notes about this shift..."
                            rows={3}
                            className="resize-none"
                        />
                    </div>

                    <DialogFooter className="gap-3 pt-4">
                        <Button type="button" variant="outline" onClick={handleClose} className="min-w-[100px]">
                            Cancel
                        </Button>
                        <Button 
                            type="submit" 
                            disabled={
                                isSubmitting || 
                                (!isEditMode && (!templateId || !hasTemplates))
                            }
                            className="min-w-[140px]"
                        >
                            {isSubmitting ? 'Submitting...' : isEditMode ? 'Update Shift' : 'Submit Request'}
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
