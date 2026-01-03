// ==========================================
// src/components/employees/EmployeeInfoTable.tsx
// ==========================================
import { Employee } from "@/types/employee";
import { ColumnDef } from "@tanstack/react-table";
import { format } from "date-fns";
import { EmployeeActions } from "./EmployeeActions";
import { EmployeeStatusBadge } from "./EmployeeStatusBadge";
import { DeleteEmployeeDialog } from "./dialogs/DeleteEmployeeDialog";
import { useEmployeeContext } from "@/contexts/EmployeeContext";
import { EmploymentTypes, HourlyEmployeeId } from "@/constants";
import { SortingHeader } from "../tables/SortingHeader";
import { Table } from "../tables/Table";
import { Badge } from "../ui/badge";

export const EmployeeTable = () => {
  const {
    employees,
    isLoading,
    isDeletePending,
    deleteDialogOpen,
    employeeToDelete,
    handleConfirmDelete,
    setDeleteDialogOpen,
  } = useEmployeeContext();

  const columns: ColumnDef<Employee>[] = [
    {
      accessorKey: "employeeCode",
      header: ({ column }) => <SortingHeader column={column} title="Employee Code" />,
      cell: ({ row }) => (
        <span className="font-medium text-center block">{row.getValue("employeeCode")}</span>
      ),
      enableSorting: true,
    },
    {
      accessorKey: "fullName",
      header: ({ column }) => <SortingHeader column={column} title="Full Name" />,
      enableSorting: true,
    },
    {
      accessorKey: "department",
      header: "Department",
      cell: ({ row }) => row.getValue("department") || "-",
    },
    {
      accessorKey: "employmentType",
      header: "Employment Type",
      cell: ({ row }) => {
        const empType = row.getValue("employmentType") as keyof typeof EmploymentTypes;
        const empTypeLabel = EmploymentTypes[empType] || empType;
        return <Badge className="capitalize" variant={empType == 0 ? "default" : "outline"}>{empTypeLabel}</Badge>;
      },
    },
    {
      accessorKey: "workStatus",
      header: ({ column }) => <SortingHeader column={column} title="Work Status" />,
      cell: ({ row }) => (
        <EmployeeStatusBadge status={row.getValue("workStatus")} />
      ),
      enableSorting: true,
    },
    {
      accessorKey: "companyEmail",
      header: "Company Email",
      cell: ({ row }) => row.getValue("companyEmail") || "-",
    },
    {
      accessorKey: "joinDate",
      header: ({ column }) => <SortingHeader column={column} title="Join Date" />,
      cell: ({ row }) => {
        const joinDate = row.getValue("joinDate") as string | undefined;
        return joinDate ? format(new Date(joinDate), "MMM dd, yyyy") : "-";
      },
      enableSorting: true,
    },
    {
      id: "setup",
      header: "Configuration Status",
      cell: ({ row }) => {
        const hasAccount = row.original.hasAccount;
        const hasDeviceUsers = row.original.hasDeviceUsers;
        const hasBenefits = row.original.hasBenefits;
        
        return (
          <div className="flex flex-col gap-1">
            {
              hasAccount === false ? (
                <span className="text-sm text-red-500">No Account</span>
              ) : null
            }
            {
              hasDeviceUsers === false ? (
                <span className="text-sm text-red-500">No Device Users</span>
              ) : null
            }
            {
              hasBenefits === false ? (
                <span className="text-sm text-red-500">No Benefits</span>
              ) : null
            }
          </div>
        );
      },
      enableSorting: true,
    },
    {
      id: "actions",
      header: "Actions",
      cell: ({ row }) => (
        <EmployeeActions
          employee={row.original}
        />
      ),
      enableSorting: false,
    },
  ];

  if (!employees) {
    return null;
  }

  return (
    <>
      <Table
        columns={columns}
        data={employees || []}
        isLoading={isLoading}
        enableSorting={true}
        emptyMessage="No employees found"
        containerHeight="calc(100vh - 320px)"
      />
      <DeleteEmployeeDialog
        open={deleteDialogOpen}
        employee={employeeToDelete}
        isDeleting={isDeletePending}
        onOpenChange={setDeleteDialogOpen}
        onConfirm={handleConfirmDelete}
      />
    </>
  );
};
