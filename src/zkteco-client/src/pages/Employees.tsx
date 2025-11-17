// ==========================================
// src/pages/Employees.tsx
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
import { CreateUserAccountDialog } from "@/components/dialogs/CreateUserAccountDialog";
import { toast } from "sonner";
import FilterBar from "@/components/users/FilterBar";
import { useDevices } from "@/hooks/useDevices";
import { CreateEmployeeRequest, UpdateEmployeeRequest, Employee } from "@/types/employee";
import { accountService } from "@/services/accountService";

export const Employees = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false);
  const [createAccountDialogOpen, setCreateAccountDialogOpen] = useState(false);
  const [userToEdit, setUserToEdit] = useState<Employee | null>(null);
  const [userForAccount, setUserForAccount] = useState<Employee | null>(null);
  const [isCreatingAccount, setIsCreatingAccount] = useState(false);
  const { data: devices } = useDevices();
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

  const handleAddUser = async (data: CreateEmployeeRequest) => {
    const results = await createUser.mutateAsync(data as CreateEmployeeRequest);
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

  const handleUpdateUser = async (data: UpdateEmployeeRequest) => {
    try {
      await updateUser.mutateAsync(data);
      toast.success("User updated successfully");
    } catch (error: any) {
      toast.error("Failed to update user", {
        description: error.message || "An error occurred",
      });
    }
  };

  const handleEdit = (user: Employee) => {
    setUserToEdit(user);
    setCreateDialogOpen(true);
  };

  const handleSubmit = (deviceIds: string[]) => {
    setSelectedDeviceIds(deviceIds);
  };

  const handleCreateAccount = (user: Employee) => {
    setUserForAccount(user);
    setCreateAccountDialogOpen(true);
  };

  const handleCreateAccountSubmit = async (
    userId: string, 
    firstName: string, 
    lastName: string, 
    email: string, 
    password: string,
    phoneNumber?: string
  ) => {
    setIsCreatingAccount(true);
    try {
      // Check if the user already has an account
      const isUpdateMode = !!userForAccount?.applicationUser;
      
      if (isUpdateMode) {
        // Update existing account
        await accountService.updateUserAccount(
          userId, 
          firstName, 
          lastName, 
          email, 
          phoneNumber,
          password // Password is optional in update mode
        );
        toast.success("Account updated successfully", {
          description: `Login account updated for ${userForAccount?.name}`,
        });
      } else {
        // Create new account
        await accountService.createUserAccount(
          userId, 
          firstName, 
          lastName, 
          email, 
          password, 
          phoneNumber
        );
        toast.success("Account created successfully", {
          description: `Login account created for ${userForAccount?.name}`,
        });
      }
      
      setCreateAccountDialogOpen(false);
      setUserForAccount(null);
    } catch (error: any) {
      const action = userForAccount?.applicationUser ? "update" : "create";
      toast.error(`Failed to ${action} account`, {
        description: error.message || "An error occurred",
      });
    } finally {
      setIsCreatingAccount(false);
    }
  };

  return (
    <div>
      <PageHeader
        title="Employees"
        description="Manage employees and sync them to devices"
        action={
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Add Employee
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
        onCreateAccount={handleCreateAccount}
      />

      <CreateUserDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        user={userToEdit}
        handleAddUser={handleAddUser}
        handleUpdateUser={handleUpdateUser}
      />

      <CreateUserAccountDialog
        open={createAccountDialogOpen}
        onOpenChange={setCreateAccountDialogOpen}
        user={userForAccount}
        onSubmit={handleCreateAccountSubmit}
        isLoading={isCreatingAccount}
      />
    </div>
  );
};
