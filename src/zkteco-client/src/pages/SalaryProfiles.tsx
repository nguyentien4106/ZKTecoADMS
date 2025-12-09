import { useState, useEffect } from 'react';
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { Plus, Edit, Trash2 } from "lucide-react";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
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
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { Badge } from "@/components/ui/badge";
import { Card } from "@/components/ui/card";
import { toast } from "sonner";
import salaryProfileService, {
  SalaryProfile,
  SalaryRateType,
  CreateSalaryProfileRequest,
  UpdateSalaryProfileRequest,
} from "@/services/salaryProfileService";

export const SalaryProfiles = () => {
  const [profiles, setProfiles] = useState<SalaryProfile[]>([]);
  const [loading, setLoading] = useState(true);
  const [isCreateDialogOpen, setIsCreateDialogOpen] = useState(false);
  const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
  const [selectedProfile, setSelectedProfile] = useState<SalaryProfile | null>(null);
  const [showActiveOnly, setShowActiveOnly] = useState(false);

  const [formData, setFormData] = useState<CreateSalaryProfileRequest>({
    name: '',
    description: '',
    rateType: SalaryRateType.Monthly,
    rate: 0,
    currency: 'USD',
    overtimeMultiplier: 1.5,
    holidayMultiplier: 2.0,
    nightShiftMultiplier: 1.3,
  });

  const [editFormData, setEditFormData] = useState<UpdateSalaryProfileRequest>({
    name: '',
    description: '',
    rateType: SalaryRateType.Monthly,
    rate: 0,
    currency: 'USD',
    overtimeMultiplier: 1.5,
    holidayMultiplier: 2.0,
    nightShiftMultiplier: 1.3,
    isActive: true,
  });

  useEffect(() => {
    loadProfiles();
  }, [showActiveOnly]);

  const loadProfiles = async () => {
    try {
      setLoading(true);
      const data = await salaryProfileService.getAllProfiles(showActiveOnly);
      setProfiles(data);
    } catch (error) {
      toast.error('Failed to load salary profiles');
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = async () => {
    try {
      await salaryProfileService.createProfile(formData);
      toast.success('Salary profile created successfully');
      setIsCreateDialogOpen(false);
      resetForm();
      loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to create salary profile');
    }
  };

  const handleEdit = async () => {
    if (!selectedProfile) return;

    try {
      await salaryProfileService.updateProfile(selectedProfile.id, editFormData);
      toast.success('Salary profile updated successfully');
      setIsEditDialogOpen(false);
      setSelectedProfile(null);
      loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to update salary profile');
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Are you sure you want to delete this salary profile?')) return;

    try {
      await salaryProfileService.deleteProfile(id);
      toast.success('Salary profile deleted successfully');
      loadProfiles();
    } catch (error: any) {
      toast.error(error.message || 'Failed to delete salary profile');
    }
  };

  const openEditDialog = (profile: SalaryProfile) => {
    setSelectedProfile(profile);
    setEditFormData({
      name: profile.name,
      description: profile.description || '',
      rateType: profile.rateType,
      rate: profile.rate,
      currency: profile.currency,
      overtimeMultiplier: profile.overtimeMultiplier,
      holidayMultiplier: profile.holidayMultiplier,
      nightShiftMultiplier: profile.nightShiftMultiplier,
      isActive: profile.isActive,
    });
    setIsEditDialogOpen(true);
  };

  const resetForm = () => {
    setFormData({
      name: '',
      description: '',
      rateType: SalaryRateType.Monthly,
      rate: 0,
      currency: 'USD',
      overtimeMultiplier: 1.5,
      holidayMultiplier: 2.0,
      nightShiftMultiplier: 1.3,
    });
  };

  const getRateTypeLabel = (type: SalaryRateType) => {
    switch (type) {
      case SalaryRateType.Hourly: return 'Hourly';
      case SalaryRateType.Daily: return 'Daily';
      case SalaryRateType.Monthly: return 'Monthly';
      case SalaryRateType.Yearly: return 'Yearly';
      default: return 'Unknown';
    }
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

        {loading ? (
          <div className="text-center py-8">Loading...</div>
        ) : (
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Name</TableHead>
                <TableHead>Rate Type</TableHead>
                <TableHead>Rate</TableHead>
                <TableHead>Currency</TableHead>
                <TableHead>OT Multiplier</TableHead>
                <TableHead>Status</TableHead>
                <TableHead className="text-right">Actions</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {profiles.length === 0 ? (
                <TableRow>
                  <TableCell colSpan={7} className="text-center py-8 text-muted-foreground">
                    No salary profiles found
                  </TableCell>
                </TableRow>
              ) : (
                profiles.map((profile) => (
                  <TableRow key={profile.id}>
                    <TableCell className="font-medium">{profile.name}</TableCell>
                    <TableCell>{getRateTypeLabel(profile.rateType)}</TableCell>
                    <TableCell>{profile.rate.toLocaleString()}</TableCell>
                    <TableCell>{profile.currency}</TableCell>
                    <TableCell>{profile.overtimeMultiplier ? `${profile.overtimeMultiplier}x` : 'N/A'}</TableCell>
                    <TableCell>
                      <Badge variant={profile.isActive ? "default" : "secondary"}>
                        {profile.isActive ? 'Active' : 'Inactive'}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => openEditDialog(profile)}
                        >
                          <Edit className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => handleDelete(profile.id)}
                        >
                          <Trash2 className="w-4 h-4" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))
              )}
            </TableBody>
          </Table>
        )}
      </Card>

      {/* Create Dialog */}
      <Dialog open={isCreateDialogOpen} onOpenChange={setIsCreateDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Create Salary Profile</DialogTitle>
            <DialogDescription>
              Create a new salary profile for employees
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="name">Profile Name *</Label>
              <Input
                id="name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                placeholder="e.g., Senior Developer Rate"
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="description">Description</Label>
              <Textarea
                id="description"
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                placeholder="Optional description"
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="rateType">Rate Type *</Label>
                <Select
                  value={formData.rateType.toString()}
                  onValueChange={(value) => setFormData({ ...formData, rateType: parseInt(value) as SalaryRateType })}
                >
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value={SalaryRateType.Hourly.toString()}>Hourly</SelectItem>
                    <SelectItem value={SalaryRateType.Daily.toString()}>Daily</SelectItem>
                    <SelectItem value={SalaryRateType.Monthly.toString()}>Monthly</SelectItem>
                    <SelectItem value={SalaryRateType.Yearly.toString()}>Yearly</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="grid gap-2">
                <Label htmlFor="rate">Rate *</Label>
                <Input
                  id="rate"
                  type="number"
                  value={formData.rate}
                  onChange={(e) => setFormData({ ...formData, rate: parseFloat(e.target.value) || 0 })}
                  placeholder="0.00"
                  step="0.01"
                />
              </div>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="currency">Currency *</Label>
              <Input
                id="currency"
                value={formData.currency}
                onChange={(e) => setFormData({ ...formData, currency: e.target.value })}
                placeholder="USD"
                maxLength={10}
              />
            </div>

            <div className="grid grid-cols-3 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="overtime">Overtime Multiplier</Label>
                <Input
                  id="overtime"
                  type="number"
                  value={formData.overtimeMultiplier || ''}
                  onChange={(e) => setFormData({ ...formData, overtimeMultiplier: parseFloat(e.target.value) || undefined })}
                  placeholder="1.5"
                  step="0.1"
                />
              </div>

              <div className="grid gap-2">
                <Label htmlFor="holiday">Holiday Multiplier</Label>
                <Input
                  id="holiday"
                  type="number"
                  value={formData.holidayMultiplier || ''}
                  onChange={(e) => setFormData({ ...formData, holidayMultiplier: parseFloat(e.target.value) || undefined })}
                  placeholder="2.0"
                  step="0.1"
                />
              </div>

              <div className="grid gap-2">
                <Label htmlFor="nightshift">Night Shift Multiplier</Label>
                <Input
                  id="nightshift"
                  type="number"
                  value={formData.nightShiftMultiplier || ''}
                  onChange={(e) => setFormData({ ...formData, nightShiftMultiplier: parseFloat(e.target.value) || undefined })}
                  placeholder="1.3"
                  step="0.1"
                />
              </div>
            </div>
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setIsCreateDialogOpen(false)}>
              Cancel
            </Button>
            <Button onClick={handleCreate} disabled={!formData.name || !formData.rate}>
              Create Profile
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>

      {/* Edit Dialog */}
      <Dialog open={isEditDialogOpen} onOpenChange={setIsEditDialogOpen}>
        <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle>Edit Salary Profile</DialogTitle>
            <DialogDescription>
              Update salary profile information
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid gap-2">
              <Label htmlFor="edit-name">Profile Name *</Label>
              <Input
                id="edit-name"
                value={editFormData.name}
                onChange={(e) => setEditFormData({ ...editFormData, name: e.target.value })}
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="edit-description">Description</Label>
              <Textarea
                id="edit-description"
                value={editFormData.description}
                onChange={(e) => setEditFormData({ ...editFormData, description: e.target.value })}
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="edit-rateType">Rate Type *</Label>
                <Select
                  value={editFormData.rateType.toString()}
                  onValueChange={(value) => setEditFormData({ ...editFormData, rateType: parseInt(value) as SalaryRateType })}
                >
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value={SalaryRateType.Hourly.toString()}>Hourly</SelectItem>
                    <SelectItem value={SalaryRateType.Daily.toString()}>Daily</SelectItem>
                    <SelectItem value={SalaryRateType.Monthly.toString()}>Monthly</SelectItem>
                    <SelectItem value={SalaryRateType.Yearly.toString()}>Yearly</SelectItem>
                  </SelectContent>
                </Select>
              </div>

              <div className="grid gap-2">
                <Label htmlFor="edit-rate">Rate *</Label>
                <Input
                  id="edit-rate"
                  type="number"
                  value={editFormData.rate}
                  onChange={(e) => setEditFormData({ ...editFormData, rate: parseFloat(e.target.value) || 0 })}
                  step="0.01"
                />
              </div>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="edit-currency">Currency *</Label>
              <Input
                id="edit-currency"
                value={editFormData.currency}
                onChange={(e) => setEditFormData({ ...editFormData, currency: e.target.value })}
                maxLength={10}
              />
            </div>

            <div className="grid grid-cols-3 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="edit-overtime">Overtime Multiplier</Label>
                <Input
                  id="edit-overtime"
                  type="number"
                  value={editFormData.overtimeMultiplier || ''}
                  onChange={(e) => setEditFormData({ ...editFormData, overtimeMultiplier: parseFloat(e.target.value) || undefined })}
                  step="0.1"
                />
              </div>

              <div className="grid gap-2">
                <Label htmlFor="edit-holiday">Holiday Multiplier</Label>
                <Input
                  id="edit-holiday"
                  type="number"
                  value={editFormData.holidayMultiplier || ''}
                  onChange={(e) => setEditFormData({ ...editFormData, holidayMultiplier: parseFloat(e.target.value) || undefined })}
                  step="0.1"
                />
              </div>

              <div className="grid gap-2">
                <Label htmlFor="edit-nightshift">Night Shift Multiplier</Label>
                <Input
                  id="edit-nightshift"
                  type="number"
                  value={editFormData.nightShiftMultiplier || ''}
                  onChange={(e) => setEditFormData({ ...editFormData, nightShiftMultiplier: parseFloat(e.target.value) || undefined })}
                  step="0.1"
                />
              </div>
            </div>

            <div className="flex items-center space-x-2">
              <input
                type="checkbox"
                id="edit-active"
                checked={editFormData.isActive}
                onChange={(e) => setEditFormData({ ...editFormData, isActive: e.target.checked })}
                className="h-4 w-4"
              />
              <Label htmlFor="edit-active">Active</Label>
            </div>
          </div>

          <DialogFooter>
            <Button variant="outline" onClick={() => setIsEditDialogOpen(false)}>
              Cancel
            </Button>
            <Button onClick={handleEdit} disabled={!editFormData.name || !editFormData.rate}>
              Update Profile
            </Button>
          </DialogFooter>
        </DialogContent>
      </Dialog>
    </div>
  );
};
