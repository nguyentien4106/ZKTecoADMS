
// ==========================================
// src/components/dialogs/CreateDeviceDialog.tsx
// ==========================================
import { useState } from 'react'
import { useCreateDevice } from '@/hooks/useDevices'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import type { CreateDeviceRequest } from '@/types'

interface CreateDeviceDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
}

export const CreateDeviceDialog = ({
  open,
  onOpenChange,
}: CreateDeviceDialogProps) => {
  const createDevice = useCreateDevice()
  const [formData, setFormData] = useState<CreateDeviceRequest>({
    serialNumber: '',
    deviceName: '',
    model: '',
    ipAddress: '',
    port: 4370,
    location: '',
  })

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await createDevice.mutateAsync(formData)
    onOpenChange(false)
    setFormData({
      serialNumber: '',
      deviceName: '',
      model: '',
      ipAddress: '',
      port: 4370,
      location: '',
    })
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Add New Device</DialogTitle>
          <DialogDescription>
            Register a new ZKTeco device to the system
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="serialNumber">Serial Number *</Label>
            <Input
              id="serialNumber"
              value={formData.serialNumber}
              onChange={(e) =>
                setFormData({ ...formData, serialNumber: e.target.value })
              }
              placeholder="XXXXXXXXXX"
              required
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="deviceName">Device Name *</Label>
            <Input
              id="deviceName"
              value={formData.deviceName}
              onChange={(e) =>
                setFormData({ ...formData, deviceName: e.target.value })
              }
              placeholder="Main Entrance Device"
              required
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="model">Model</Label>
              <Input
                id="model"
                value={formData.model}
                onChange={(e) =>
                  setFormData({ ...formData, model: e.target.value })
                }
                placeholder="MB560-VL"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="port">Port</Label>
              <Input
                id="port"
                type="number"
                value={formData.port}
                onChange={(e) =>
                  setFormData({ ...formData, port: parseInt(e.target.value) })
                }
                placeholder="4370"
              />
            </div>
          </div>

          <div className="space-y-2">
            <Label htmlFor="ipAddress">IP Address</Label>
            <Input
              id="ipAddress"
              value={formData.ipAddress}
              onChange={(e) =>
                setFormData({ ...formData, ipAddress: e.target.value })
              }
              placeholder="192.168.1.100"
            />
          </div>

          <div className="space-y-2">
            <Label htmlFor="location">Location</Label>
            <Input
              id="location"
              value={formData.location}
              onChange={(e) =>
                setFormData({ ...formData, location: e.target.value })
              }
              placeholder="Building A - Main Door"
            />
          </div>

          <div className="flex justify-end gap-3 pt-4">
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={createDevice.isPending}>
              {createDevice.isPending ? 'Creating...' : 'Create Device'}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  )
}