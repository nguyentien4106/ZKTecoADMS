import { useState } from 'react';
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus } from "lucide-react";
import { Label } from "@/components/ui/label";
import { Badge } from "@/components/ui/badge";
import { Card } from "@/components/ui/card";
import { SalaryProfileTable } from "@/components/salary/SalaryProfileTable";
import { CreateSalaryProfileDialog } from "@/components/salary/CreateSalaryProfileDialog";
import { EditSalaryProfileDialog } from "@/components/salary/EditSalaryProfileDialog";
import { useSalaryProfiles } from "@/hooks/useSalaryProfiles";
import { SalaryProfile } from "@/services/salaryProfileService";

export const SalaryProfiles = () => {
  const [showActiveOnly, setShowActiveOnly] = useState(false);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [selectedProfile, setSelectedProfile] = useState<SalaryProfile | null>(null);

  const {
    profiles,
    loading,
    createProfile,
    updateProfile,
    deleteProfile,
  } = useSalaryProfiles(showActiveOnly);

  const handleEdit = (profile: SalaryProfile) => {
    setSelectedProfile(profile);
    setIsEditDialogOpen(true);
  };

  const handleEditDialogClose = () => {
    setIsEditDialogOpen(false);
    setSelectedProfile(null);
  };

  return (
    <div className="space-y-6">
      <PageHeader
        title="Salary Profiles"
        description="Manage salary profiles and rates"
        action={
          <Button onClick={() => setIsCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Create Profile
          </Button>
        }
      />

      <Card className="p-4">
        <div className="flex items-center justify-between mb-4">
          <div className="flex items-center space-x-2">
            <input
              type="checkbox"
              checked={showActiveOnly}
              onChange={(e) => setShowActiveOnly(e.target.checked)}
              id="active-only"
              className="h-4 w-4"
            />
            <Label htmlFor="active-only">Show active only</Label>
          </div>
          <Badge variant="outline" className="text-sm">
            {profiles.length} profile{profiles.length !== 1 ? 's' : ''}
          </Badge>
        </div>

        <SalaryProfileTable
          profiles={profiles}
          loading={loading}
          onEdit={handleEdit}
          onDelete={deleteProfile}
        />
      </Card>

      <CreateSalaryProfileDialog
        open={isCreateDialogOpen}
        onOpenChange={setIsCreateDialogOpen}
        onSubmit={createProfile}
      />

      <EditSalaryProfileDialog
        open={isEditDialogOpen}
        profile={selectedProfile}
        onOpenChange={handleEditDialogClose}
        onSubmit={updateProfile}
      />
    </div>
  );
};
