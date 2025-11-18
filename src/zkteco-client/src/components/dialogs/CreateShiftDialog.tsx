import { useState } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { DateTimePicker } from '@/components/ui/datetime-picker';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { useShiftContext } from '@/contexts/ShiftContext';
import { useShiftTemplates } from '@/hooks/useShiftTemplate';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { format } from 'date-fns';
import { CalendarIcon } from 'lucide-react';
import { DateTimeFormat } from '@/constants';

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

const getDefaultTemplateShift = () => ({
    templateId: '',
    date: new Date(),
    description: '',
});

export const CreateShiftDialog = () => {
    const {
        createDialogOpen,
        setCreateDialogOpen,
        handleCreate,
    } = useShiftContext();

    const { data: templates = [], isLoading: templatesLoading } = useShiftTemplates();
    const [activeTab, setActiveTab] = useState<'manual' | 'template'>('manual');
    const [newShift, setNewShift] = useState(getDefaultNewShift());
    const [templateShift, setTemplateShift] = useState(getDefaultTemplateShift());

    const now = new Date();
    const maxEndTime = new Date();
    maxEndTime.setDate(maxEndTime.getDate() + 1);

    const hasTemplates = templates.length > 0;

    const handleManualSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!newShift.startTime || !newShift.endTime) {
            return;
        }
        
        handleCreate({
            startTime: format(newShift.startTime, DateTimeFormat),
            endTime: format(newShift.endTime, DateTimeFormat),
            description: newShift.description,
        });
        setNewShift(getDefaultNewShift());
    };

    const handleTemplateSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!templateShift.templateId || !templateShift.date) {
            return;
        }

        const selectedTemplate = templates.find(t => t.id === templateShift.templateId);
        if (!selectedTemplate) return;

        // Parse the TimeSpan format "HH:mm:ss" and combine with selected date
        const [startHours, startMinutes] = selectedTemplate.startTime.split(':');
        const [endHours, endMinutes] = selectedTemplate.endTime.split(':');
        
        const startTime = new Date(templateShift.date);
        startTime.setHours(parseInt(startHours), parseInt(startMinutes), 0, 0);

        const endTime = new Date(templateShift.date);
        endTime.setHours(parseInt(endHours), parseInt(endMinutes), 0, 0);

        handleCreate({
            startTime: format(startTime, DateTimeFormat),
            endTime: format(endTime, DateTimeFormat),
            description: templateShift.description,
        });
        setTemplateShift(getDefaultTemplateShift());
    };

    const handleOpenChange = (open: boolean) => {
        setCreateDialogOpen(open);
        if (!open) {
            setNewShift(getDefaultNewShift());
            setTemplateShift(getDefaultTemplateShift());
            setActiveTab('manual');
        }
    };

    return (
        <Dialog open={createDialogOpen} onOpenChange={handleOpenChange}>
            <DialogContent className="max-w-[95vw] sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
                <DialogHeader>
                    <DialogTitle>Request New Shift</DialogTitle>
                    <DialogDescription>
                        Submit a shift request for manager approval
                    </DialogDescription>
                </DialogHeader>

                <Tabs value={activeTab} onValueChange={(v) => setActiveTab(v as 'manual' | 'template')} className="w-full">
                    <TabsList className="grid w-full grid-cols-2">
                        <TabsTrigger value="manual">Manual Entry</TabsTrigger>
                        <TabsTrigger value="template" disabled={!hasTemplates}>
                            Use Template {hasTemplates && `(${templates.length})`}
                        </TabsTrigger>
                    </TabsList>

                    <TabsContent value="manual">
                        <form onSubmit={handleManualSubmit}>
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
                                        className="resize-none"
                                    />
                                </div>
                            </div>

                            <DialogFooter className="flex-col sm:flex-row gap-2">
                                <Button 
                                    type="button" 
                                    variant="outline" 
                                    onClick={() => handleOpenChange(false)}
                                    className="w-full sm:w-auto"
                                >
                                    Cancel
                                </Button>
                                <Button type="submit" className="w-full sm:w-auto">Submit Request</Button>
                            </DialogFooter>
                        </form>
                    </TabsContent>

                    <TabsContent value="template">
                        <form onSubmit={handleTemplateSubmit}>
                            <div className="space-y-4 py-4">
                                <div className="space-y-2">
                                    <Label htmlFor="template">Select Shift Template</Label>
                                    {templatesLoading ? (
                                        <div className="text-sm text-muted-foreground">Loading templates...</div>
                                    ) : (
                                        <Select
                                            value={templateShift.templateId}
                                            onValueChange={(value) => setTemplateShift({ ...templateShift, templateId: value })}
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

                                <div className="space-y-2">
                                    <Label htmlFor="date">Shift Date</Label>
                                    <Popover>
                                        <PopoverTrigger asChild>
                                            <Button
                                                variant="outline"
                                                className={cn(
                                                    "w-full justify-start text-left font-normal",
                                                    !templateShift.date && "text-muted-foreground"
                                                )}
                                            >
                                                <CalendarIcon className="mr-2 h-4 w-4" />
                                                {templateShift.date ? format(templateShift.date, "PPP") : <span>Pick a date</span>}
                                            </Button>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto p-0">
                                            <Calendar
                                                mode="single"
                                                style={{ minWidth: "250px" }}
                                                selected={templateShift.date}
                                                onSelect={(date) => setTemplateShift({ ...templateShift, date: date || new Date() })}
                                                disabled={(date) => date < new Date(new Date().setHours(0, 0, 0, 0))}
                                                initialFocus
                                            />
                                        </PopoverContent>
                                    </Popover>
                                </div>

                                <div className="space-y-2">
                                    <Label htmlFor="templateDescription">Description (Optional)</Label>
                                    <Textarea
                                        id="templateDescription"
                                        value={templateShift.description}
                                        onChange={(e) => setTemplateShift({ ...templateShift, description: e.target.value })}
                                        placeholder="Add any notes about this shift..."
                                        rows={3}
                                        className="resize-none"
                                    />
                                </div>
                            </div>

                            <DialogFooter className="flex-col sm:flex-row gap-2">
                                <Button 
                                    type="button" 
                                    variant="outline" 
                                    onClick={() => handleOpenChange(false)}
                                    className="w-full sm:w-auto"
                                >
                                    Cancel
                                </Button>
                                <Button type="submit" className="w-full sm:w-auto">Submit Request</Button>
                            </DialogFooter>
                        </form>
                    </TabsContent>
                </Tabs>
            </DialogContent>
        </Dialog>
    );
};
