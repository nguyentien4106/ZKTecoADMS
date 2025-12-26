// ==========================================
// src/components/dialogs/EmployeeAccountRequestDialog.tsx
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
import { Loader2, UserCog, UserPlus } from 'lucide-react'
import { useDeviceUserContext } from '@/contexts/DeviceUserContext'
import { PasswordInput, validatePassword } from '@/components/PasswordInput'

interface FormData {
  firstName: string
  lastName: string
  userName: string
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
  userName?: string
}

const initialFormData: FormData = {
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: '',
  phoneNumber: '',
  userName: '',
}

export const EmployeeAccountRequestDialog = () => {
  const {
    createAccountDialogOpen,
    employeeForAccount,
    isCreatingAccount,
    setCreateAccountDialogOpen,
    handleCreateAccountSubmit
  } = useDeviceUserContext()
  const [formData, setFormData] = useState<FormData>(initialFormData)
  const [errors, setErrors] = useState<FormErrors>({})
  
  const isUpdateMode = !! employeeForAccount?.applicationUser

  // Pre-fill form with existing account data when in update mode
  useEffect(() => {
    if (createAccountDialogOpen && employeeForAccount?.applicationUser) {
      setFormData({
        firstName: employeeForAccount.applicationUser.firstName || '',
        lastName: employeeForAccount.applicationUser.lastName || '',
        email: employeeForAccount.applicationUser.email || '',
        password: '',
        confirmPassword: '',
        phoneNumber: employeeForAccount.applicationUser.phoneNumber || '',
        userName: employeeForAccount.applicationUser.userName || '',
      })
    } else if (createAccountDialogOpen) {
      setFormData(initialFormData)
    }
  }, [createAccountDialogOpen, employeeForAccount])

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
      } else {
        const passwordError = validatePassword(formData.password)
        if (passwordError) {
          newErrors.password = passwordError
        }
      }
      
      if (formData.password !== formData.confirmPassword) {
        newErrors.confirmPassword = 'Passwords do not match'
      }
    } else if (formData.password) {
      // If password is provided in update mode, validate it
      const passwordError = validatePassword(formData.password)
      if (passwordError) {
        newErrors.password = passwordError
      }
      if (formData.password !== formData.confirmPassword) {
        newErrors.confirmPassword = 'Passwords do not match'
      }
    }
    
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors)
      return
    }
    
    if (!employeeForAccount?.id) return
    
    try {
      await handleCreateAccountSubmit({
        employeeDeviceId: employeeForAccount.id, 
        firstName: formData.firstName, 
        lastName: formData.lastName, 
        email: formData.email, 
        password: formData.password, 
        phoneNumber: formData.phoneNumber,
        userName: formData.userName
      })
      // Reset form on success
      setFormData(initialFormData)
      setErrors({})
      setCreateAccountDialogOpen(false)
    } catch (error) {
      // Error handling is done in parent component
    }
  }

  const handleClose = () => {
    setFormData(initialFormData)
    setErrors({})
    setCreateAccountDialogOpen(false)
  }

  return (
    <Dialog open={createAccountDialogOpen} onOpenChange={handleClose}>
      <DialogContent className="sm:max-w-[600px] max-h-[90vh] overflow-y-auto">
        <form onSubmit={handleSubmit}>
          <DialogHeader>
            <DialogTitle className="flex items-center gap-2">
              {isUpdateMode ? (
                <>
                  <UserCog className="w-5 h-5" />
                  Update Account for Employee
                </>
              ) : (
                <>
                  <UserPlus className="w-5 h-5" />
                  Create Account for Employee
                </>
              )}
            </DialogTitle>
            <DialogDescription>
              {isUpdateMode ? 'Update' : 'Create'} a login account for <strong>{employeeForAccount?.name}</strong> (PIN: {employeeForAccount?.pin})
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
                  disabled={isCreatingAccount}
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
                  disabled={isCreatingAccount}
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
                disabled={isCreatingAccount || isUpdateMode}
                className={errors.email ? 'border-destructive' : ''}
              />
              {errors.email && (
                <p className="text-sm text-destructive">{errors.email}</p>
              )}
            </div>

            <div className="grid gap-2">
              <Label htmlFor="userName">
                UserName <span className="text-destructive">*</span> 
                
              </Label>
              <Input
                id="userName"
                placeholder="john.doe@example.com"
                value={formData.userName}
                onChange={(e) => updateField('userName', e.target.value)}
                className={errors.email ? 'border-destructive' : ''}
              />
              {errors.email && (
                <p className="text-sm text-destructive">{errors.userName}</p>
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
                disabled={isCreatingAccount}
              />
            </div>

            <PasswordInput
              password={formData.password}
              confirmPassword={formData.confirmPassword}
              onPasswordChange={(value) => updateField('password', value)}
              onConfirmPasswordChange={(value) => updateField('confirmPassword', value)}
              disabled={isCreatingAccount}
              isOptional={isUpdateMode}
              passwordError={errors.password}
              confirmPasswordError={errors.confirmPassword}
              showRequirements={!isUpdateMode}
            />

          </div>

          <DialogFooter>
            <Button
              type="button"
              variant="outline"
              onClick={handleClose}
              disabled={isCreatingAccount}
            >
              Cancel
            </Button>
            <Button type="submit" disabled={isCreatingAccount}>
              {isCreatingAccount && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isUpdateMode ? 'Update Account' : 'Create Account'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
