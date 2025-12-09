import { Card, CardContent } from "@/components/ui/card";
import { Table, TableBody } from "@/components/ui/table";
import { useEmployeeContext } from "@/contexts/EmployeeContext";
import { useQueryClient } from "@tanstack/react-query";
import { EmployeeTableHeader } from "./EmployeeTableHeader";
import { EmployeeTableLoading } from "./EmployeeTableLoading";
import { EmployeeTableEmpty } from "./EmployeeTableEmpty";
import { EmployeeTableRow } from "./EmployeeTableRow";
import { DeleteEmployeeDialog } from "./DeleteEmployeeDialog";
import { AssignSalaryDialog } from "../salary/AssignSalaryDialog";
import { Employee } from "@/types/employee";

export const EmployeesTable = () => {
  const queryClient = useQueryClient();
  const {
    employees,
    isLoading,
    isDeletePending,
    deleteDialogOpen,
    employeeToDelete,
    assignSalaryDialogOpen,
    employeeForSalary,
    handleEdit,
    handleDelete,
    handleConfirmDelete,
    setDeleteDialogOpen,
    setAssignSalaryDialogOpen,
    handleOpenCreateDialog,
    handleCreateAccount,
    handleAssignSalary,
  } = useEmployeeContext();

  return (
    <>
      <Card>
        <CardContent className="p-0">
          <Table>
            <EmployeeTableHeader />
            <TableBody>
              {isLoading ? (
                <EmployeeTableLoading />
              ) : !employees || employees.length === 0 ? (
                <EmployeeTableEmpty onAddUser={handleOpenCreateDialog} />
              ) : (
                employees.map((employee: Employee) => (
                  <EmployeeTableRow
                    key={employee.id}
                    employee={employee}
                    isDeletePending={isDeletePending}
                    onEdit={handleEdit}
                    onDelete={handleDelete}
                    onCreateAccount={handleCreateAccount}
                    onAssignSalary={handleAssignSalary}
                  />
                ))
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      <DeleteEmployeeDialog
        open={deleteDialogOpen}
        employee={employeeToDelete}
        isDeleting={isDeletePending}
        onOpenChange={setDeleteDialogOpen}
        onConfirm={handleConfirmDelete}
      />

      {employeeForSalary && (
        <AssignSalaryDialog
          open={assignSalaryDialogOpen}
          onOpenChange={setAssignSalaryDialogOpen}
          employeeId={employeeForSalary.id}
          employeeName={employeeForSalary.name}
          onSuccess={() => {
            // Invalidate and refetch employees to show updated salary profile
            queryClient.invalidateQueries({ queryKey: ['employees'] });
          }}
        />
      )}
    </>
  );
};
