
// ==========================================
// src/pages/Users.tsx
// ==========================================
import { useEffect, useState } from 'react'
import { PageHeader } from '@/components/PageHeader'
import { Button } from '@/components/ui/button'
import { UsersTable } from '@/components/users/UsersTable'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { useUsers, useDeleteUser, useSyncUserToAllDevices } from '@/hooks/useUsers'
import { Users as UsersIcon, Plus, Trash2, Edit, RefreshCw } from 'lucide-react'
import { CreateUserDialog } from '@/components/dialogs/CreateUserDialog'
import { toast } from 'sonner'
import { User } from '@/types'
import FilterBar from '@/components/users/FilterBar'
import { useAuth } from '@/contexts/AuthContext'
import { useDevicesByUser } from '@/hooks/useDevices'

export const Users = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [userToEdit, setUserToEdit] = useState<User | null>(null)
  const { applicationUserId } = useAuth()
  const { data: devices, isFetching: devicesFetching } = useDevicesByUser(applicationUserId)
  const [selectedDeviceIds, setSelectedDeviceIds] = useState<string[]>([])
  const deleteUser = useDeleteUser()
  const syncUser = useSyncUserToAllDevices()
  const { data: users, isLoading } = useUsers(selectedDeviceIds)

  const handleDelete = async (id: string) => {
    if (!id) return
    if (confirm('Are you sure you want to delete this user?')) {
      await deleteUser.mutateAsync(id)
    }
  }

  useEffect(() => {
    setSelectedDeviceIds(devices ? devices.map(device => device.id) : [])
  }, [devices])

  const handleEdit = (user: User) => {
    setUserToEdit(user)
    setCreateDialogOpen(true)
  }

  const handleSync = async (id: string, name: string) => {
    if (!id) return
    await toast.promise(syncUser.mutateAsync(id), {
      loading: `Syncing ${name} to all devices...`,
      success: `${name} synced successfully`,
      error: 'Failed to sync user',
    })
  }

  const handleSubmit = (deviceIds: string[]) => {
    setSelectedDeviceIds(deviceIds)
  }

  if (devicesFetching) {
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

      <FilterBar 
        devices={devices} 
        handleSubmit={handleSubmit} 
        selectedDeviceIds={selectedDeviceIds}
      /> 

      <UsersTable
        users={users}
        onSync={handleSync}
        onEdit={handleEdit}
        onDelete={handleDelete}
        isSyncPending={syncUser.isPending}
        isDeletePending={deleteUser.isPending}
        onAddUser={() => setCreateDialogOpen(true)}
        isLoading={isLoading}
      />

      <CreateUserDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        user={userToEdit}
      />
    </div>
  )
}
