import { TableCell, TableRow } from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Employee } from "@/types/employee";
import { UserPrivileges } from "@/constants";
import { EmployeeTableActions } from "./EmployeeTableActions";
import { DollarSign, AlertCircle } from "lucide-react";
import { formatCurrency } from "@/lib/utils";

interface EmployeeTableRowProps {
  employee: Employee;
  isDeletePending: boolean;
  onEdit: (employee: Employee) => void;
  onDelete: (employee: Employee) => void;
  onCreateAccount?: (employee: Employee) => void;
  onAssignSalary?: (employee: Employee) => void;
}

export const EmployeeTableRow = ({
  employee,
  isDeletePending,
  onEdit,
  onDelete,
  onCreateAccount,
  onAssignSalary,
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
      <TableCell>
        {employee.currentSalaryProfile ? (
          <div className="flex flex-col gap-1">
            <div className="flex items-center gap-2">
              <Badge variant="secondary" className="flex items-center gap-1">
                <DollarSign className="w-3 h-3" />
                {employee.currentSalaryProfile.profileName}
              </Badge>
            </div>
            <div className="text-xs text-muted-foreground">
              {formatCurrency(employee.currentSalaryProfile.rate)}/{employee.currentSalaryProfile.rateTypeName}
            </div>
          </div>
        ) : (
          <div className="flex items-center gap-1 text-muted-foreground">
            <AlertCircle className="w-4 h-4 text-orange-500" />
            <span className="text-xs">No salary profile</span>
          </div>
        )}
      </TableCell>
      <TableCell className="text-right">
        <EmployeeTableActions
          employee={employee}
          isDeletePending={isDeletePending}
          onEdit={onEdit}
          onDelete={onDelete}
          onCreateAccount={onCreateAccount}
          onAssignSalary={onAssignSalary}
        />
      </TableCell>
    </TableRow>
  );
};
