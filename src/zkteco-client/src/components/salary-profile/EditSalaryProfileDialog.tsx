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
import { UpdateSalaryProfileRequest } from "@/services/salaryProfileService";
import { useSalaryProfileContext } from "@/contexts/SalaryProfileContext";

export const EditSalaryProfileDialog = () => {
  const { editDialogOpen, setEditDialogOpen, profileToEdit, handleUpdateProfile, isUpdatePending } = useSalaryProfileContext();
  const [formData, setFormData] = useState<UpdateSalaryProfileRequest>({ ...profileToEdit } as UpdateSalaryProfileRequest);

  useEffect(() => {
    if (profileToEdit) {
      setFormData({ ...profileToEdit });
    }
  }, [profileToEdit]);

  const handleSubmit = async () => {
    await handleUpdateProfile(formData);
  };

  return (
    <Dialog open={editDialogOpen} onOpenChange={setEditDialogOpen}>
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
          <Button variant="outline" onClick={() => setEditDialogOpen(false)} disabled={isUpdatePending}>
            Cancel
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={!formData.name || !formData.rate || isUpdatePending}
          >
            {isUpdatePending ? 'Updating...' : 'Update Profile'}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
};
