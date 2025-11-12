import { createContext, useContext, ReactNode, useState } from 'react'
import {
  useDeleteDevice,
  useActiveDevice,
  useSyncUsers,
  useSyncAttendance,
  useClearAttendance,
  useRestartDevice,
  useSendCommand,
} from '@/hooks/useDevices'
import { DeviceCommandTypes } from '@/types/device'

interface DeviceContextType {
  // Delete device
  handleDelete: (id: string) => Promise<void>
  
  // Toggle active status
  handleToggleActive: (id: string) => Promise<void>
  
  // Device commands
  handleSyncUsers: (deviceId: string) => Promise<void>
  handleSyncAttendances: (deviceId: string) => Promise<void>
  handleClearAttendance: (deviceId: string) => Promise<void>
  handleClearUsers: (deviceId: string) => Promise<void>
  handleClearData: (deviceId: string) => Promise<void>
  handleRestartDevice: (deviceId: string) => Promise<void>
  
  // Loading states
  isDeleting: boolean
  isTogglingActive: boolean
  isSyncingUsers: boolean
  isSyncingAttendance: boolean
  isClearingAttendance: boolean
  isRestartingDevice: boolean
  
  // Dialog states
  createDialogOpen: boolean
  setCreateDialogOpen: (open: boolean) => void
  infoDialogOpen: boolean
  setInfoDialogOpen: (open: boolean) => void
  
  // Selected device
  selectedDeviceId: string | null
  setSelectedDeviceId: (id: string | null) => void
  selectedDeviceName: string | undefined
  setSelectedDeviceName: (name: string | undefined) => void
  
  // Helper functions
  openCreateDialog: () => void
  openInfoDialog: (id: string, name?: string) => void
}

const DeviceContext = createContext<DeviceContextType | undefined>(undefined)

interface DeviceProviderProps {
  children: ReactNode
}

export const DeviceProvider = ({ children }: DeviceProviderProps) => {
  // Dialog states
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [infoDialogOpen, setInfoDialogOpen] = useState(false)
  const [selectedDeviceId, setSelectedDeviceId] = useState<string | null>(null)
  const [selectedDeviceName, setSelectedDeviceName] = useState<string | undefined>(undefined)

  // Mutation hooks
  const deleteDevice = useDeleteDevice()
  const activeDevice = useActiveDevice()
  const syncUsers = useSyncUsers()
  const syncAttendance = useSyncAttendance()
  const clearAttendance = useClearAttendance()
  const restartDevice = useRestartDevice()
  const sendCommand = useSendCommand()
  

  // Handler functions
  const handleDelete = async (id: string) => {
    try {
      await deleteDevice.mutateAsync(id)
    } catch (error) {
      console.error('Error deleting device:', error)
    }
  }

  const handleToggleActive = async (id: string) => {
    try {
      await activeDevice.mutateAsync(id)
    } catch (error) {
      console.error('Error toggling device active status:', error)
    }
  }

  const handleSyncUsers = async (id: string) => {
    try {
      await syncUsers.mutateAsync(id)
    } catch (error) {
      console.error('Error syncing users:', error)
    }
  }

  const handleSyncAttendances = async (deviceId: string) => {
    try {
      await sendCommand.mutateAsync({
        deviceId: deviceId,
        data: {
          commandType: DeviceCommandTypes.SYNC_ATTENDANCES
        }
      })
    } catch (error) {
      console.error('Error syncing attendance:', error)
    }
  }

  const handleClearAttendance = async (deviceId: string) => {
    try {
        await sendCommand.mutateAsync({
          deviceId: deviceId,
          data: {
            commandType: DeviceCommandTypes.CLEAR_ATTENDANCEs
          }
        })
    } catch (error) {
      console.error('Error clearing attendance:', error)
    }
  }

  const handleClearUsers = async (deviceId: string) => {
    try {
      await sendCommand.mutateAsync({
        deviceId: deviceId,
        data: {
          commandType: DeviceCommandTypes.CLEAR_USERS
        }
      })
    } catch (error) {
      console.error('Error clearing users:', error)
    }
  }

  const handleClearData = async (deviceId: string) => {
    try {
      await sendCommand.mutateAsync({
        deviceId: deviceId,
        data: {
          commandType: DeviceCommandTypes.CLEAR_DATA
        }
      })
    } catch (error) {
      console.error('Error clearing data:', error)
    }
  } 

  const handleRestartDevice = async (deviceId: string) => {
    try {
      await sendCommand.mutateAsync({
        deviceId: deviceId,
        data: {
          commandType: DeviceCommandTypes.RESTART_DEVICE
        }
      })
    } catch (error) {
      console.error('Error restarting device:', error)
    }
  }

  // Helper functions for dialogs
  const openCreateDialog = () => {
    setCreateDialogOpen(true)
  }

  const openInfoDialog = (id: string, name?: string) => {
    setSelectedDeviceId(id)
    setSelectedDeviceName(name)
    setInfoDialogOpen(true)
  }

  const value: DeviceContextType = {
    handleDelete,
    handleToggleActive,
    handleSyncUsers,
    handleSyncAttendances,
    handleClearAttendance,
    handleRestartDevice,
    handleClearData,
    handleClearUsers,

    isDeleting: deleteDevice.isPending,
    isTogglingActive: activeDevice.isPending,
    isSyncingUsers: syncUsers.isPending,
    isSyncingAttendance: syncAttendance.isPending,
    isClearingAttendance: clearAttendance.isPending,
    isRestartingDevice: restartDevice.isPending,
    
    createDialogOpen,
    setCreateDialogOpen,
    infoDialogOpen,
    setInfoDialogOpen,
    selectedDeviceId,
    setSelectedDeviceId,
    selectedDeviceName,
    setSelectedDeviceName,
    openCreateDialog,
    openInfoDialog,
  }

  return <DeviceContext.Provider value={value}>{children}</DeviceContext.Provider>
}

export const useDeviceContext = () => {
  const context = useContext(DeviceContext)
  if (context === undefined) {
    throw new Error('useDeviceContext must be used within a DeviceProvider')
  }
  return context
}
