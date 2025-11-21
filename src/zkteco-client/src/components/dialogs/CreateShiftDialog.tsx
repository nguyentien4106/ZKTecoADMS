import { useState } from 'react';
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { DateTimePicker } from '@/components/ui/datetime-picker';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { useShiftTemplates } from '@/hooks/useShiftTemplate';
import { Calendar } from '@/components/ui/calendar';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { format, getDate } from 'date-fns';
import { CalendarIcon } from 'lucide-react';
import { DateTimeFormat } from '@/constants';
import { useEmployeesByManager } from '@/hooks/useAccount';
import { defaultNewShiftWithEmployeeUserId, defaultTemplateShift } from '@/constants/defaultValue';
import { CreateShiftRequest } from '@/types/shift';

type CreateShiftDialogProps =
{
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onSubmit: (data: CreateShiftRequest) => Promise<void>;
    mode : 'request' | 'assign';
    title?: string;
    description?: string;
};

const getDateString = (time: string, dateStr: Date) => {
    const [hours, minutes] = time.split(':');
    const date = new Date(dateStr);
    date.setHours(parseInt(hours), parseInt(minutes), 0, 0);
    return format(date, DateTimeFormat);
}

export const CreateShiftDialog = ({ 
    open, 
    onOpenChange, 
    onSubmit,
    mode = 'request',
    title,
    description: dialogDescription
}: CreateShiftDialogProps) => {
    const { data: templates = [], isLoading: templatesLoading } = useShiftTemplates();
    const includeEmployee = mode === 'assign';
    const { data: employees = [], isLoading: employeesLoading } = useEmployeesByManager(includeEmployee);
    const [activeTab, setActiveTab] = useState<'manual' | 'template'>('manual');
    const [newShift, setNewShift] = useState(defaultNewShiftWithEmployeeUserId);
    const [templateShift, setTemplateShift] = useState(defaultTemplateShift);
    const hasTemplates = templates.length > 0;

    const now = new Date();
    const maxEndTime = new Date();
    maxEndTime.setDate(maxEndTime.getDate() + 1);

    const handleManualSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newShift.startTime || !newShift.endTime) {
            return;
        }

        const data: CreateShiftRequest = {
            employeeUserId: newShift.employeeUserId === '' ? null : newShift.employeeUserId,
            startTime: format(newShift.startTime, DateTimeFormat),
            endTime: format(newShift.endTime, DateTimeFormat),
            description: newShift.description
        }
        await onSubmit(data);
        setNewShift(defaultNewShiftWithEmployeeUserId);
    };

    const handleTemplateSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!templateShift.templateId || !templateShift.date) {
            return;
        }

        const selectedTemplate = templates.find(t => t.id === templateShift.templateId);
        if (!selectedTemplate) return;

        const data: CreateShiftRequest = {
            employeeUserId: newShift.employeeUserId == '' ? null : newShift.employeeUserId,
            startTime: getDateString(selectedTemplate.startTime, templateShift.date),
            endTime: getDateString(selectedTemplate.endTime, templateShift.date),
            description: templateShift.description,
        };
        console.log('Submitting shift from template:', data);
        await onSubmit(data);
        setNewShift(defaultNewShiftWithEmployeeUserId);
        setTemplateShift(defaultTemplateShift);
    };

    const handleOpenChange = (open: boolean) => {
        onOpenChange(open);
        if (!open) {
            setNewShift(defaultNewShiftWithEmployeeUserId);
            setTemplateShift(defaultTemplateShift);
            setActiveTab('manual');
        }
    };

    const defaultTitle = mode === 'assign' ? 'Assign Shift to Employee' : 'Request New Shift';
    const defaultDescription = mode === 'assign' 
        ? 'Assign a shift directly to an employee'
        : 'Submit a shift request for manager approval';

    return (
        <Dialog open={open} onOpenChange={handleOpenChange}>
            <DialogContent className="max-w-[95vw] sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
                <DialogHeader>
                    <DialogTitle>{title || defaultTitle}</DialogTitle>
                    <DialogDescription>
                        {dialogDescription || defaultDescription}
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
                                {includeEmployee && (
                                    <div className="space-y-2">
                                        <Label htmlFor="employee">Employee</Label>
                                        {employeesLoading ? (
                                            <div className="text-sm text-muted-foreground">Loading employees...</div>
                                        ) : (
                                            <Select
                                                value={'employeeUserId' in newShift ? newShift.employeeUserId : ''}
                                                onValueChange={(value) => setNewShift({ ...newShift, employeeUserId: value })}
                                            >
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select an employee..." />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    {employees.map((employee) => (
                                                        <SelectItem key={employee.id} value={employee.id}>
                                                            {employee.fullName}
                                                        </SelectItem>
                                                    ))}
                                                </SelectContent>
                                            </Select>
                                        )}
                                    </div>
                                )}

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
                                        maxDate={newShift.startTime ? new Date(newShift.startTime.getTime() + 24 * 60 * 60 * 1000) : maxEndTime}
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
                                <Button 
                                    type="submit" 
                                    className="w-full sm:w-auto"
                                    disabled={includeEmployee && !('employeeUserId' in newShift && newShift.employeeUserId)}
                                >
                                    {mode === 'assign' ? 'Assign Shift' : 'Submit Request'}
                                </Button>
                            </DialogFooter>
                        </form>
                    </TabsContent>

                    <TabsContent value="template">
                        <form onSubmit={handleTemplateSubmit}>
                            <div className="space-y-4 py-4">
                                {includeEmployee && (
                                    <div className="space-y-2">
                                        <Label htmlFor="templateEmployee">Employee</Label>
                                        {employeesLoading ? (
                                            <div className="text-sm text-muted-foreground">Loading employees...</div>
                                        ) : (
                                            <Select
                                                value={'employeeUserId' in newShift ? newShift.employeeUserId : ''}
                                                onValueChange={(value) => setNewShift({ ...newShift, employeeUserId: value })}
                                            >
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select an employee..." />
                                                </SelectTrigger>
                                                <SelectContent>
                                                    {employees.map((employee) => (
                                                        <SelectItem key={employee.id} value={employee.id}>
                                                            {employee.fullName}
                                                        </SelectItem>
                                                    ))}
                                                </SelectContent>
                                            </Select>
                                        )}
                                    </div>
                                )}

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
                                <Button 
                                    type="submit" 
                                    className="w-full sm:w-auto"
                                    disabled={includeEmployee && !('employeeUserId' in templateShift && templateShift.employeeUserId)}
                                >
                                    {mode === 'assign' ? 'Assign Shift' : 'Submit Request'}
                                </Button>
                            </DialogFooter>
                        </form>
                    </TabsContent>
                </Tabs>
            </DialogContent>
        </Dialog>
    );
};
