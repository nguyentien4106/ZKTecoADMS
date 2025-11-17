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
    employees,
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
    handleAddEmployee: handleAddUser,
    handleUpdateEmployee: handleUpdateUser,
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

      <UsersTable />

      <CreateUserDialog />

      <CreateEmployeeAccountDialog
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
