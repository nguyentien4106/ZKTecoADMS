import { useState } from 'react';
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
import { CreateSalaryProfileRequest, SalaryRateType } from "@/services/salaryProfileService";

interface CreateSalaryProfileDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: CreateSalaryProfileRequest) => Promise<void>;
}

const defaultFormData: CreateSalaryProfileRequest = {
  name: '',
  description: '',
  rateType: SalaryRateType.Hourly,
  rate: 0,
  currency: 'VND',
  overtimeMultiplier: 1.5,
  holidayMultiplier: 2.0,
  nightShiftMultiplier: 1.3,
};

export const CreateSalaryProfileDialog = ({
  open,
  onOpenChange,
  onSubmit,
}: CreateSalaryProfileDialogProps) => {
  const [formData, setFormData] = useState<CreateSalaryProfileRequest>(defaultFormData);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async () => {
    setIsSubmitting(true);
    try {
      await onSubmit(formData);
      setFormData(defaultFormData);
      onOpenChange(false);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setFormData(defaultFormData);
    onOpenChange(false);
  };

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Create Salary Profile</DialogTitle>
          <DialogDescription>
            Create a new salary profile for employees
          </DialogDescription>
        </DialogHeader>

        <SalaryProfileForm
          formData={formData}
          onChange={setFormData}
        />

        <DialogFooter>
          <Button variant="outline" onClick={handleClose} disabled={isSubmitting}>
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!formData.name || !formData.rate || isSubmitting}
          >
            {isSubmitting ? 'Creating...' : 'Create Profile'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
