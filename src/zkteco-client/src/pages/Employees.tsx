// ==========================================
// src/pages/Employees.tsx
// ==========================================
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { UsersTable } from "@/components/users/UsersTable";
import { Plus } from "lucide-react";
import { CreateUserDialog } from "@/components/dialogs/CreateEmployeeDialog";
import { CreateEmployeeAccountDialog } from "@/components/dialogs/CreateEmployeeAccountDialog";
import FilterBar from "@/components/users/FilterBar";
import { EmployeeProvider, useEmployeeContext } from "@/contexts/EmployeeContext";

const EmployeesContent = () => {
  const {
    users,
    isLoading,
    devices,
    selectedDeviceIds,
    createDialogOpen,
    createAccountDialogOpen,
    employeeToEdit,
    employeeForAccount,
    isCreatingAccount,
    isDeletePending,
    setCreateDialogOpen,
    setCreateAccountDialogOpen,
    handleDelete,
    handleAddUser,
    handleUpdateUser,
    handleEdit,
    handleOpenCreateDialog,
    handleCreateAccount,
    handleCreateAccountSubmit,
    handleFilterSubmit,
  } = useEmployeeContext();

  return (
    <div>
      <PageHeader
        title="Employees"
        description="Manage employees and sync them to devices"
        action={
          <Button onClick={handleOpenCreateDialog}>
            <Plus className="w-4 h-4 mr-2" />
            Add Employee
          </Button>
        }
      />

      <FilterBar
        devices={devices}
        handleSubmit={handleFilterSubmit}
        selectedDeviceIds={selectedDeviceIds}
      />

      <UsersTable
        users={users}
        onEdit={handleEdit}
        onDelete={handleDelete}
        isDeletePending={isDeletePending}
        onAddUser={handleOpenCreateDialog}
        isLoading={isLoading}
        onCreateAccount={handleCreateAccount}
      />

      <CreateUserDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        employee={employeeToEdit}
        handleAddUser={handleAddUser}
        handleUpdateUser={handleUpdateUser}
      />

      <CreateEmployeeAccountDialog
        open={createAccountDialogOpen}
        onOpenChange={setCreateAccountDialogOpen}
        employee={employeeForAccount}
        onSubmit={handleCreateAccountSubmit}
        isLoading={isCreatingAccount}
      />
    </div>
  );
};

export const Employees = () => {
  return (
    <EmployeeProvider>
      <EmployeesContent />
    </EmployeeProvider>
  );
};
