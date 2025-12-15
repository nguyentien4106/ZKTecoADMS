import { useState, useEffect } from 'react';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { SalaryProfileForm } from './SalaryProfileForm';
import { SalaryProfile, UpdateSalaryProfileRequest } from "@/services/salaryProfileService";

interface EditSalaryProfileDialogProps {
  open: boolean;
  profile: SalaryProfile | null;
  onOpenChange: (open: boolean) => void;
  onSubmit: (id: string, data: UpdateSalaryProfileRequest) => Promise<void>;
}

export const EditSalaryProfileDialog = ({
  open,
  profile,
  onOpenChange,
  onSubmit,
}: EditSalaryProfileDialogProps) => {
  const [formData, setFormData] = useState<UpdateSalaryProfileRequest>({ ...profile } as UpdateSalaryProfileRequest);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (profile) {
      setFormData({ ...profile });
    }
  }, [profile]);

  const handleSubmit = async () => {
    if (!profile) return;

    setIsSubmitting(true);
    try {
      await onSubmit(profile.id, formData);
      onOpenChange(false);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Edit Salary Profile</DialogTitle>
          <DialogDescription>
            Update salary profile information
          </DialogDescription>
        </DialogHeader>

        <SalaryProfileForm
          formData={formData as any}
          onChange={(data) => setFormData({ ...formData, ...data })}
          showActiveToggle
        />

        <DialogFooter>
          <Button variant="outline" onClick={() => onOpenChange(false)} disabled={isSubmitting}>
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!formData.name || !formData.rate || isSubmitting}
          >
            {isSubmitting ? 'Updating...' : 'Update Profile'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
