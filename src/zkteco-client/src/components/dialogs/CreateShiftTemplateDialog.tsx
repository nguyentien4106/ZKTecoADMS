import { useState } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { defaultNewShiftTemplate } from '@/constants/defaultValue';
import { useShiftManagementContext } from '@/contexts/ShiftManagementContext';

export const CreateShiftTemplateDialog = () => {
    const [template, setTemplate] = useState(defaultNewShiftTemplate);
    const {
        createTemplateDialogOpen,
        setCreateTemplateDialogOpen,
        handleCreateTemplate,
    } = useShiftManagementContext()

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!template.name || !template.startTime || !template.endTime) {
            return;
        }
        
        // Convert HH:mm to HH:mm:ss format for TimeSpan
        const startTime = `${template.startTime}:00`;
        const endTime = `${template.endTime}:00`;
        
        handleCreateTemplate({
            name: template.name,
            startTime,
            endTime,
        });
        
        setTemplate(defaultNewShiftTemplate);
    };

    const handleOpenChangeInternal = (open: boolean) => {
        setCreateTemplateDialogOpen(open);
        if (!open) {
            setTemplate(defaultNewShiftTemplate);
        }
    };

    return (
        <Dialog open={createTemplateDialogOpen} onOpenChange={handleOpenChangeInternal}>
            <DialogContent className="max-w-[95vw] sm:max-w-[500px]">
                <form onSubmit={handleSubmit}>
                    <DialogHeader>
                        <DialogTitle>Create Shift Template</DialogTitle>
                        <DialogDescription>
                            Create a reusable shift template for quick shift scheduling
                        </DialogDescription>
                    </DialogHeader>

                    <div className="space-y-4 py-4">
                        <div className="space-y-2">
                            <Label htmlFor="name">Template Name</Label>
                            <Input
                                id="name"
                                value={template.name}
                                onChange={(e) => setTemplate({ ...template, name: e.target.value })}
                                placeholder="e.g., Morning Shift, Night Shift"
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="startTime">Start Time</Label>
                            <Input
                                id="startTime"
                                type="time"
                                value={template.startTime}
                                onChange={(e) => setTemplate({ ...template, startTime: e.target.value })}
                                required
                            />
                        </div>

                        <div className="space-y-2">
                            <Label htmlFor="endTime">End Time</Label>
                            <Input
                                id="endTime"
                                type="time"
                                value={template.endTime}
                                onChange={(e) => setTemplate({ ...template, endTime: e.target.value })}
                                required
                            />
                        </div>
                    </div>

                    <DialogFooter className="flex-col sm:flex-row gap-2">
                        <Button 
                            type="button" 
                            variant="outline" 
                            onClick={() => handleOpenChangeInternal(false)}
                            className="w-full sm:w-auto"
                        >
                            Cancel
                        </Button>
                        <Button type="submit" className="w-full sm:w-auto">Create Template</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
