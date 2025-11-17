// ==========================================
// src/components/dialogs/CreateemployeeAccountDialog.tsx
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
import { Loader2, CheckCircle2, Circle, UserCog, UserPlus } from 'lucide-react'
import { useEmployeeContext } from '@/contexts/EmployeeContext'

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

const validatePassword = (password: string): string | null => {
  if (!password) return null
  
  if (password.length < 8) {
    return 'Password must be at least 8 characters'
  }
  
  if (!/[0-9]/.test(password)) {
    return 'Password must contain at least one digit'
  }
  
  if (!/[a-z]/.test(password)) {
    return 'Password must contain at least one lowercase letter'
  }
  
  if (!/[A-Z]/.test(password)) {
    return 'Password must contain at least one uppercase letter'
  }
  
  if (!/[^a-zA-Z0-9]/.test(password)) {
    return 'Password must contain at least one special character'
  }
  
  return null
}

const initialFormData: FormData = {
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: '',
  phoneNumber: '',
}

export const CreateEmployeeAccountDialog = () => {
  const {
    createAccountDialogOpen,
    employeeForAccount,
    isCreatingAccount,
    setCreateAccountDialogOpen,
    handleCreateAccountSubmit
  } = useEmployeeContext()
  const [formData, setFormData] = useState<FormData>(initialFormData)
  const [errors, setErrors] = useState<FormErrors>({})
  const [showPasswordRequirements, setShowPasswordRequirements] = useState(false)
  
  const isUpdateMode = !! employeeForAccount?.applicationUser

  // Password requirements state
  const passwordRequirements = {
    minLength: formData.password.length >= 8,
    hasUppercase: /[A-Z]/.test(formData.password),
    hasLowercase: /[a-z]/.test(formData.password),
    hasDigit: /[0-9]/.test(formData.password),
    hasSpecial: /[^a-zA-Z0-9]/.test(formData.password),
  }

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
      await handleCreateAccountSubmit(
        employeeForAccount.id, 
        formData.firstName, 
        formData.lastName, 
        formData.email, 
        formData.password, 
        formData.phoneNumber
      )
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
                  onFocus={() => setShowPasswordRequirements(true)}
                  disabled={isCreatingAccount}
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
                  disabled={isCreatingAccount}
                  className={errors.confirmPassword ? 'border-destructive' : ''}
                />
                {errors.confirmPassword && (
                  <p className="text-sm text-destructive">{errors.confirmPassword}</p>
                )}
              </div>
            </div>

            {!isUpdateMode && (showPasswordRequirements || formData.password) && (
              <div className="space-y-2 text-xs bg-muted/30 p-3 rounded-lg border border-border/50">
                <p className="font-semibold text-foreground mb-2">Password must contain:</p>
                <div className="space-y-1.5">
                  <div className="flex items-center gap-2">
                    {passwordRequirements.minLength ? (
                      <CheckCircle2 className="w-4 h-4 text-green-600 dark:text-green-500 flex-shrink-0" />
                    ) : (
                      <Circle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                    )}
                    <span className={passwordRequirements.minLength ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}>
                      At least 8 characters
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    {passwordRequirements.hasUppercase ? (
                      <CheckCircle2 className="w-4 h-4 text-green-600 dark:text-green-500 flex-shrink-0" />
                    ) : (
                      <Circle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                    )}
                    <span className={passwordRequirements.hasUppercase ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}>
                      One uppercase letter (A-Z)
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    {passwordRequirements.hasLowercase ? (
                      <CheckCircle2 className="w-4 h-4 text-green-600 dark:text-green-500 flex-shrink-0" />
                    ) : (
                      <Circle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                    )}
                    <span className={passwordRequirements.hasLowercase ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}>
                      One lowercase letter (a-z)
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    {passwordRequirements.hasDigit ? (
                      <CheckCircle2 className="w-4 h-4 text-green-600 dark:text-green-500 flex-shrink-0" />
                    ) : (
                      <Circle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                    )}
                    <span className={passwordRequirements.hasDigit ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}>
                      One digit (0-9)
                    </span>
                  </div>
                  <div className="flex items-center gap-2">
                    {passwordRequirements.hasSpecial ? (
                      <CheckCircle2 className="w-4 h-4 text-green-600 dark:text-green-500 flex-shrink-0" />
                    ) : (
                      <Circle className="w-4 h-4 text-muted-foreground flex-shrink-0" />
                    )}
                    <span className={passwordRequirements.hasSpecial ? 'text-green-600 dark:text-green-500' : 'text-muted-foreground'}>
                      One special character (!@#$%^&*)
                    </span>
                  </div>
                </div>
              </div>
            )}

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
