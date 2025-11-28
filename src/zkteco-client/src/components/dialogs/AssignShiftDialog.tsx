import { useState } from 'react';
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
import { CalendarIcon } from 'lucide-react';
import { DateTimeFormat } from '@/constants';
import { useEmployeesByManager } from '@/hooks/useAccount';
import { defaultTemplateShift } from '@/constants/defaultValue';
import { CreateShiftRequest } from '@/types/shift';

type AssignShiftDialogProps =
{
    open: boolean;
    onOpenChange: (open: boolean) => void;
    onSubmit: (data: CreateShiftRequest) => Promise<void>;
};

const getDateString = (time: string, dateStr: Date) => {
    const [hours, minutes] = time.split(':');
    const date = new Date(dateStr);
    date.setHours(parseInt(hours), parseInt(minutes), 0, 0);
    return format(date, DateTimeFormat);
}

export const AssignShiftDialog = ({ 
    open, 
    onOpenChange, 
    onSubmit,
}: AssignShiftDialogProps) => {
    const { data: templates = [], isLoading: templatesLoading } = useShiftTemplates();
    const { data: employees = [], isLoading: employeesLoading } = useEmployeesByManager();
    const [employeeUserId, setEmployeeUserId] = useState<string | null>(null);
    const [templateShift, setTemplateShift] = useState(defaultTemplateShift);
    const hasTemplates = templates.length > 0;

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!templateShift.templateId || !templateShift.date || !employeeUserId) {
            return;
        }

        const selectedTemplate = templates.find(t => t.id === templateShift.templateId);
        if (!selectedTemplate) return;

        const data: CreateShiftRequest = {
            employeeUserId: employeeUserId,
            startTime: getDateString(selectedTemplate.startTime, templateShift.date),
            endTime: getDateString(selectedTemplate.endTime, templateShift.date),
            maximumAllowedLateMinutes: selectedTemplate.maximumAllowedLateMinutes,
            maximumAllowedEarlyLeaveMinutes: selectedTemplate.maximumAllowedEarlyLeaveMinutes,
            description: templateShift.description,
        };

        await onSubmit(data);
        setEmployeeUserId(null);
        setTemplateShift(defaultTemplateShift);
    };

    const handleOpenChange = (open: boolean) => {
        onOpenChange(open);
        if (!open) {
            setEmployeeUserId(null);
            setTemplateShift(defaultTemplateShift);
        }
    };

    return (
        <Dialog open={open} onOpenChange={handleOpenChange}>
            <DialogContent className="max-w-[95vw] sm:max-w-[500px] max-h-[90vh] overflow-y-auto">
                <DialogHeader>
                    <DialogTitle>Assign Shift to Employee</DialogTitle>
                    <DialogDescription>
                        Assign a shift directly to an employee
                    </DialogDescription>
                </DialogHeader>

                <form onSubmit={handleSubmit}>
                    <div className="space-y-4 py-4">
                        <div className="space-y-2">
                            <Label htmlFor="employee">Employee</Label>
                            {employeesLoading ? (
                                <div className="text-sm text-muted-foreground">Loading employees...</div>
                            ) : (
                                <Select
                                    value={employeeUserId ?? ''}
                                    onValueChange={(value) => setEmployeeUserId(value)}
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

                        <div className="space-y-2">
                            <Label htmlFor="template">Select Shift Template</Label>
                            {templatesLoading ? (
                                <div className="text-sm text-muted-foreground">Loading templates...</div>
                            ) : !hasTemplates ? (
                                <div className="text-sm text-muted-foreground">No templates available. Please create a template first.</div>
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
                            <Label htmlFor="description">Description (Optional)</Label>
                            <Textarea
                                id="description"
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
                            disabled={!employeeUserId || !templateShift.templateId || !hasTemplates}
                        >
                            Assign Shift
                        </Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    );
};
