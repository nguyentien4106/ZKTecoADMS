import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { DatePicker } from '@/components/date-picker'
import { MultiSelect } from '@/components/multi-select'
import { Filter } from 'lucide-react'
import { AttendancesFilterParams } from '@/types/attendance'

export interface Option {
  value: string
  label: string
}

interface AttendanceFilterBarProps {
  deviceOptions: Option[]
  onApplyFilters: () => void
  onClearFilters: () => void
  filter: AttendancesFilterParams
  onDateChange: (date: Date | undefined, type: 'fromDate' | 'toDate') => void
  onSelectChange: (values: string[]) => void
}

export const AttendanceFilterBar = ({
  deviceOptions,
  onApplyFilters,
  onClearFilters,
  filter,
  onDateChange,
  onSelectChange
}: AttendanceFilterBarProps) => {
  return (
    <Card className="shadow-sm">
      <CardContent className="p-6">
        <div className="space-y-6">
          {/* Header */}
          <div className="flex items-center gap-2 pb-2 border-b">
            <Filter className="w-5 h-5 text-primary" />
            <h3 className="text-base font-semibold">Filter Attendance</h3>
          </div>

          {/* Filter Inputs */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="space-y-2">
              <DatePicker
                label="From Date"
                value={filter.fromDate}
                onSelectDate={date => onDateChange(date, 'fromDate')}
                placeholder="Select start date"
              />
            </div>
            
            <div className="space-y-2">
              <DatePicker
                label="To Date"
                value={filter.toDate}
                onSelectDate={date => onDateChange(date, 'toDate')}
                placeholder="Select end date"
              />
            </div>
            
            <div className="space-y-2">
              <div className="flex flex-col gap-3">
                <label className="text-sm font-medium px-1">Devices</label>
                <MultiSelect
                  options={deviceOptions}
                  defaultValue={filter.deviceIds}
                  onValueChange={values => onSelectChange(values)}
                  placeholder="Select devices..."
                />
              </div>
            </div>
          </div>

          {/* Action Buttons */}
          <div className="flex items-center gap-3 pt-2">
            <Button 
              onClick={onApplyFilters}
              className="px-6"
            >
              Apply Filters
            </Button>
            <Button 
              variant="outline" 
              onClick={onClearFilters}
              className="px-6"
            >
              Clear Filters
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  )
}
