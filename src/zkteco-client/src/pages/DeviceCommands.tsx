// ==========================================
// src/pages/DeviceCommands.tsx
// ==========================================
import { useState, useEffect } from 'react'
import { PageHeader } from '@/components/PageHeader'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { EmptyState } from '@/components/EmptyState'
import { useDevicesByUser, useDevicePendingCommands as useDeviceCommandsQuery } from '@/hooks/useDevices'
import { CommandCenter, CommandHistory } from '@/components/device-commands'
import { Terminal } from 'lucide-react'

export const DeviceCommands = () => {
  const { data: devices, isLoading: devicesLoading } = useDevicesByUser()
  const [selectedDeviceId, setSelectedDeviceId] = useState<string>('')
  const { data: commands, isFetching: commandsLoading, refetch } = useDeviceCommandsQuery(selectedDeviceId)

  // Auto-select first device
  useEffect(() => {
    if (devices && devices.length > 0 && !selectedDeviceId) {
      setSelectedDeviceId(devices[0].id)
    }
  }, [devices, selectedDeviceId])

  if (devicesLoading) {
    return <LoadingSpinner />
  }

  if (!devices || devices.length === 0) {
    return (
      <EmptyState
        icon={Terminal}
        title="No devices available"
        description="You need to add a device before sending commands."
      />
    )
  }

  return (
    <div className="space-y-6">
      <PageHeader
        title="Device Commands"
        description="Send commands and monitor their execution status"
      />

      <CommandCenter
        devices={devices}
        selectedDeviceId={selectedDeviceId}
        onDeviceChange={setSelectedDeviceId}
        onRefresh={refetch}
      />

      <CommandHistory
        commands={commands}
        isLoading={commandsLoading}
        selectedDeviceId={selectedDeviceId}
      />
    </div>
  )
}
