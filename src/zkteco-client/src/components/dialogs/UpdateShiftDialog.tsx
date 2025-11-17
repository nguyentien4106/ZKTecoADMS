import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import { Shift } from '@/types/shift';
import { useShiftContext } from '@/contexts/ShiftContext';

interface UpdateShiftDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    shift: Shift;
    onSubmit: (data: { startTime: string; endTime: string; description?: string }) => void;
}

export const UpdateShiftDialog = () => {
    const {
        selectedShift,
        updateDialogOpen,
        setUpdateDialogOpen,
        handleUpdate
    } = useShiftContext()
    const [startTime, setStartTime] = useState('');
    const [endTime, setEndTime] = useState('');
    const [description, setDescription] = useState('');

    useEffect(() => {
        if (selectedShift) {
            // Convert ISO string to datetime-local format
            const formatForInput = (dateString: string) => {
                const date = new Date(dateString);
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                const hours = String(date.getHours()).padStart(2, '0');
                const minutes = String(date.getMinutes()).padStart(2, '0');
                return `${year}-${month}-${day}T${hours}:${minutes}`;
            };

            setStartTime(formatForInput(selectedShift.startTime));
            setEndTime(formatForInput(selectedShift.endTime));
            setDescription(selectedShift.description || '');
        }
    }, [selectedShift]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        handleUpdate(selectedShift?.id ?? "", {
            startTime,
            endTime,
            description
        });
    };

    return (
        <Dialog open={updateDialogOpen} onOpenChange={setUpdateDialogOpen}>
            <DialogContent>
                <form onSubmit={handleSubmit}>
                    <DialogHeader>
                        <DialogTitle>Update Shift</DialogTitle>
                        <DialogDescription>
                            Modify your shift request
                        </DialogDescription>
                    </DialogHeader>

                    <div className="space-y-4 py-4">
                        <div className="space-y-2">
                            <Label htmlFor="startTime">Start Time</Label>
                            <Input
                                id="startTime"
                                type="datetime-local"
                                value={startTime}
                                onChange={(e) => setStartTime(e.target.value)}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="endTime">End Time</Label>
                            <Input
                                id="endTime"
                                type="datetime-local"
                                value={endTime}
                                onChange={(e) => setEndTime(e.target.value)}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="description">Description (Optional)</Label>
                            <Textarea
                                id="description"
                                value={description}
                                onChange={(e) => setDescription(e.target.value)}
                                placeholder="Add any notes about this shift..."
                                rows={3}
                            />
                        </div>
                    </div>

                    <DialogFooter>
                        <Button type="button" variant="outline" onClick={() => setUpdateDialogOpen(false)}>
                            Cancel
                        </Button>
                        <Button type="submit">Update Shift</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
