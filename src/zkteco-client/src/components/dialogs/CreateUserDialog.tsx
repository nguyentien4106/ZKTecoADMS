
// ==========================================
// src/components/dialogs/CreateUserDialog.tsx
// ==========================================
import { useEffect, useState } from 'react'
import { useCreateUser } from '@/hooks/useUsers'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import type { CreateUserRequest, User } from '@/types'
import {
  Select,
  SelectContent,
  SelectGroup,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import { UserPrivileges } from '@/constants'
import { defaultNewUser } from '@/constants/defaultValue'
import { Button } from '../ui/button'
import { useDevicesByUser } from '@/hooks/useDevices'
import { useAuth } from '@/contexts/AuthContext'

interface CreateUserDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  user: User | null
}

export const CreateUserDialog = ({
  open,
  onOpenChange,
  user
}: CreateUserDialogProps) => {
  const createUser = useCreateUser()
  const [formData, setFormData] = useState<CreateUserRequest>(user ?? defaultNewUser)
  const { applicationUserId } = useAuth()
  const { data: devices, isFetching } = useDevicesByUser(applicationUserId)

  useEffect(() => {
    if (user) {
      setFormData(user)
    }
  }, [user])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    await createUser.mutateAsync(formData)
    onOpenChange(false)
    setFormData(defaultNewUser)
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Add New User</DialogTitle>
          <DialogDescription>
            Create a new user and sync to devices
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="pin">PIN *</Label>
              <Input
                id="pin"
                value={formData.pin}
                onChange={(e) => setFormData({ ...formData, pin: e.target.value })}
                placeholder="1001"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="fullName">Full Name *</Label>
              <Input
                id="fullName"
                value={formData.fullName}
                onChange={(e) =>
                  setFormData({ ...formData, fullName: e.target.value })
                }
                placeholder="Nguyen Van A"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="cardNumber">Card Number</Label>
              <Input
                id="cardNumber"
                value={formData.cardNumber}
                onChange={(e) =>
                  setFormData({ ...formData, cardNumber: e.target.value })
                }
                placeholder="1234567890"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="password">Password</Label>
              <Input
                id="password"
                type="password"
                value={formData.password}
                onChange={(e) =>
                  setFormData({ ...formData, password: e.target.value })
                }
                placeholder="****"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input
                id="email"
                type="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                placeholder="john.doe@company.com"
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="phoneNumber">Phone Number</Label>
              <Input
                id="phoneNumber"
                value={formData.phoneNumber}
                onChange={(e) =>
                  setFormData({ ...formData, phoneNumber: e.target.value })
                }
                placeholder="+1234567890"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="department">Department</Label>
              <Input
                id="department"
                value={formData.department}
                onChange={(e) =>
                  setFormData({ ...formData, department: e.target.value })
                }
                placeholder="IT"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="privilege">Privilege</Label>
              <Select defaultValue={formData.privilege?.toString()} onValueChange={(value) => setFormData({ ...formData, privilege: Number(value) })}>
                <SelectTrigger className="">
                  <SelectValue placeholder="Select a fruit" />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    {
                      Object.keys(UserPrivileges).map(key => {
                        return <SelectItem key={key} value={String(UserPrivileges[key as keyof typeof UserPrivileges])}>{key}</SelectItem>
                      })
                    }
                    
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>
            
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="devices">Devices *</Label>
              <Select 
                defaultValue={formData.DeviceIds?.toString()} 
                onValueChange={(value) => setFormData({ ...formData, DeviceIds: [value] })}
                required>
                <SelectTrigger className="">
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectGroup>
                    {
                      isFetching ? null : devices?.map(device => {
                        return <SelectItem key={device.id} value={device.id}>{device.deviceName}</SelectItem>
                      })
                    }
                    
                  </SelectGroup>
                </SelectContent>
              </Select>
            </div>
            
          </div>
          <div className="grid gap-4">
            <Button type="submit" className="ml-auto">
              Create User
            </Button>
            
          </div>
        </form>
    </DialogContent>    

    
    </Dialog>
  )}