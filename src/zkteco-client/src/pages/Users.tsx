
// ==========================================
// src/pages/Users.tsx
// ==========================================
import { useState } from 'react'
import { PageHeader } from '@/components/PageHeader'
import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { EmptyState } from '@/components/EmptyState'
import { useUsers, useDeleteUser, useSyncUserToAllDevices } from '@/hooks/useUsers'
import { Users as UsersIcon, Plus, Trash2, Edit, RefreshCw } from 'lucide-react'
import { CreateUserDialog } from '@/components/dialogs/CreateUserDialog'
import { toast } from 'sonner'
import { User } from '@/types'

export const Users = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [userToEdit, setUserToEdit] = useState<User | null>(null)
  const { data: users, isLoading } = useUsers()
  const deleteUser = useDeleteUser()
  const syncUser = useSyncUserToAllDevices()

  const handleDelete = async (id: number) => {
    if (confirm('Are you sure you want to delete this user?')) {
      await deleteUser.mutateAsync(id)
    }
  }

  const handleEdit = (user: User) => {
    setUserToEdit(user)
    setCreateDialogOpen(true)
  }

  const handleSync = async (id: number, name: string) => {
    toast.promise(syncUser.mutateAsync(id), {
      loading: `Syncing ${name} to all devices...`,
      success: `${name} synced successfully`,
      error: 'Failed to sync user',
    })
  }

  if (isLoading) {
    return <LoadingSpinner />
  }

  return (
    <div>
      <PageHeader
        title="Users"
        description="Manage users and sync them to devices"
        action={
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Add User
          </Button>
        }
      />

      <Card>
        <CardContent className="p-0">
          {!users || users.length === 0 ? (
            <EmptyState
              icon={UsersIcon}
              title="No users found"
              description="Get started by adding your first user"
              action={
                <Button onClick={() => setCreateDialogOpen(true)}>
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
                  <TableHead>Position</TableHead>
                  <TableHead>Email</TableHead>
                  <TableHead>Card Number</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {users.map((user) => (
                  <TableRow key={user.id}>
                    <TableCell className="font-mono font-medium">
                      {user.pin}
                    </TableCell>
                    <TableCell className="font-medium">
                      {user.fullName}
                    </TableCell>
                    <TableCell className="font-medium">
                      {user.privilege}
                    </TableCell>
                    <TableCell>{user.department || '-'}</TableCell>
                    <TableCell>{user.position || '-'}</TableCell>
                    <TableCell className="text-muted-foreground">
                      {user.email || '-'}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {user.cardNumber || '-'}
                    </TableCell>
                    <TableCell>
                      <Badge variant={user.isActive ? 'success' : 'secondary'}>
                        {user.isActive ? 'Active' : 'Inactive'}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => handleSync(user.id, user.fullName)}
                          disabled={syncUser.isPending}
                        >
                          <RefreshCw className="w-4 h-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(user)}>
                          <Edit className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => handleDelete(user.id)}
                          disabled={deleteUser.isPending}
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

      <CreateUserDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        user={userToEdit}
      />
    </div>
  )
}
