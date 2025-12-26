import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { Separator } from "@/components/ui/separator";
import { MoneyInput } from "@/components/MoneyInput";
import { SalaryProfileFormData } from "./SalaryProfileForm";

interface MonthlyProfileFieldsProps {
  formData: SalaryProfileFormData;
  onChange: (data: SalaryProfileFormData) => void;
}

export const MonthlyProfileFields = ({
  formData,
  onChange,
}: MonthlyProfileFieldsProps) => {
  return (
    <>
      {/* Base Salary Configuration */}
      <div className="space-y-4">
        <h3 className="text-sm font-semibold">Base Salary Configuration</h3>
        
        <div className="grid grid-cols-2 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="rate">Base Monthly Salary *</Label>
            <MoneyInput
              id="rate"
              value={formData.rate}
              onChange={(rate) => onChange({ ...formData, rate })}
              placeholder="0"
            />
            <p className="text-xs text-muted-foreground">
              The fixed monthly salary amount
            </p>
          </div>
            <div className="grid gap-2">
            <Label htmlFor="standardHoursPerDay">Standard Hours per Day</Label>
            <Input
              id="standardHoursPerDay"
              type="number"
              value={formData.standardHoursPerDay || ''}
              onChange={(e) => onChange({ ...formData, standardHoursPerDay: e.target.value ? parseInt(e.target.value) : undefined })}
              placeholder="8"
            />
            <p className="text-xs text-muted-foreground">
              Standard working hours per day
            </p>
          </div>
         
        </div>

        
      </div>

      <Separator />

      {/* Leave & Attendance Rules */}
      <div className="space-y-4">
        <h3 className="text-sm font-semibold">Leave & Attendance Rules</h3>
        
        <div className="grid gap-2">
          <Label>Weekly Off Days</Label>
          <div className="flex flex-wrap gap-4 p-3 border rounded-md">
            {['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'].map((day) => {
              const selectedDays = formData.weeklyOffDays ? formData.weeklyOffDays.split(',') : [];
              const isChecked = selectedDays.includes(day);
              
              return (
                <label key={day} className="flex items-center space-x-2 cursor-pointer">
                  <input
                    type="checkbox"
                    checked={isChecked}
                    onChange={(e) => {
                      const currentDays = formData.weeklyOffDays ? formData.weeklyOffDays.split(',').filter((d: string) => d) : [];
                      let newDays: string[];
                      
                      if (e.target.checked) {
                        newDays = [...currentDays, day];
                      } else {
                        newDays = currentDays.filter((d: string) => d !== day);
                      }
                      
                      onChange({ ...formData, weeklyOffDays: newDays.length > 0 ? newDays.join(',') : undefined });
                    }}
                    className="h-4 w-4 rounded border-gray-300"
                  />
                  <span className="text-sm">{day}</span>
                </label>
              );
            })}
          </div>
          <p className="text-xs text-muted-foreground">
            Select which days of the week are off days (e.g., Saturday and Sunday)
          </p>
        </div>

        <div className="grid grid-cols-3 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="paidLeaveDays">Paid Leave Days</Label>
            <Input
              id="paidLeaveDays"
              type="number"
              value={formData.paidLeaveDays || ''}
              onChange={(e) => onChange({ ...formData, paidLeaveDays: e.target.value ? parseInt(e.target.value) : undefined })}
              placeholder="10"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="unpaidLeaveDays">Unpaid Leave Days</Label>
            <Input
              id="unpaidLeaveDays"
              type="number"
              value={formData.unpaidLeaveDays || ''}
              onChange={(e) => onChange({ ...formData, unpaidLeaveDays: e.target.value ? parseInt(e.target.value) : undefined })}
              placeholder="0"
            />
          </div>

        </div>
      </div>

      <Separator />

      {/* Allowances */}
      <div className="space-y-4">
        <h3 className="text-sm font-semibold">Allowances</h3>
        
        <div className="grid grid-cols-3 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="mealAllowance">Meal Allowance</Label>
            <MoneyInput
              id="mealAllowance"
              value={formData.mealAllowance}
              onChange={(mealAllowance) => onChange({ ...formData, mealAllowance })}
              placeholder="0"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="transportAllowance">Transport Allowance</Label>
            <MoneyInput
              id="transportAllowance"
              value={formData.transportAllowance}
              onChange={(transportAllowance) => onChange({ ...formData, transportAllowance })}
              placeholder="0"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="housingAllowance">Housing Allowance</Label>
            <MoneyInput
              id="housingAllowance"
              value={formData.housingAllowance}
              onChange={(housingAllowance) => onChange({ ...formData, housingAllowance })}
              placeholder="0"
            />
          </div>
        </div>

        <div className="grid grid-cols-3 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="responsibilityAllowance">Responsibility Allowance</Label>
            <MoneyInput
              id="responsibilityAllowance"
              value={formData.responsibilityAllowance}
              onChange={(responsibilityAllowance) => onChange({ ...formData, responsibilityAllowance })}
              placeholder="0"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="attendanceBonus">Attendance Bonus</Label>
            <MoneyInput
              id="attendanceBonus"
              value={formData.attendanceBonus}
              onChange={(attendanceBonus) => onChange({ ...formData, attendanceBonus })}
              placeholder="0"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="phoneSkillShiftAllowance">Phone/Skill/Shift Allowance</Label>
            <MoneyInput
              id="phoneSkillShiftAllowance"
              value={formData.phoneSkillShiftAllowance}
              onChange={(phoneSkillShiftAllowance) => onChange({ ...formData, phoneSkillShiftAllowance })}
              placeholder="0"
            />
          </div>
        </div>
      </div>

      <Separator />

      {/* Health Insurance */}
      <div className="space-y-4">
        <div className="flex items-center space-x-2">
          <input
            type="checkbox"
            id="hasHealthInsurance"
            checked={formData.hasHealthInsurance || false}
            onChange={(e) => onChange({ ...formData, hasHealthInsurance: e.target.checked })}
            className="h-4 w-4 rounded border-gray-300"
          />
          <Label htmlFor="hasHealthInsurance" className="text-sm font-semibold cursor-pointer">
            Include Health Insurance Deduction
          </Label>
        </div>

        {formData.hasHealthInsurance && (
          <div className="pl-6 space-y-4">
            <div className="grid gap-2">
              <Label htmlFor="healthInsuranceRate">Health Insurance Rate (%)</Label>
              <Input
                id="healthInsuranceRate"
                type="number"
                value={formData.healthInsuranceRate || ''}
                onChange={(e) => onChange({ ...formData, healthInsuranceRate: e.target.value ? parseFloat(e.target.value) : undefined })}
                placeholder="0.00"
                step="0.01"
                min="0"
                max="100"
              />
              <p className="text-xs text-muted-foreground">
                Percentage of base salary to deduct for health insurance (0-100%)
              </p>
            </div>
          </div>
        )}
      </div>

      <Separator />

      {/* Overtime Configuration */}
      <div className="space-y-4">
        <h3 className="text-sm font-semibold">Overtime Configuration</h3>
        
        <div className="grid grid-cols-2 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="otRateWeekday">OT Rate - Weekday (150%)</Label>
            <Input
              id="otRateWeekday"
              type="number"
              value={formData.otRateWeekday || ''}
              onChange={(e) => onChange({ ...formData, otRateWeekday: e.target.value ? parseFloat(e.target.value) : undefined })}
              placeholder="1.5"
              step="0.1"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="otRateWeekend">OT Rate - Weekend (200%)</Label>
            <Input
              id="otRateWeekend"
              type="number"
              value={formData.otRateWeekend || ''}
              onChange={(e) => onChange({ ...formData, otRateWeekend: e.target.value ? parseFloat(e.target.value) : undefined })}
              placeholder="2.0"
              step="0.1"
            />
          </div>
        </div>

        <div className="grid grid-cols-3 gap-4">
          <div className="grid gap-2">
            <Label htmlFor="otRateHoliday">OT Rate - Holiday (300%)</Label>
            <Input
              id="otRateHoliday"
              type="number"
              value={formData.otRateHoliday || ''}
              onChange={(e) => onChange({ ...formData, otRateHoliday: e.target.value ? parseFloat(e.target.value) : undefined })}
              placeholder="3.0"
              step="0.1"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="nightShiftRate">Night Shift Rate</Label>
            <Input
              id="nightShiftRate"
              type="number"
              value={formData.nightShiftRate || ''}
              onChange={(e) => onChange({ ...formData, nightShiftRate: e.target.value ? parseFloat(e.target.value) : undefined })}
              placeholder="1.3"
              step="0.1"
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="otHourLimit">OT Hour Limit/Month</Label>
            <Input
              id="otHourLimit"
              type="number"
              value={formData.otHourLimitPerMonth || ''}
              onChange={(e) => onChange({ ...formData, otHourLimitPerMonth: e.target.value ? parseInt(e.target.value) : undefined })}
              placeholder="40"
            />
          </div>
        </div>
      </div>

      <div className="p-4 bg-muted/50 rounded-lg">
        <p className="text-sm text-muted-foreground">
          <strong>Monthly Salary Profile:</strong> Configure base salary, weekly off days, leave policies, 
          allowances, and overtime rates. All fields are optional except base salary and currency.
        </p>
      </div>
    </>
  );
};
