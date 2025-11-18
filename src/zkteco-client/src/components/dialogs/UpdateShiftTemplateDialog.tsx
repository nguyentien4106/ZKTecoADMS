import { useState, useEffect } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { ShiftTemplate } from '@/types/shift';

interface UpdateShiftTemplateDialogProps {
    open: boolean;
    onOpenChange: (open: boolean) => void;
    template: ShiftTemplate | null;
    onUpdate: (id: string, data: { name: string; startTime: string; endTime: string; isActive: boolean }) => void;
}

const extractTime = (timeString: string): string => {
    // TimeSpan comes as "HH:mm:ss" format, extract HH:mm for the time input
    if (timeString.includes(':')) {
        const parts = timeString.split(':');
        return `${parts[0]}:${parts[1]}`;
    }
    // Fallback for ISO datetime format
    const date = new Date(timeString);
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
};

export const UpdateShiftTemplateDialog = ({ open, onOpenChange, template, onUpdate }: UpdateShiftTemplateDialogProps) => {
    const [formData, setFormData] = useState({
        name: '',
        startTime: '09:00',
        endTime: '17:00',
        isActive: true,
    });

    useEffect(() => {
        if (template) {
            setFormData({
                name: template.name,
                startTime: extractTime(template.startTime),
                endTime: extractTime(template.endTime),
                isActive: template.isActive,
            });
        }
    }, [template]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!template || !formData.name || !formData.startTime || !formData.endTime) {
            return;
        }
        
        // Convert HH:mm to HH:mm:ss format for TimeSpan
        const startTime = `${formData.startTime}:00`;
        const endTime = `${formData.endTime}:00`;
        
        onUpdate(template.id, {
            name: formData.name,
            startTime,
            endTime,
            isActive: formData.isActive,
        });
    };

    if (!template) return null;

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent className="max-w-[95vw] sm:max-w-[500px]">
                <form onSubmit={handleSubmit}>
                    <DialogHeader>
                        <DialogTitle>Update Shift Template</DialogTitle>
                        <DialogDescription>
                            Update the shift template details
                        </DialogDescription>
                    </DialogHeader>

                    <div className="space-y-4 py-4">
                        <div className="space-y-2">
                            <Label htmlFor="name">Template Name</Label>
                            <Input
                                id="name"
                                value={formData.name}
                                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                                placeholder="e.g., Morning Shift, Night Shift"
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="startTime">Start Time</Label>
                            <Input
                                id="startTime"
                                type="time"
                                value={formData.startTime}
                                onChange={(e) => setFormData({ ...formData, startTime: e.target.value })}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="endTime">End Time</Label>
                            <Input
                                id="endTime"
                                type="time"
                                value={formData.endTime}
                                onChange={(e) => setFormData({ ...formData, endTime: e.target.value })}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="isActive">Status</Label>
                            <Select
                                value={formData.isActive ? "active" : "inactive"}
                                onValueChange={(value) => setFormData({ ...formData, isActive: value === "active" })}
                            >
                                <SelectTrigger>
                                    <SelectValue />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="active">Active</SelectItem>
                                    <SelectItem value="inactive">Inactive</SelectItem>
                                </SelectContent>
                            </Select>
                        </div>
                    </div>

                    <DialogFooter className="flex-col sm:flex-row gap-2">
                        <Button 
                            type="button" 
                            variant="outline" 
                            onClick={() => onOpenChange(false)}
                            className="w-full sm:w-auto"
                        >
                            Cancel
                        </Button>
                        <Button type="submit" className="w-full sm:w-auto">Update Template</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
