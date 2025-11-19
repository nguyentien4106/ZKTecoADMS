import { Card, CardContent } from "@/components/ui/card";
import { Table, TableBody } from "@/components/ui/table";
import { useEmployeeContext } from "@/contexts/EmployeeContext";
import { EmployeeTableHeader } from "./EmployeeTableHeader";
import { EmployeeTableLoading } from "./EmployeeTableLoading";
import { EmployeeTableEmpty } from "./EmployeeTableEmpty";
import { EmployeeTableRow } from "./EmployeeTableRow";
import { DeleteEmployeeDialog } from "./DeleteEmployeeDialog";
import { Employee } from "@/types/employee";

export const EmployeesTable = () => {
  const {
    employees,
    isLoading,
    isDeletePending,
    deleteDialogOpen,
    employeeToDelete,
    handleEdit,
    handleDelete,
    handleConfirmDelete,
    setDeleteDialogOpen,
    handleOpenCreateDialog,
    handleCreateAccount,
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
    </>
  );
};
