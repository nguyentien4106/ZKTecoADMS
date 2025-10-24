import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
import { Button } from '@/components/ui/button'
import { Settings, Users, Calendar, RefreshCw, Trash2, Lock, Unlock } from 'lucide-react'
import { useDeviceContext } from '@/contexts/DeviceContext'

interface DeviceSettingsDropdownProps {
  deviceId: string
}

export const DeviceSettingsDropdown = ({
  deviceId,
}: DeviceSettingsDropdownProps) => {
    const {
        handleSyncAttendance,
        handleClearAttendance,
        handleRestartDevice,

    } = useDeviceContext()
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" size="icon">
          <Settings className="w-4 h-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-56">
        <DropdownMenuLabel>Device Actions</DropdownMenuLabel>
        <DropdownMenuSeparator />
        
        <DropdownMenuItem onClick={() => handleClearAttendance(deviceId)}>
          <Trash2 className="w-4 h-4 mr-2 text-red-500" />
          Clear Attendances
        </DropdownMenuItem>
        <DropdownMenuSeparator />

        <DropdownMenuItem onClick={() => handleSyncAttendance(deviceId)}>
          <Trash2 className="w-4 h-4 mr-2 text-red-500" />
            Clear Users
        </DropdownMenuItem>
        
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={() => handleSyncAttendance(deviceId)}>
          <Trash2 className="w-4 h-4 mr-2 text-red-500" />
            Clear All Data
        </DropdownMenuItem>
        <DropdownMenuSeparator />
        
        <DropdownMenuItem onClick={() => handleRestartDevice(deviceId)}>
          <RefreshCw className="w-4 h-4 mr-2" />
          Reboot Device
        </DropdownMenuItem>
        
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
