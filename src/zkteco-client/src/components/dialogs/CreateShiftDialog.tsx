import { useState } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { DateTimePicker } from '@/components/ui/datetime-picker';
import { useShiftContext } from '@/contexts/ShiftContext';

const getDefaultNewShift = () => {
    const now = new Date();
    const tomorrow = new Date(now);
    tomorrow.setDate(tomorrow.getDate() + 1);
    
    return {
        startTime: now,
        endTime: tomorrow,
        description: '',
    };
};

export const CreateShiftDialog = () => {
    const {
        createDialogOpen,
        setCreateDialogOpen,
        handleCreate,
    } = useShiftContext();

    const [newShift, setNewShift] = useState(getDefaultNewShift());

    const now = new Date();
    const maxEndTime = new Date();
    maxEndTime.setDate(maxEndTime.getDate() + 1);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!newShift.startTime || !newShift.endTime) {
            return;
        }
        console.log('Submitting shift request:', {
            startTime: newShift.startTime.toISOString(),
            endTime: newShift.endTime.toISOString(),
            description: newShift.description,
        });
        handleCreate({
            startTime: newShift.startTime.toISOString(),
            endTime: newShift.endTime.toISOString(),
            description: newShift.description,
        });
        setNewShift(getDefaultNewShift());
    };

    const handleOpenChange = (open: boolean) => {
        setCreateDialogOpen(open);
        if (!open) {
            setNewShift(getDefaultNewShift());
        }
    };

    return (
        <Dialog open={createDialogOpen} onOpenChange={handleOpenChange}>
            <DialogContent>
                <form onSubmit={handleSubmit}>
                    <DialogHeader>
                        <DialogTitle>Request New Shift</DialogTitle>
                        <DialogDescription>
                            Submit a shift request for manager approval
                        </DialogDescription>
                    </DialogHeader>

                    <div className="space-y-4 py-4">
                        <div className="space-y-2">
                            <Label htmlFor="startTime">Start Time</Label>
                            <DateTimePicker
                                date={newShift.startTime}
                                setDate={(date) => setNewShift({ ...newShift, startTime: date || now })}
                                minDate={now}
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="endTime">End Time</Label>
                            <DateTimePicker
                                date={newShift.endTime}
                                setDate={(date) => setNewShift({ ...newShift, endTime: date || maxEndTime })}
                                minDate={newShift.startTime || now}
                                maxDate={maxEndTime}
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="description">Description (Optional)</Label>
                            <Textarea
                                id="description"
                                value={newShift.description}
                                onChange={(e) => setNewShift({ ...newShift, description: e.target.value })}
                                placeholder="Add any notes about this shift..."
                                rows={3}
                            />
                        </div>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={() => handleOpenChange(false)}>
                            Cancel
                        </Button>
                        <Button type="submit">Submit Request</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
