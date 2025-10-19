// ==========================================
// src/pages/Devices.tsx
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
import { useDevices, useDeleteDevice } from '@/hooks/useDevices'
import { Monitor, Plus, Trash2, Settings } from 'lucide-react'
import { format } from 'date-fns'
import { CreateDeviceDialog } from '@/components/dialogs/CreateDeviceDialog'

export const Devices = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const { data: devices, isLoading } = useDevices()
  console.log('devices', devices)
  const deleteDevice = useDeleteDevice()

  const handleDelete = async (id: number) => {
    if (confirm('Are you sure you want to delete this device?')) {
      await deleteDevice.mutateAsync(id)
    }
  }

  if (isLoading) {
    return <LoadingSpinner />
  }

  return (
    <div>
      <PageHeader
        title="Devices"
        description="Manage your ZKTeco attendance devices"
        action={
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Add Device
          </Button>
        }
      />

      <Card>
        <CardContent className="p-0">
          {!devices || devices.length === 0 ? (
            <EmptyState
              icon={Monitor}
              title="No devices found"
              description="Get started by adding your first ZKTeco device"
              action={
                <Button onClick={() => setCreateDialogOpen(true)}>
                  <Plus className="w-4 h-4 mr-2" />
                  Add Device
                </Button>
              }
            />
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Device Name</TableHead>
                  <TableHead>Serial Number</TableHead>
                  <TableHead>Model</TableHead>
                  <TableHead>IP Address</TableHead>
                  <TableHead>Location</TableHead>
                  <TableHead>Status</TableHead>
                  <TableHead>Is Active</TableHead>
                  <TableHead>Last Online</TableHead>
                  <TableHead className="text-right">Actions</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {devices.map((device) => (
                  <TableRow key={device.id}>
                    <TableCell className="font-medium">
                      {device.deviceName}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {device.serialNumber}
                    </TableCell>
                    <TableCell>{device.model || '-'}</TableCell>
                    <TableCell>{device.ipAddress || '-'}</TableCell>
                    <TableCell>{device.location || '-'}</TableCell>
                    <TableCell>
                      <Badge
                        variant={
                          device.deviceStatus === 'Online'
                            ? 'success'
                            : device.deviceStatus === 'Offline'
                            ? 'secondary'
                            : 'destructive'
                        }
                      >
                        {device.deviceStatus}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <Badge
                        variant={
                          device.deviceStatus === 'Online'
                            ? 'success'
                            : device.deviceStatus === 'Offline'
                            ? 'secondary'
                            : 'destructive'
                        }
                      >
                        {device.isActive ? "Actived" : "Deactived"}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {device.lastOnline
                        ? format(new Date(device.lastOnline), 'MMM dd, yyyy HH:mm')
                        : 'Never'}
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="icon">
                          <Settings className="w-4 h-4" />
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          onClick={() => handleDelete(device.id)}
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

      <CreateDeviceDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
      />
    </div>
  )
}
