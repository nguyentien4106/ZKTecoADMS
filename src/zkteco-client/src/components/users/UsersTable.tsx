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
  Edit
} from "lucide-react";
import { User } from "@/types";
import { UserPrivileges } from "@/constants";

type UsersTableProps = {
  users?: User[];
  onEdit: (user: User) => void;
  onDelete: (id: string) => void;
  isDeletePending: boolean;
  onAddUser: () => void;
  isLoading: boolean;
};

export const UsersTable = ({
  users,
  onEdit,
  onDelete,
  isDeletePending,
  onAddUser,
  isLoading,
}: UsersTableProps) => {
  if (isLoading) {
    return (
      <Card>
        <CardContent className="p-0">
          <div className="flex items-center justify-center h-48"></div>
          <span className="text-muted-foreground">Loading users...</span>
        </CardContent>
      </Card>
    );
  }
  return (
    <Card>
      <CardContent className="p-0">
        {!users || users.length === 0 ? (
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
        ) : (
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
              {users.map((user: User) => (
                <TableRow key={user.id}>
                  <TableCell className="font-mono font-medium">
                    {user.pin}
                  </TableCell>
                  <TableCell className="font-medium">{user.fullName}</TableCell>
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
                      {/* <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onSync(user.id, user.fullName)}
                        disabled={isSyncPending}
                      >
                        <RefreshCw className="w-4 h-4" />
                      </Button> */}
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onEdit(user)}
                      >
                        <Edit className="w-4 h-4" />
                      </Button>
                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => onDelete(user.id)}
                        disabled={isDeletePending}
                      >
                        <Trash2 className="w-4 h-4 text-destructive" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        )}
      </CardContent>
    </Card>
  );
};
