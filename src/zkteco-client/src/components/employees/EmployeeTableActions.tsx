import { Button } from "@/components/ui/button";
import { Trash2, Edit, UserPlus, User2Icon, DollarSign } from "lucide-react";
import { Employee } from "@/types/employee";

interface EmployeeTableActionsProps {
  employee: Employee;
  isDeletePending: boolean;
  onEdit: (employee: Employee) => void;
  onDelete: (employee: Employee) => void;
  onCreateAccount?: (employee: Employee) => void;
  onAssignSalary?: (employee: Employee) => void;
}

export const EmployeeTableActions = ({
  employee,
  isDeletePending,
  onEdit,
  onDelete,
  onCreateAccount,
  onAssignSalary,
}: EmployeeTableActionsProps) => {
  return (
    <div className="flex justify-end gap-2">
      <Button
        variant="ghost"
        size="icon"
        onClick={() => onEdit(employee)}
        title="Edit User"
      >
        <Edit className="w-4 h-4" />
      </Button>
      {onCreateAccount && (
        <Button
          variant="ghost"
          size="icon"
          onClick={() => onCreateAccount(employee)}
          title="Update Account"
        >
          <User2Icon className={`w-4 h-4 ${employee.applicationUser ? "text-green-600" : "text-muted-foreground"}`} />
        </Button>
      )}
      {onAssignSalary && (
        <Button
          variant="ghost"
          size="icon"
          onClick={() => onAssignSalary(employee)}
          title="Assign Salary Profile"
        >
          <DollarSign className={`w-4 h-4 ${employee.currentSalaryProfile ? "text-emerald-600" : "text-muted-foreground"}`} />
        </Button>
      )}
      <Button
        variant="ghost"
        size="icon"
        onClick={() => onDelete(employee)}
        disabled={isDeletePending}
        title="Delete User"
      >
        <Trash2 className="w-4 h-4 text-destructive" />
      </Button>
    </div>
  );
};
