// ==========================================
// src/pages/Employees.tsx
// ==========================================
import { PageHeader } from "@/components/PageHeader";
import { Button } from "@/components/ui/button";
import { EmployeesTable } from "@/components/employees";
import { Plus } from "lucide-react";
import { CreateUserDialog } from "@/components/employees/dialogs/EmployeeRequestDialog";
import { EmployeeAccountRequestDialog } from "@/components/dialogs/EmployeeAccountRequestDialog";
import FilterBar from "@/components/employees/FilterBar";
import { EmployeeProvider, useEmployeeContext } from "@/contexts/EmployeeContext";

const EmployeesContent = () => {
  const {
    devices,
    selectedDeviceIds,
    handleOpenCreateDialog,
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

      <EmployeesTable />

      <CreateUserDialog />

      <EmployeeAccountRequestDialog
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
