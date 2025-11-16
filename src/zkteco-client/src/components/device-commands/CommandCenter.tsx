// ==========================================
// src/components/device-commands/CommandCenter.tsx
// ==========================================
import { Device } from '@/types'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { RefreshCw, Users, Download, Trash2, RefreshCcwDot } from 'lucide-react'
import { useDeviceCommands } from '@/hooks/useDeviceCommands'

interface CommandCenterProps {
  devices: Device[]
  selectedDeviceId: string
  onDeviceChange: (deviceId: string) => void
  onRefresh: () => void
  isDeviceInactive?: boolean
}

export const CommandCenter = ({
  devices,
  selectedDeviceId,
  onDeviceChange,
  onRefresh,
  isDeviceInactive = false,
}: CommandCenterProps) => {
  const {
    handleSyncUsers,
    handleClearAttendances,
    handleClearUsers,
    handleSyncAttendances,
    handleRestartDevice,
  } = useDeviceCommands({
    onSuccess: onRefresh,
  })

  return (
    <Card>
      <CardHeader>
        <CardTitle>Command Center</CardTitle>
        <CardDescription>
          Select a device and execute commands to manage users, attendance, and device operations
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Device Selector */}
        <div className="flex items-center gap-4">
          <label className="text-sm font-medium min-w-20">Device:</label>
          <Select value={selectedDeviceId} onValueChange={onDeviceChange}>
            <SelectTrigger className="w-full max-w-md">
              <SelectValue placeholder="Select a device" />
            </SelectTrigger>
            <SelectContent>
              {devices.map((device) => (
                <SelectItem key={device.id} value={device.id}>
                  {device.deviceName} - {device.serialNumber}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
          <Button
            variant="outline"
            size="icon"
            onClick={onRefresh}
            disabled={!selectedDeviceId || isDeviceInactive}
          >
            <RefreshCw className="w-4 h-4" />
          </Button>
        </div>

        {/* Command Buttons */}
        {selectedDeviceId && (
          <div className="space-y-3">
            <div className="text-sm font-medium">Available Commands:</div>
            <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-6 gap-3">
              <Button
                variant="outline"
                className="flex items-center gap-2"
                onClick={() => handleSyncUsers(selectedDeviceId)}
                disabled={isDeviceInactive}
              >
                <Users className="w-4 h-4" />
                Sync Users
              </Button>
              <Button
                variant="outline"
                className="flex items-center gap-2"
                onClick={() => handleSyncAttendances(selectedDeviceId)}
                disabled={isDeviceInactive}
              >
                <Download className="w-4 h-4" />
                Sync Attendance
              </Button>
              <Button
                variant="outline"
                className="flex items-center gap-2"
                onClick={() => handleClearAttendances(selectedDeviceId)}
                disabled={isDeviceInactive}
              >
                <Trash2 className="w-4 h-4 text-red-500" />
                Clear Attendance
              </Button>
              <Button
                variant="outline"
                className="flex items-center gap-2"
                onClick={() => handleClearUsers(selectedDeviceId)}
                disabled={isDeviceInactive}
              >
                <Trash2 className="w-4 h-4 text-red-500" />
                Clear Users
              </Button>
              <Button
                variant="outline"
                className="flex items-center gap-2"
                onClick={() => handleRestartDevice(selectedDeviceId)}
                disabled={isDeviceInactive}
              >
                <RefreshCcwDot className="w-4 h-4 text-red-500" />
                Restart
              </Button>
            </div>
          </div>
        )}
      </CardContent>
    </Card>
  )
}
