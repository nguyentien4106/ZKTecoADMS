// ==========================================
// src/hooks/useDeviceCommands.ts
// ==========================================
import { toast } from 'sonner'
import { useSendCommand } from './useDevices'
import { DeviceCommandTypes } from '@/types/device'

export interface DeviceCommandHandlers {
  handleSyncUsers: (deviceId: string) => Promise<void>
  handleSyncAttendances: (deviceId: string) => Promise<void>
  handleClearAttendances: (deviceId: string) => Promise<void>
  handleClearUsers: (deviceId: string) => Promise<void>
  handleClearData: (deviceId: string) => Promise<void>
  handleRestartDevice: (deviceId: string) => Promise<void>}

/**
 * Custom hook that provides device command handlers and their loading states.
 * Can be used in any component that needs to execute device commands.
 * 
 * @param options.onSuccess - Optional callback to execute after successful command
 * @param options.validateDeviceId - Whether to validate deviceId before execution (default: true)
 */
export const useDeviceCommands = (options?: {
  onSuccess?: () => void
  validateDeviceId?: boolean
}): DeviceCommandHandlers => {
  const { onSuccess, validateDeviceId = true } = options || {}

  // Command mutations
    const sendCommand = useSendCommand()    
  /**
   * Generic command handler that validates device ID and handles success/error
   */
  const executeCommand = async (
    deviceId: string,
    commandType: DeviceCommandTypes,
    commandName: string
  ) => {
    if (validateDeviceId && !deviceId) {
      toast.error('Please select a device')
      return
    }

    try {
      await sendCommand.mutateAsync({
        deviceId: deviceId,
        data: {
          commandType: commandType
        }
      })
      toast.success(`${commandName} command sent successfully`)
      onSuccess?.()
    } catch (error) {
      toast.error(`Failed to send ${commandName} command`)
      console.error(`Error executing ${commandName}:`, error)
    }
  }

  return {
    handleSyncUsers: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.SYNC_USERS, 'Sync Users'),
    handleSyncAttendances: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.SYNC_ATTENDANCES, 'Sync Attendances'),
    handleClearAttendances: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.CLEAR_ATTENDANCEs, 'Clear Attendances'),
    handleClearData: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.CLEAR_DATA, 'Clear Data'),
    handleClearUsers: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.CLEAR_USERS, 'Clear Users'),
    handleRestartDevice: (deviceId) => executeCommand(deviceId, DeviceCommandTypes.RESTART_DEVICE, 'Restart Device')
  }
}
