import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { MonthPicker } from '@/components/ui/monthpicker'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { Label } from '@/components/ui/label'
import { 
    MultiSelect,
    MultiSelectTrigger,
    MultiSelectValue,
    MultiSelectContent,
    MultiSelectItem
} from '@/components/ui/multi-select'
import { CalendarIcon, Filter, X } from 'lucide-react'
import { Employee } from '@/types/employee'
import { format } from 'date-fns'
import { cn } from '@/lib/utils'

interface ShiftFilterBarProps {
    selectedMonth: Date
    selectedEmployeeIds: string[]
    employees: Employee[]
    onMonthChange: (date: Date | undefined) => void
    onEmployeeIdsChange: (employeeIds: string[]) => void
    onApplyFilters: () => void
    onClearFilters: () => void
    isLoading?: boolean
}

export const ManagementShiftFilterBar = ({ 
    selectedMonth,
    selectedEmployeeIds,
    employees,
    onMonthChange,
    onEmployeeIdsChange,
    onApplyFilters,
    onClearFilters,
    isLoading = false
}: ShiftFilterBarProps) => {
    console.log("Selected Month in selectedEmployeeIds:", selectedEmployeeIds);
    const employeeOptions = employees.map(emp => ({
        value: emp.id,
        label: `${emp.firstName} ${emp.lastName}`,
    }))

    return (
        <Card className="shadow-sm mb-6">
            <CardContent className="p-6">
                <div className="space-y-6">
                    {/* Header */}
                    <div className="flex items-center gap-2 pb-2 border-b">
                        <Filter className="w-5 h-5 text-primary" />
                        <h3 className="text-base font-semibold">Filter Shifts</h3>
                    </div>

                    {/* Filter Inputs */}
                    <div className="flex flex-col md:flex-row gap-4">
                        {/* Month Picker */}
                        <div className="flex-1 max-w-[200px]">
                            <div className="flex flex-col gap-3">
                                <Label className="text-sm font-medium px-1">Month & Year</Label>
                                <Popover>
                                    <PopoverTrigger asChild>
                                        <Button
                                            variant="outline"
                                            disabled={isLoading}
                                            className={cn(
                                                "w-full justify-start text-left font-normal",
                                                !selectedMonth && "text-muted-foreground"
                                            )}
                                        >
                                            <CalendarIcon className="mr-2 h-4 w-4" />
                                            {selectedMonth ? format(selectedMonth, "MMM yyyy") : "Select month"}
                                        </Button>
                                    </PopoverTrigger>
                                    <PopoverContent className="w-auto p-0" align="start">
                                        <MonthPicker
                                            selectedMonth={selectedMonth}
                                            onMonthSelect={onMonthChange}
                                        />
                                    </PopoverContent>
                                </Popover>
                            </div>
                        </div>

                        {/* Employee Multi-Select */}
                        <div className="flex-1">
                            <div className="flex flex-col gap-3">
                                <Label className="text-sm font-medium px-1">Employees (Hourly) </Label>
                                <MultiSelect
                                    values={selectedEmployeeIds}
                                    onValuesChange={onEmployeeIdsChange}
                                >
                                    <MultiSelectTrigger className="w-full max-w-[280px] max-h-[40px]">
                                        <MultiSelectValue placeholder="Select employees..." />
                                    </MultiSelectTrigger>
                                    <MultiSelectContent className="max-h-[300px] overflow-y-auto">
                                        {employeeOptions.map((employee) => (
                                            <MultiSelectItem
                                                key={employee.value}
                                                value={employee.value}
                                            >
                                                {employee.label}
                                            </MultiSelectItem>
                                        ))}
                                    </MultiSelectContent>
                                </MultiSelect>
                            </div>
                        </div>

                        {/* Spacer for alignment */}
                        <div className="flex-1 hidden md:block" />
                    </div>

                    {/* Action Buttons */}
                    <div className="flex items-center gap-3 pt-2">
                        <Button
                            onClick={onApplyFilters}
                            className="px-6"
                            size="sm"
                            disabled={isLoading}
                        >
                            <Filter className="w-4 h-4 mr-2" />
                            Apply Filters
                        </Button>
                        <Button
                            onClick={onClearFilters}
                            variant="outline"
                            className="px-6"
                            size="sm"
                            disabled={isLoading}
                        >
                            <X className="w-4 h-4 mr-2" />
                            Clear
                        </Button>
                    </div>
                </div>
            </CardContent>
        </Card>
    )
}
