import { Card, CardContent } from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { EmptyState } from "@/components/EmptyState";
import {
  Users as UsersIcon,
  Plus,
  Trash2,
  Edit,
  UserPlus
} from "lucide-react";
import { User } from "@/types/user";
import { UserPrivileges } from "@/constants";

type UsersTableProps = {
  users?: User[];
  onEdit: (user: User) => void;
  onDelete: (id: string) => void;
  isDeletePending: boolean;
  onAddUser: () => void;
  isLoading: boolean;
  onCreateAccount?: (user: User) => void;
};

export const UsersTable = ({
  users,
  onEdit,
  onDelete,
  isDeletePending,
  onAddUser,
  isLoading,
  onCreateAccount,
}: UsersTableProps) => {
  return (
    <Card>
      <CardContent className="p-0">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>PIN</TableHead>
              <TableHead>Name</TableHead>
              <TableHead>Privilege</TableHead>
              <TableHead>Department</TableHead>
              <TableHead>Email</TableHead>
              <TableHead>Card Number</TableHead>
              <TableHead>Device</TableHead>
              <TableHead>Status</TableHead>
              <TableHead className="text-right">Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell colSpan={9} className="h-48">
                  <div className="flex items-center justify-center">
                    <div className="flex flex-col items-center gap-2">
                      <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
                      <span className="text-muted-foreground">Loading users...</span>
                    </div>
                  </div>
                </TableCell>
              </TableRow>
            ) : !users || users.length === 0 ? (
              <TableRow>
                <TableCell colSpan={9} className="h-48">
                  <EmptyState
                    icon={UsersIcon}
                    title="No users found"
                    description="Get started by adding your first user"
                    action={
                      <Button onClick={onAddUser}>
                        <Plus className="w-4 h-4 mr-2" />
                        Add User
                      </Button>
                    }
                  />
                </TableCell>
              </TableRow>
            ) : (
              users.map((user: User) => (
                <TableRow key={user.id}>
                  <TableCell className="font-mono font-medium">
                    {user.pin}
                  </TableCell>
                  <TableCell className="font-medium">{user.name}</TableCell>
                  <TableCell className="font-medium">
                    {UserPrivileges[user.privilege]}
                  </TableCell>
                  <TableCell>{user.department || "-"}</TableCell>
                  <TableCell className="text-muted-foreground">
                    {user.email || "-"}
                  </TableCell>
                  <TableCell className="text-muted-foreground">
                    {user.cardNumber || "-"}
                  </TableCell>
                  <TableCell className="text-muted-foreground">
                    {user.deviceName || "-"}
                  </TableCell>
                  <TableCell>
                    <Badge variant={user.isActive ? "default" : "secondary"}>
                      {user.isActive ? "Active" : "Inactive"}
                    </Badge>
                  </TableCell>
                  <TableCell className="text-right">
                    <div className="flex justify-end gap-2">
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onEdit(user)}
                        title="Edit User"
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      {onCreateAccount && (
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => onCreateAccount(user)}
                          title="Create Account"
                        >
                          <UserPlus className="w-4 h-4 text-blue-600" />
                        </Button>
                      )}
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onDelete(user.id)}
                        disabled={isDeletePending}
                        title="Delete User"
                      >
                        <Trash2 className="w-4 h-4 text-destructive" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
};
