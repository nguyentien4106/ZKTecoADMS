// ==========================================
// src/contexts/EmployeeContext.tsx
// ==========================================
import { createContext, useContext, useState, useEffect, ReactNode } from 'react'
import { toast } from 'sonner'
import {
  useUsers,
  useDeleteUser,
  useCreateUser,
  useUpdateUser,
} from '@/hooks/useEmployees'
import { useDevices } from '@/hooks/useDevices'
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '@/types/employee'
import { accountService } from '@/services/accountService'

interface EmployeeContextValue {
  // State
  users: Employee[] | undefined
  isLoading: boolean
  devices: any[] | undefined
  selectedDeviceIds: string[]
  
  // Dialog states
  createDialogOpen: boolean
  createAccountDialogOpen: boolean
  userToEdit: Employee | null
  employeeForAccount: Employee | null
  isCreatingAccount: boolean
  
  // Actions
  setCreateDialogOpen: (open: boolean) => void
  setCreateAccountDialogOpen: (open: boolean) => void
  setSelectedDeviceIds: (deviceIds: string[]) => void
  handleDelete: (userId: string) => Promise<void>
  handleAddUser: (data: CreateEmployeeRequest) => Promise<void>
  handleUpdateUser: (data: UpdateEmployeeRequest) => Promise<void>
  handleEdit: (user: Employee) => void
  handleCreateAccount: (user: Employee) => void
  handleCreateAccountSubmit: (
    userId: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phoneNumber?: string
  ) => Promise<void>
  handleFilterSubmit: (deviceIds: string[]) => void
  handleOpenCreateDialog: () => void
  
  // Mutation states
  isDeletePending: boolean
}

const EmployeeContext = createContext<EmployeeContextValue | undefined>(undefined)

export const useEmployeeContext = () => {
  const context = useContext(EmployeeContext)
  if (!context) {
    throw new Error('useEmployeeContext must be used within EmployeeProvider')
  }
  return context
}

interface EmployeeProviderProps {
  children: ReactNode
}

export const EmployeeProvider = ({ children }: EmployeeProviderProps) => {
  // Dialog states
  const [createDialogOpen, setCreateDialogOpenState] = useState(false)
  const [createAccountDialogOpen, setCreateAccountDialogOpen] = useState(false)
  const [userToEdit, setUserToEdit] = useState<Employee | null>(null)
  const [employeeForAccount, setEmployeeForAccount] = useState<Employee | null>(null)
  const [isCreatingAccount, setIsCreatingAccount] = useState(false)
  const [selectedDeviceIds, setSelectedDeviceIds] = useState<string[]>([])
  
  // Hooks
  const { data: devices } = useDevices()
  const { data: users, isLoading } = useUsers(selectedDeviceIds)
  const deleteUser = useDeleteUser()
  const createUser = useCreateUser()
  const updateUser = useUpdateUser()

  // Initialize selected devices
  useEffect(() => {
    if (devices) {
      setSelectedDeviceIds(devices.map((device) => device.id))
    }
  }, [devices])

  // Wrapper for setCreateDialogOpen to clear user state when closing
  const setCreateDialogOpen = (open: boolean) => {
    if (!open) {
      setUserToEdit(null) // Clear user data when dialog closes
    }
    setCreateDialogOpenState(open)
  }

  // Handlers
  const handleDelete = async (userId: string) => {
    if (!userId) return
    if (confirm('Are you sure you want to delete this employee?')) {
      await deleteUser.mutateAsync(userId)
    }
  }

  const handleAddUser = async (data: CreateEmployeeRequest) => {
    const results = await createUser.mutateAsync(data)
    const successes = results.filter((res) => res.isSuccess)
    const errors = results.filter((res) => !res.isSuccess)

    if (successes && successes.length) {
      toast.success('Employee added successfully to devices.', {
        description: successes.map((item) => item.data.deviceName).join(', '),
      })
    }
    if (errors && errors.length) {
      toast.error('Employee added failed to devices.', {
        description: errors.map((item) => item.data.deviceName).join(', '),
      })
    }
  }

  const handleUpdateUser = async (data: UpdateEmployeeRequest) => {
    try {
      await updateUser.mutateAsync(data)
      toast.success('Employee updated successfully')
    } catch (error: any) {
      toast.error('Failed to update employee', {
        description: error.message || 'An error occurred',
      })
    }
  }

  const handleEdit = (user: Employee) => {
    setUserToEdit(user)
    setCreateDialogOpen(true)
  }

  const handleOpenCreateDialog = () => {
    setUserToEdit(null) // Clear any previous user data
    setCreateDialogOpen(true)
  }

  const handleFilterSubmit = (deviceIds: string[]) => {
    setSelectedDeviceIds(deviceIds)
  }

  const handleCreateAccount = (user: Employee) => {
    setEmployeeForAccount(user)
    setCreateAccountDialogOpen(true)
  }

  const handleCreateAccountSubmit = async (
    userId: string,
    firstName: string,
    lastName: string,
    email: string,
    password: string,
    phoneNumber?: string
  ) => {
    setIsCreatingAccount(true)
    try {
      // Check if the user already has an account
      const isUpdateMode = !!employeeForAccount?.applicationUser

      if (isUpdateMode) {
        // Update existing account
        
        await accountService.updateUserAccount(
          userId,
            {
                firstName,
                lastName,
                email,
                phoneNumber,
                password
            })
        toast.success('Account updated successfully', {
          description: `Login account updated for ${employeeForAccount?.name}`,
        })
      } else {
        // Create new account
        await accountService.createUserAccount({
            employeeDeviceId:   userId,
            firstName,
            lastName,
            email,
            password,
            phoneNumber
        })
        toast.success('Account created successfully', {
          description: `Login account created for ${employeeForAccount?.name}`,
        })
      }

      setCreateAccountDialogOpen(false)
      setEmployeeForAccount(null)
    } catch (error: any) {
      const action = employeeForAccount?.applicationUser ? 'update' : 'create'
      toast.error(`Failed to ${action} account`, {
        description: error.message || 'An error occurred',
      })
    } finally {
      setIsCreatingAccount(false)
    }
  }

  const value: EmployeeContextValue = {
    // State
    users,
    isLoading,
    devices,
    selectedDeviceIds,
    
    // Dialog states
    createDialogOpen,
    createAccountDialogOpen,
    userToEdit,
    employeeForAccount,
    isCreatingAccount,
    
    // Actions
    setCreateDialogOpen,
    setCreateAccountDialogOpen,
    setSelectedDeviceIds,
    handleDelete,
    handleAddUser,
    handleUpdateUser,
    handleEdit,
    handleOpenCreateDialog,
    handleCreateAccount,
    handleCreateAccountSubmit,
    handleFilterSubmit,
    
    // Mutation states
    isDeletePending: deleteUser.isPending,
  }

  return (
    <EmployeeContext.Provider value={value}>
      {children}
    </EmployeeContext.Provider>
  )
}
