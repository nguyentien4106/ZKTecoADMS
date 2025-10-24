// ==========================================
// src/pages/Devices.tsx
// ==========================================
import { useState } from 'react'
import { PageHeader } from '@/components/PageHeader'
import { Button } from '@/components/ui/button'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { EmptyState } from '@/components/EmptyState'
import { useDevicesByUser } from '@/hooks/useDevices'
import { Monitor, Plus } from 'lucide-react'
import { CreateDeviceDialog } from '@/components/dialogs/CreateDeviceDialog'
import { DevicesContent, DeviceInfoDialog } from '@/components/devices'
import { useAuth } from '@/contexts/AuthContext'
import { DeviceProvider, useDeviceContext } from '@/contexts/DeviceContext'

const DevicesContent_Internal = () => {
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [infoDialogOpen, setInfoDialogOpen] = useState(false)
  const [selectedDeviceId, setSelectedDeviceId] = useState<string | null>(null)
  const [selectedDeviceName, setSelectedDeviceName] = useState<string | undefined>(undefined)
  
  const { applicationUserId } = useAuth()
  const { data: devices, isFetching, isError } = useDevicesByUser(applicationUserId)
  
  // Get device actions from context
  const {
    handleDelete,
    handleToggleActive,
    handleSyncUsers,
    handleSyncAttendance,
    handleClearAttendance,
    handleRestartDevice,
    handleLockDevice,
    handleUnlockDevice,
  } = useDeviceContext()

  const handleShowInfo = (id: string) => {
    const device = devices?.find((d) => d.id === id)
    setSelectedDeviceId(id)
    setSelectedDeviceName(device?.deviceName)
    setInfoDialogOpen(true)
  }

  if (isError) {
    return (
      <EmptyState
        icon={Monitor}
        title="Error loading devices"
        description="There was an error loading your devices. Please try again later."
      />
    )
  }

  if (isFetching && !devices) {
    return <LoadingSpinner />
  }

  return (
    <div>
      <PageHeader
        title="Devices"
        description="Mange your Biometric devices."
        action={
          <Button onClick={() => setCreateDialogOpen(true)}>
            <Plus className="w-4 h-4 mr-2" />
            Add Device
          </Button>
        }
      />

      <DevicesContent
        devices={devices}
        onCreateDevice={() => setCreateDialogOpen(true)}
        onDelete={handleDelete}
        onToggleActive={handleToggleActive}
        onShowInfo={handleShowInfo}
      />

      <CreateDeviceDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
      />

      <DeviceInfoDialog
        open={infoDialogOpen}
        onOpenChange={setInfoDialogOpen}
        deviceId={selectedDeviceId}
        deviceName={selectedDeviceName}
      />
    </div>
  )
}

// Wrap with DeviceProvider
export const Devices = () => {
  return (
    <DeviceProvider>
      <DevicesContent_Internal />
    </DeviceProvider>
  )
}
