import { format } from "date-fns";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Badge } from "@/components/ui/badge";
import { Separator } from "@/components/ui/separator";
import { EmployeeSalaryProfile, SalaryRateType } from "@/services/salaryProfileService";

interface EmployeeSalaryProfileDetailDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  profile: EmployeeSalaryProfile | null;
}

export const EmployeeSalaryProfileDetailDialog = ({
  open,
  onOpenChange,
  profile,
}: EmployeeSalaryProfileDetailDialogProps) => {
  if (!profile) return null;

  const isHourly = profile.rateType === SalaryRateType.Hourly;
  const isMonthly = profile.rateType === SalaryRateType.Monthly;

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-3xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>{profile.employeeName} Salary Details</DialogTitle>
          <DialogDescription>
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-6">
          {/* Basic Information */}
          <div>
            <h3 className="text-lg font-semibold mb-3">Basic Information</h3>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Employee</p>
                <p className="font-medium">{profile.employeeName}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Effective Date</p>
                <p className="font-medium">{format(new Date(profile.effectiveDate), 'MMM dd, yyyy')}</p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">End Date</p>
                <p className="font-medium">
                  {profile.endDate ? format(new Date(profile.endDate), 'MMM dd, yyyy') : 'Active'}
                </p>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Status</p>
                <Badge variant={profile.isActive ? "default" : "secondary"}>
                  {profile.isActive ? 'Active' : 'Inactive'}
                </Badge>
              </div>
              {profile.notes && (
                <div className="col-span-2">
                  <p className="text-sm text-muted-foreground">Notes</p>
                  <p className="font-medium">{profile.notes}</p>
                </div>
              )}
            </div>
          </div>

          <Separator />

          {/* Salary Configuration */}
          <div>
            <h3 className="text-lg font-semibold mb-3">Salary Configuration</h3>
            <div className="grid grid-cols-2 gap-4">
              <div>
                <p className="text-sm text-muted-foreground">Rate Type</p>
                <Badge variant="outline">
                  {isHourly ? 'Hourly' : isMonthly ? 'Monthly' : 'Unknown'}
                </Badge>
              </div>
              <div>
                <p className="text-sm text-muted-foreground">Base Rate</p>
                <p className="font-medium text-lg">
                  {profile.rate.toLocaleString()} {profile.currency}
                  {isHourly && ' / hour'}
                  {isMonthly && ' / month'}
                </p>
              </div>
              {isMonthly && profile.standardHoursPerDay && (
                <div>
                  <p className="text-sm text-muted-foreground">Standard Hours/Day</p>
                  <p className="font-medium">{profile.standardHoursPerDay} hours</p>
                </div>
              )}
            </div>
          </div>

          <Separator />

          {/* Multipliers - Show for both Hourly and Monthly */}
          <div>
            <h3 className="text-lg font-semibold mb-3">Rate Multipliers</h3>
            <div className="grid grid-cols-3 gap-4">
              {profile.overtimeMultiplier && (
                <div>
                  <p className="text-sm text-muted-foreground">Overtime</p>
                  <p className="font-medium">{profile.overtimeMultiplier}x</p>
                </div>
              )}
              {profile.holidayMultiplier && (
                <div>
                  <p className="text-sm text-muted-foreground">Holiday</p>
                  <p className="font-medium">{profile.holidayMultiplier}x</p>
                </div>
              )}
              {profile.nightShiftMultiplier && (
                <div>
                  <p className="text-sm text-muted-foreground">Night Shift</p>
                  <p className="font-medium">{profile.nightShiftMultiplier}x</p>
                </div>
              )}
            </div>
          </div>

          {/* Monthly-specific information */}
          {isMonthly && (
            <>
              <Separator />

              {/* Leave & Attendance Rules */}
              <div>
                <h3 className="text-lg font-semibold mb-3">Leave & Attendance Rules</h3>
                <div className="grid grid-cols-2 gap-4">

                  {profile.paidLeaveDays !== null && profile.paidLeaveDays !== undefined && (
                    <div>
                      <p className="text-sm text-muted-foreground">Paid Leave Days</p>
                      <p className="font-medium">{profile.paidLeaveDays} days</p>
                    </div>
                  )}
                  {profile.unpaidLeaveDays !== null && profile.unpaidLeaveDays !== undefined && (
                    <div>
                      <p className="text-sm text-muted-foreground">Unpaid Leave Days</p>
                      <p className="font-medium">{profile.unpaidLeaveDays} days</p>
                    </div>
                  )}
                  {profile.checkIn && (
                    <div>
                      <p className="text-sm text-muted-foreground">Check-In Time</p>
                      <p className="font-medium">{profile.checkIn}</p>
                    </div>
                  )}
                  {profile.checkOut && (
                    <div>
                      <p className="text-sm text-muted-foreground">Check-Out Time</p>
                      <p className="font-medium">{profile.checkOut}</p>
                    </div>
                  )}
                    {profile.weeklyOffDays && (
                    <div>
                      <p className="text-sm text-muted-foreground">Weekly Off Days</p>
                      <p className="font-medium">{profile.weeklyOffDays}</p>
                    </div>
                  )}
                </div>
              </div>

              <Separator />

              {/* Allowances */}
              {(profile.mealAllowance || profile.transportAllowance || profile.housingAllowance || 
                profile.responsibilityAllowance || profile.attendanceBonus || profile.phoneSkillShiftAllowance) && (
                <>
                  <div>
                    <h3 className="text-lg font-semibold mb-3">Allowances</h3>
                    <div className="grid grid-cols-2 gap-4">
                      {profile.mealAllowance && (
                        <div>
                          <p className="text-sm text-muted-foreground">Meal Allowance</p>
                          <p className="font-medium">{profile.mealAllowance.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                      {profile.transportAllowance && (
                        <div>
                          <p className="text-sm text-muted-foreground">Transport Allowance</p>
                          <p className="font-medium">{profile.transportAllowance.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                      {profile.housingAllowance && (
                        <div>
                          <p className="text-sm text-muted-foreground">Housing Allowance</p>
                          <p className="font-medium">{profile.housingAllowance.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                      {profile.responsibilityAllowance && (
                        <div>
                          <p className="text-sm text-muted-foreground">Responsibility Allowance</p>
                          <p className="font-medium">{profile.responsibilityAllowance.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                      {profile.attendanceBonus && (
                        <div>
                          <p className="text-sm text-muted-foreground">Attendance Bonus</p>
                          <p className="font-medium">{profile.attendanceBonus.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                      {profile.phoneSkillShiftAllowance && (
                        <div>
                          <p className="text-sm text-muted-foreground">Phone/Skill/Shift Allowance</p>
                          <p className="font-medium">{profile.phoneSkillShiftAllowance.toLocaleString()} {profile.currency}</p>
                        </div>
                      )}
                    </div>
                  </div>
                  <Separator />
                </>
              )}

              {/* Overtime Configuration */}
              {(profile.otRateWeekday || profile.otRateWeekend || profile.otRateHoliday || profile.nightShiftRate) && (
                <>
                  <div>
                    <h3 className="text-lg font-semibold mb-3">Overtime Configuration</h3>
                    <div className="grid grid-cols-2 gap-4">
                      {profile.otRateWeekday && (
                        <div>
                          <p className="text-sm text-muted-foreground">OT Rate (Weekday)</p>
                          <p className="font-medium">{profile.otRateWeekday}x</p>
                        </div>
                      )}
                      {profile.otRateWeekend && (
                        <div>
                          <p className="text-sm text-muted-foreground">OT Rate (Weekend)</p>
                          <p className="font-medium">{profile.otRateWeekend}x</p>
                        </div>
                      )}
                      {profile.otRateHoliday && (
                        <div>
                          <p className="text-sm text-muted-foreground">OT Rate (Holiday)</p>
                          <p className="font-medium">{profile.otRateHoliday}x</p>
                        </div>
                      )}
                      {profile.nightShiftRate && (
                        <div>
                          <p className="text-sm text-muted-foreground">Night Shift Rate</p>
                          <p className="font-medium">{profile.nightShiftRate}x</p>
                        </div>
                      )}
                    </div>
                  </div>
                  <Separator />
                </>
              )}

              {/* Health Insurance */}
              {profile.hasHealthInsurance && (
                <div>
                  <h3 className="text-lg font-semibold mb-3">Health Insurance</h3>
                  <div className="grid grid-cols-2 gap-4">
                    <div>
                      <p className="text-sm text-muted-foreground">Coverage</p>
                      <Badge variant="default">Included</Badge>
                    </div>
                    {profile.healthInsuranceRate && (
                      <div>
                        <p className="text-sm text-muted-foreground">Insurance Rate</p>
                        <p className="font-medium">{(profile.healthInsuranceRate * 100).toFixed(2)}%</p>
                      </div>
                    )}
                  </div>
                </div>
              )}
            </>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
};
