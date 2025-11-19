import { Button } from "@/components/ui/button";
import { Trash2, Edit, UserPlus, User2Icon } from "lucide-react";
import { Employee } from "@/types/employee";

interface EmployeeTableActionsProps {
  employee: Employee;
  isDeletePending: boolean;
  onEdit: (employee: Employee) => void;
  onDelete: (employee: Employee) => void;
  onCreateAccount?: (employee: Employee) => void;
}

export const EmployeeTableActions = ({
  employee,
  isDeletePending,
  onEdit,
  onDelete,
  onCreateAccount,
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
        employee.applicationUser ? (
          <Button
            variant="ghost"
            size="icon"
            onClick={() => onCreateAccount(employee)}
            title="Update Account"
          >
            <User2Icon className="w-4 h-4 text-green-600" />
          </Button>
        ) : (
          <Button
            variant="ghost"
            size="icon"
            onClick={() => onCreateAccount(employee)}
            title="Create Account"
          >
            <UserPlus className="w-4 h-4 text-blue-600" />
          </Button>
        )
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
