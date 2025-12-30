import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { MonthPicker } from '@/components/ui/monthpicker'
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { CalendarIcon, Filter, X } from 'lucide-react'
import { ShiftStatus } from '@/types/shift'
import { format } from 'date-fns'
import { cn } from '@/lib/utils'

interface MyShiftsFilterBarProps {
  selectedMonth?: Date
  selectedStatus?: ShiftStatus | 'all'
  onMonthChange: (date: Date | undefined) => void
  onStatusChange: (status: ShiftStatus | 'all') => void
  onApplyFilters: () => void
  onClearFilters: () => void
}

const statusOptions = [
  { value: 'all', label: 'All Status' },
  { value: ShiftStatus.Pending.toString(), label: 'Pending' },
  { value: ShiftStatus.Approved.toString(), label: 'Approved' },
  { value: ShiftStatus.Rejected.toString(), label: 'Rejected' },
  { value: ShiftStatus.Cancelled.toString(), label: 'Cancelled' },
  { value: ShiftStatus.ApprovedLeave.toString(), label: 'Approved Leave' },
]

const getStatusLabel = (status?: ShiftStatus | 'all'): string => {
  if (status === undefined || status === 'all') return 'All Status'
  const option = statusOptions.find(opt => opt.value === status.toString())
  return option?.label || 'All Status'
}

export const MyShiftsFilterBar = ({
  selectedMonth,
  selectedStatus,
  onMonthChange,
  onStatusChange,
  onApplyFilters,
  onClearFilters
}: MyShiftsFilterBarProps) => {
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
            <div className="flex-1">
              <div className="flex flex-col gap-3">
                <Label className="text-sm font-medium px-1">Month & Year</Label>
                <Popover>
                  <PopoverTrigger asChild>
                    <Button
                      variant="outline"
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

            {/* Status Select */}
            <div className="flex-1">
              <div className="flex flex-col gap-3">
                <Label className="text-sm font-medium px-1">Status</Label>
                <Select
                  value={selectedStatus?.toString() || 'all'}
                  onValueChange={(value) => {
                    onStatusChange(value === 'all' ? 'all' : parseInt(value) as ShiftStatus)
                  }}
                >
                  <SelectTrigger className="w-full max-w-[280px]">
                    <SelectValue>
                      {getStatusLabel(selectedStatus)}
                    </SelectValue>
                  </SelectTrigger>
                  <SelectContent>
                    {statusOptions.map((option) => (
                      <SelectItem key={option.value} value={option.value}>
                        {option.label}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
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
            >
              <Filter className="w-4 h-4 mr-2" />
              Apply Filters
            </Button>
            <Button
              onClick={onClearFilters}
              variant="outline"
              className="px-6"
              size="sm"
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
