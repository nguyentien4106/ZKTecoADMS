import { useState, useEffect } from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "sonner";
import {
  AssignSalaryProfileRequest,
  SalaryRateType,
} from "@/services/salaryProfileService";
import { useSalaryProfileContext } from "@/contexts/SalaryProfileContext";
import { useEmployees } from "@/hooks/useEmployee";
import { format } from 'date-fns';
import { useAssignSalaryProfile } from '@/hooks/useSalaryProfiles';

const defaultFormData: AssignSalaryProfileRequest = {
  employeeId: '',
  salaryProfileId: '',
  effectiveDate: format(new Date(), 'yyyy-MM-dd'),
  notes: '',
};

export const AssignSalaryToEmployeeDialog = () => {
  const { assignDialogOpen, setAssignDialogOpen, profileToAssign } = useSalaryProfileContext();
  const { data: employeesData } = useEmployees({ pageSize: 1000, employmentType: profileToAssign?.rateType == SalaryRateType.Hourly ? '0' : '1' });
  const [submitting, setSubmitting] = useState(false);
  const [formData, setFormData] = useState<AssignSalaryProfileRequest>(defaultFormData);
  const assignSalaryProfile = useAssignSalaryProfile()

  useEffect(() => {
    if (assignDialogOpen && profileToAssign) {
      setFormData({...defaultFormData, salaryProfileId: profileToAssign.id});
    }
  }, [assignDialogOpen, profileToAssign]);

  const handleSubmit = async () => {
    if (!formData.employeeId) {
      toast.error('Please select an employee');
      return;
    }

    try {
      setSubmitting(true);
      await assignSalaryProfile.mutateAsync(formData);
      setAssignDialogOpen(false);
    } catch (error: any) {
      toast.error(error.message || 'Failed to assign salary profile');
    } finally {
      setSubmitting(false);
    }
  };

  const handleClose = () => {
    setFormData(defaultFormData);
    setAssignDialogOpen(false);
  };    

  return (
    <Dialog open={assignDialogOpen} onOpenChange={handleClose}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>Assign Salary Profile</DialogTitle>
          <DialogDescription>
            Assign "{profileToAssign?.name}" to an employee
          </DialogDescription>
        </DialogHeader>

        <div className="grid gap-4 py-4">
          {profileToAssign && (
            <div className="bg-muted p-3 rounded-md">
              <h4 className="font-medium text-sm mb-2">Profile Details</h4>
              <div className="text-sm space-y-1">
                <div>Name: {profileToAssign.name}</div>
                <div>Rate: {profileToAssign.rate} {profileToAssign.currency}</div>
                <div>Type: {profileToAssign.rateTypeName}</div>
                {profileToAssign.overtimeMultiplier && (
                  <div>Overtime: {profileToAssign.overtimeMultiplier}x</div>
                )}
                {profileToAssign.holidayMultiplier && (
                  <div>Holiday: {profileToAssign.holidayMultiplier}x</div>
                )}
                {profileToAssign.nightShiftMultiplier && (
                  <div>Night Shift: {profileToAssign.nightShiftMultiplier}x</div>
                )}
              </div>
            </div>
          )}

          <div className="grid gap-2">
            <Label htmlFor="employee">Employee *</Label>
            <Select
              value={formData.employeeId}
              onValueChange={(value) => setFormData({ ...formData, employeeId: value })}
            >
              <SelectTrigger id="employee">
                <SelectValue placeholder="Select an employee" />
              </SelectTrigger>
              <SelectContent>
                {employeesData?.items?.map((employee) => (
                  <SelectItem key={employee.id} value={employee.id}>
                    {employee.fullName} - {employee.employeeCode}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          <div className="grid gap-2">
            <Label htmlFor="effectiveDate">Effective Date *</Label>
            <Input
              id="effectiveDate"
              type="date"
              value={formData.effectiveDate}
              onChange={(e) => setFormData({ ...formData, effectiveDate: e.target.value })}
            />
          </div>

          <div className="grid gap-2">
            <Label htmlFor="notes">Notes</Label>
            <Textarea
              id="notes"
              value={formData.notes}
              onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
              placeholder="Optional notes about this assignment"
              rows={3}
            />
          </div>
        </div>

        <DialogFooter>
          <Button variant="outline" onClick={handleClose} disabled={submitting}>
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!formData.employeeId || submitting}
          >
            {submitting ? 'Assigning...' : 'Assign Profile'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
