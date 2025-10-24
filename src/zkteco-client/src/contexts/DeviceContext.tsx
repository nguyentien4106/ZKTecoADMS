import { createContext, useContext, ReactNode } from 'react'
import {
  useDeleteDevice,
  useActiveDevice,
  useSyncUsers,
  useSyncAttendance,
  useClearAttendance,
  useRestartDevice,
  useLockDevice,
  useUnlockDevice,
} from '@/hooks/useDevices'

interface DeviceContextType {
  // Delete device
  handleDelete: (id: string) => Promise<void>
  
  // Toggle active status
  handleToggleActive: (id: string) => Promise<void>
  
  // Device commands
  handleSyncUsers: (id: string) => Promise<void>
  handleSyncAttendance: (id: string) => Promise<void>
  handleClearAttendance: (id: string) => Promise<void>
  handleRestartDevice: (id: string) => Promise<void>
  handleLockDevice: (id: string) => Promise<void>
  handleUnlockDevice: (id: string) => Promise<void>
  
  // Loading states
  isDeleting: boolean
  isTogglingActive: boolean
  isSyncingUsers: boolean
  isSyncingAttendance: boolean
  isClearingAttendance: boolean
  isRestartingDevice: boolean
  isLockingDevice: boolean
  isUnlockingDevice: boolean
}

const DeviceContext = createContext<DeviceContextType | undefined>(undefined)

interface DeviceProviderProps {
  children: ReactNode
}

export const DeviceProvider = ({ children }: DeviceProviderProps) => {
  // Mutation hooks
  const deleteDevice = useDeleteDevice()
  const activeDevice = useActiveDevice()
  const syncUsers = useSyncUsers()
  const syncAttendance = useSyncAttendance()
  const clearAttendance = useClearAttendance()
  const restartDevice = useRestartDevice()
  const lockDevice = useLockDevice()
  const unlockDevice = useUnlockDevice()

  // Handler functions
  const handleDelete = async (id: string) => {
    try {
      if (confirm('Are you sure you want to delete this device?')) {
        await deleteDevice.mutateAsync(id)
      }
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

  const handleSyncAttendance = async (id: string) => {
    try {
      await syncAttendance.mutateAsync(id)
    } catch (error) {
      console.error('Error syncing attendance:', error)
    }
  }

  const handleClearAttendance = async (id: string) => {
    try {
      if (confirm('Are you sure you want to clear all attendance records from this device?')) {
        await clearAttendance.mutateAsync(id)
      }
    } catch (error) {
      console.error('Error clearing attendance:', error)
    }
  }

  const handleRestartDevice = async (id: string) => {
    try {
      if (confirm('Are you sure you want to restart this device?')) {
        await restartDevice.mutateAsync(id)
      }
    } catch (error) {
      console.error('Error restarting device:', error)
    }
  }

  const handleLockDevice = async (id: string) => {
    try {
      await lockDevice.mutateAsync(id)
    } catch (error) {
      console.error('Error locking device:', error)
    }
  }

  const handleUnlockDevice = async (id: string) => {
    try {
      await unlockDevice.mutateAsync(id)
    } catch (error) {
      console.error('Error unlocking device:', error)
    }
  }

  const value: DeviceContextType = {
    handleDelete,
    handleToggleActive,
    handleSyncUsers,
    handleSyncAttendance,
    handleClearAttendance,
    handleRestartDevice,
    handleLockDevice,
    handleUnlockDevice,
    isDeleting: deleteDevice.isPending,
    isTogglingActive: activeDevice.isPending,
    isSyncingUsers: syncUsers.isPending,
    isSyncingAttendance: syncAttendance.isPending,
    isClearingAttendance: clearAttendance.isPending,
    isRestartingDevice: restartDevice.isPending,
    isLockingDevice: lockDevice.isPending,
    isUnlockingDevice: unlockDevice.isPending,
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
