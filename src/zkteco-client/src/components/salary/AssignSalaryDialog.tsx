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
import salaryProfileService, {
  SalaryProfile,
  AssignSalaryProfileRequest,
} from "@/services/salaryProfileService";

interface AssignSalaryDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  employeeId: string;
  employeeName: string;
  onSuccess?: () => void;
}

export const AssignSalaryDialog = ({
  open,
  onOpenChange,
  employeeId,
  employeeName,
  onSuccess,
}: AssignSalaryDialogProps) => {
  const [profiles, setProfiles] = useState<SalaryProfile[]>([]);
  const [loading, setLoading] = useState(false);
  const [submitting, setSubmitting] = useState(false);

  const [formData, setFormData] = useState<AssignSalaryProfileRequest>({
    employeeId,
    salaryProfileId: '',
    effectiveDate: new Date().toISOString().split('T')[0],
    notes: '',
  });

  useEffect(() => {
    if (open) {
      loadProfiles();
      setFormData({
        employeeId,
        salaryProfileId: '',
        effectiveDate: new Date().toISOString().split('T')[0],
        notes: '',
      });
    }
  }, [open, employeeId]);

  const loadProfiles = async () => {
    try {
      setLoading(true);
      const data = await salaryProfileService.getAllProfiles(true);
      setProfiles(data);
    } catch (error) {
      toast.error('Failed to load salary profiles');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    if (!formData.salaryProfileId) {
      toast.error('Please select a salary profile');
      return;
    }

    try {
      setSubmitting(true);
      await salaryProfileService.assignProfile(formData);
      toast.success(`Salary profile assigned to ${employeeName} successfully`);
      onOpenChange(false);
      if (onSuccess) onSuccess();
    } catch (error: any) {
      toast.error(error.message || 'Failed to assign salary profile');
    } finally {
      setSubmitting(false);
    }
  };

  const selectedProfile = profiles.find(p => p.id === formData.salaryProfileId);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-lg">
        <DialogHeader>
          <DialogTitle>Assign Salary Profile</DialogTitle>
          <DialogDescription>
            Assign a salary profile to {employeeName}
          </DialogDescription>
        </DialogHeader>

        <div className="grid gap-4 py-4">
          <div className="grid gap-2">
            <Label htmlFor="profile">Salary Profile *</Label>
            <Select
              value={formData.salaryProfileId}
              onValueChange={(value) => setFormData({ ...formData, salaryProfileId: value })}
              disabled={loading}
            >
              <SelectTrigger id="profile">
                <SelectValue placeholder="Select a salary profile" />
              </SelectTrigger>
              <SelectContent>
                {profiles.map((profile) => (
                  <SelectItem key={profile.id} value={profile.id}>
                    {profile.name} - {profile.rate} {profile.currency}/{profile.rateTypeName}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {selectedProfile && (
            <div className="bg-muted p-3 rounded-md">
              <h4 className="font-medium text-sm mb-2">Profile Details</h4>
              <div className="text-sm space-y-1">
                <div>Rate: {selectedProfile.rate} {selectedProfile.currency}</div>
                <div>Type: {selectedProfile.rateTypeName}</div>
                {selectedProfile.overtimeMultiplier && (
                  <div>Overtime: {selectedProfile.overtimeMultiplier}x</div>
                )}
                {selectedProfile.holidayMultiplier && (
                  <div>Holiday: {selectedProfile.holidayMultiplier}x</div>
                )}
                {selectedProfile.nightShiftMultiplier && (
                  <div>Night Shift: {selectedProfile.nightShiftMultiplier}x</div>
                )}
              </div>
            </div>
          )}

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
          <Button variant="outline" onClick={() => onOpenChange(false)} disabled={submitting}>
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!formData.salaryProfileId || submitting}
          >
            {submitting ? 'Assigning...' : 'Assign Profile'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
