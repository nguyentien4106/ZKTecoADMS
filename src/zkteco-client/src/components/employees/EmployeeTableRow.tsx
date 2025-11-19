import { TableCell, TableRow } from "@/components/ui/table";
import { Employee } from "@/types/employee";
import { UserPrivileges } from "@/constants";
import { EmployeeTableActions } from "./EmployeeTableActions";

interface EmployeeTableRowProps {
  employee: Employee;
  isDeletePending: boolean;
  onEdit: (employee: Employee) => void;
  onDelete: (employee: Employee) => void;
  onCreateAccount?: (employee: Employee) => void;
}

export const EmployeeTableRow = ({
  employee,
  isDeletePending,
  onEdit,
  onDelete,
  onCreateAccount,
}: EmployeeTableRowProps) => {
  return (
    <TableRow key={employee.id}>
      <TableCell className="text-muted-foreground">
        {employee.deviceName || "-"}
      </TableCell>
      <TableCell className="font-mono font-medium">
        {employee.pin}
      </TableCell>
      <TableCell className="font-medium">{employee.name}</TableCell>
      <TableCell className="font-medium">
        {UserPrivileges[employee.privilege]}
      </TableCell>
      <TableCell>{employee.department || "-"}</TableCell>
      <TableCell className="text-muted-foreground">
        {employee.cardNumber || "-"}
      </TableCell>
      <TableCell className="text-right">
        <EmployeeTableActions
          employee={employee}
          isDeletePending={isDeletePending}
          onEdit={onEdit}
          onDelete={onDelete}
          onCreateAccount={onCreateAccount}
        />
      </TableCell>
    </TableRow>
  );
};
