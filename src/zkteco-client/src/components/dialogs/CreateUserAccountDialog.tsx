// ==========================================
// src/components/dialogs/CreateUserAccountDialog.tsx
// ==========================================
import { useState, useEffect } from 'react'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Loader2, UserPlus, UserCog } from 'lucide-react'
import { Employee } from '@/types/employee'

interface CreateUserAccountDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  user: Employee | null
  onSubmit: (
    userId: string, 
    firstName: string, 
    lastName: string, 
    email: string, 
    password: string,
    phoneNumber?: string
  ) => Promise<void>
  isLoading?: boolean
}

interface FormData {
  firstName: string
  lastName: string
  email: string
  password: string
  confirmPassword: string
  phoneNumber: string
}

interface FormErrors {
  firstName?: string
  lastName?: string
  email?: string
  password?: string
  confirmPassword?: string
}

const initialFormData: FormData = {
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: '',
  phoneNumber: '',
}

export const CreateUserAccountDialog = ({
  open,
  onOpenChange,
  user,
  onSubmit,
  isLoading = false,
}: CreateUserAccountDialogProps) => {
  const [formData, setFormData] = useState<FormData>(initialFormData)
  const [errors, setErrors] = useState<FormErrors>({})
  
  const isUpdateMode = !!user?.applicationUser

  // Pre-fill form with existing account data when in update mode
  useEffect(() => {
    if (open && user?.applicationUser) {
      setFormData({
        firstName: user.applicationUser.firstName || '',
        lastName: user.applicationUser.lastName || '',
        email: user.applicationUser.email || '',
        password: '',
        confirmPassword: '',
        phoneNumber: user.applicationUser.phoneNumber || '',
      })
    } else if (open) {
      setFormData(initialFormData)
    }
  }, [open, user])

  const updateField = (field: keyof FormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }))
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    
    // Reset errors
    setErrors({})
    
    // Validation
    const newErrors: FormErrors = {}
    
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'First name is required'
    }
    
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required'
    }
    
    if (!formData.email) {
      newErrors.email = 'Email is required'
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Invalid email format'
    }
    
    // Password is optional in update mode
    if (!isUpdateMode) {
      if (!formData.password) {
        newErrors.password = 'Password is required'
      } else if (formData.password.length < 6) {
        newErrors.password = 'Password must be at least 6 characters'
      }
      
      if (formData.password !== formData.confirmPassword) {
        newErrors.confirmPassword = 'Passwords do not match'
      }
    } else if (formData.password) {
      // If password is provided in update mode, validate it
      if (formData.password.length < 6) {
        newErrors.password = 'Password must be at least 6 characters'
      }
      if (formData.password !== formData.confirmPassword) {
        newErrors.confirmPassword = 'Passwords do not match'
      }
    }
    
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors)
      return
    }
    
    if (!user?.id) return
    
    try {
      await onSubmit(
        user.id, 
        formData.firstName, 
        formData.lastName, 
        formData.email, 
        formData.password, 
        formData.phoneNumber
      )
      // Reset form on success
      setFormData(initialFormData)
      setErrors({})
      onOpenChange(false)
    } catch (error) {
      // Error handling is done in parent component
    }
  }

  const handleClose = () => {
    setFormData(initialFormData)
    setErrors({})
    onOpenChange(false)
  }

  return (
    <Dialog open={open} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle className="flex items-center gap-2">
              {isUpdateMode ? (
                <>
                  <UserCog className="w-5 h-5" />
                  Update Account for User
                </>
              ) : (
                <>
                  <UserPlus className="w-5 h-5" />
                  Create Account for User
                </>
              )}
            </DialogTitle>
            <DialogDescription>
              {isUpdateMode ? 'Update' : 'Create'} a login account for <strong>{user?.name}</strong> (PIN: {user?.pin})
            </DialogDescription>
          </DialogHeader>

          <div className="grid gap-4 py-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="firstName">
                  First Name <span className="text-destructive">*</span>
                </Label>
                <Input
                  id="firstName"
                  type="text"
                  placeholder="John"
                  value={formData.firstName}
                  onChange={(e) => updateField('firstName', e.target.value)}
                  disabled={isLoading}
                  className={errors.firstName ? 'border-destructive' : ''}
                />
                {errors.firstName && (
                  <p className="text-sm text-destructive">{errors.firstName}</p>
                )}
              </div>

              <div className="grid gap-2">
                <Label htmlFor="lastName">
                  Last Name <span className="text-destructive">*</span>
                </Label>
                <Input
                  id="lastName"
                  type="text"
                  placeholder="Doe"
                  value={formData.lastName}
                  onChange={(e) => updateField('lastName', e.target.value)}
                  disabled={isLoading}
                  className={errors.lastName ? 'border-destructive' : ''}
                />
                {errors.lastName && (
                  <p className="text-sm text-destructive">{errors.lastName}</p>
                )}
              </div>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="email">
                Email <span className="text-destructive">*</span>
                {isUpdateMode && <span className="text-sm text-muted-foreground ml-2">(cannot be changed)</span>}
              </Label>
              <Input
                id="email"
                type="email"
                placeholder="john.doe@example.com"
                value={formData.email}
                onChange={(e) => updateField('email', e.target.value)}
                disabled={isLoading || isUpdateMode}
                className={errors.email ? 'border-destructive' : ''}
              />
              {errors.email && (
                <p className="text-sm text-destructive">{errors.email}</p>
              )}
            </div>

            <div className="grid gap-2">
              <Label htmlFor="phoneNumber">
                Phone Number
              </Label>
              <Input
                id="phoneNumber"
                type="tel"
                placeholder="+1234567890"
                value={formData.phoneNumber}
                onChange={(e) => updateField('phoneNumber', e.target.value)}
                disabled={isLoading}
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="password">
                  Password {!isUpdateMode && <span className="text-destructive">*</span>}
                  {isUpdateMode && <span className="text-sm text-muted-foreground">(leave blank to keep current)</span>}
                </Label>
                <Input
                  id="password"
                  type="password"
                  placeholder="••••••••"
                  value={formData.password}
                  onChange={(e) => updateField('password', e.target.value)}
                  disabled={isLoading}
                  className={errors.password ? 'border-destructive' : ''}
                />
                {errors.password && (
                  <p className="text-sm text-destructive">{errors.password}</p>
                )}
              </div>

              <div className="grid gap-2">
                <Label htmlFor="confirmPassword">
                  Confirm Password {!isUpdateMode && <span className="text-destructive">*</span>}
                </Label>
                <Input
                  id="confirmPassword"
                  type="password"
                  placeholder="••••••••"
                  value={formData.confirmPassword}
                  onChange={(e) => updateField('confirmPassword', e.target.value)}
                  disabled={isLoading}
                  className={errors.confirmPassword ? 'border-destructive' : ''}
                />
                {errors.confirmPassword && (
                  <p className="text-sm text-destructive">{errors.confirmPassword}</p>
                )}
              </div>
            </div>

            <div className="rounded-md bg-muted p-3 text-sm">
              <p className="font-medium mb-1">Account Details:</p>
              <ul className="list-disc list-inside space-y-1 text-muted-foreground">
                <li>Role: Employee</li>
                <li>Access: Dashboard, Attendance, Settings</li>
                <li>Can view own attendance records</li>
              </ul>
            </div>
          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={isLoading}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isLoading}>
              {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isUpdateMode ? 'Update Account' : 'Create Account'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
