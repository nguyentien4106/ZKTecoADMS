// ==========================================
// src/pages/Users.tsx
// ==========================================
import { useEffect, useState } from "react";
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { UsersTable } from "@/components/users/UsersTable";
import {
  useUsers,
  useDeleteUser,
  useCreateUser,
  useUpdateUser,
} from "@/hooks/useUsers";
import {
  Plus,
} from "lucide-react";
import { CreateUserDialog } from "@/components/dialogs/CreateUserDialog";
import { toast } from "sonner";
import { User } from "@/types/user";
import FilterBar from "@/components/users/FilterBar";
import { useDevices } from "@/hooks/useDevices";
import { CreateUserRequest, UpdateUserRequest } from "@/types/user";

export const Users = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [userToEdit, setUserToEdit] = useState<User | null>(null);
  const { data: devices, isFetching: devicesFetching } = useDevices();
  const [selectedDeviceIds, setSelectedDeviceIds] = useState<string[]>([]);
  const deleteUser = useDeleteUser();
  const { data: users, isLoading } = useUsers(selectedDeviceIds);
  const createUser = useCreateUser();
  const updateUser = useUpdateUser();

  const handleDelete = async (userId: string) => {
    if (!userId) return;
    if (confirm("Are you sure you want to delete this user?")) {
      await deleteUser.mutateAsync(userId);
    }
  };

  useEffect(() => {
    setSelectedDeviceIds(devices ? devices.items.map((device) => device.id) : []);
  }, [devices]);

  const handleAddUser = async (data: CreateUserRequest) => {
    const results = await createUser.mutateAsync(data as CreateUserRequest);
    const successes = results.filter((res) => res.isSuccess);
    const errors = results.filter((res) => !res.isSuccess);

    if (successes && successes.length) {
      toast.success("User added successfully to devices.", {
        description: successes.map((item) => item.data.deviceName).join(", "),
      });
    }
    if (errors && errors.length) {
      toast.error("User added failed to devices.", {
        description: errors.map((item) => item.data.deviceName).join(", "),
      });
    }
  };

  const handleUpdateUser = async (data: UpdateUserRequest) => {
    try {
      await updateUser.mutateAsync(data);
      toast.success("User updated successfully");
    } catch (error: any) {
      toast.error("Failed to update user", {
        description: error.message || "An error occurred",
      });
    }
  };

  const handleEdit = (user: User) => {
    setUserToEdit(user);
    setCreateDialogOpen(true);
  };

  const handleSubmit = (deviceIds: string[]) => {
    setSelectedDeviceIds(deviceIds);
  };

  return (
    <div>
      <PageHeader
        title="Users"
        description="Manage users and sync them to devices"
        action={
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Add User
          </Button>
        }
      />

      <FilterBar
        devices={devices?.items}
        handleSubmit={handleSubmit}
        selectedDeviceIds={selectedDeviceIds}
      />

      <UsersTable
        users={users}
        onEdit={handleEdit}
        onDelete={handleDelete}
        isDeletePending={deleteUser.isPending}
        onAddUser={() => setCreateDialogOpen(true)}
        isLoading={isLoading}
      />

      <CreateUserDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        user={userToEdit}
        handleAddUser={handleAddUser}
        handleUpdateUser={handleUpdateUser}
      />
    </div>
  );
};
