// ==========================================
// src/contexts/EmployeeContext.tsx
// ==========================================
import { createContext, useContext, useState, useEffect, ReactNode } from 'react'
import { toast } from 'sonner'
import {
  useEmployees,
  useDeleteEmployee,
  useCreateEmployee,
  useUpdateEmployee,
  useCreateEmployeeAccount,
  useUpdateEmployeeAccount,
} from '@/hooks/useEmployees'
import { useDevices } from '@/hooks/useDevices'
import { Employee, CreateEmployeeRequest, UpdateEmployeeRequest } from '@/types/employee'
import { Account, CreateEmployeeAccountRequest } from '@/types/account'
import { da } from 'date-fns/locale'

interface EmployeeContextValue {
  // State
  employees: Employee[] | undefined
  isLoading: boolean
  devices: any[] | undefined
  selectedDeviceIds: string[]
  
  // Dialog states
  createDialogOpen: boolean
  createAccountDialogOpen: boolean
  deleteDialogOpen: boolean
  employeeToEdit: Employee | null
  employeeForAccount: Employee | null
  employeeToDelete: Employee | null
  isCreatingAccount: boolean
  
  // Actions
  setCreateDialogOpen: (open: boolean) => void
  setCreateAccountDialogOpen: (open: boolean) => void
  setDeleteDialogOpen: (open: boolean) => void
  setSelectedDeviceIds: (deviceIds: string[]) => void
  handleDelete: (employee: Employee) => void
  handleConfirmDelete: () => Promise<void>
  handleAddEmployee: (data: CreateEmployeeRequest) => Promise<void>
  handleUpdateEmployee: (data: UpdateEmployeeRequest) => Promise<void>
  handleEdit: (user: Employee) => void
  handleCreateAccount: (user: Employee) => void
  handleCreateAccountSubmit: (data: CreateEmployeeAccountRequest) => Promise<Account | undefined>
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
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false)
  const [employeeToEdit, setEmployeeToEdit] = useState<Employee | null>(null)
  const [employeeForAccount, setEmployeeForAccount] = useState<Employee | null>(null)
  const [employeeToDelete, setEmployeeToDelete] = useState<Employee | null>(null)
  const [isCreatingAccount, setIsCreatingAccount] = useState(false)
  const [selectedDeviceIds, setSelectedDeviceIds] = useState<string[]>([])
  
  // Hooks
  const { data: devices } = useDevices()
  const { data: employees, isLoading } = useEmployees(selectedDeviceIds)
  const deleteEmployee = useDeleteEmployee()
  const createEmployee = useCreateEmployee()
  const updateEmployee = useUpdateEmployee()
  const createEmployeeAccount = useCreateEmployeeAccount()
  const updateEmployeeAccount = useUpdateEmployeeAccount()

  // Initialize selected devices
  useEffect(() => {
    if (devices) {
      setSelectedDeviceIds(devices.map((device) => device.id))
    }
  }, [devices])

  // Wrapper for setCreateDialogOpen to clear user state when closing
  const setCreateDialogOpen = (open: boolean) => {
    if (!open) {
      setEmployeeToEdit(null) // Clear user data when dialog closes
    }
    setCreateDialogOpenState(open)
  }

  // Handlers
  const handleDelete = (employee: Employee) => {
    setEmployeeToDelete(employee)
    setDeleteDialogOpen(true)
  }

  const handleConfirmDelete = async () => {
    if (!employeeToDelete?.id) return
    await deleteEmployee.mutateAsync(employeeToDelete.id)
    setDeleteDialogOpen(false)
    setEmployeeToDelete(null)
  }

  const handleAddEmployee = async (data: CreateEmployeeRequest) => {
    const results = await createEmployee.mutateAsync(data)
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

  const handleUpdateEmployee = async (data: UpdateEmployeeRequest) => {
    try {
      await updateEmployee.mutateAsync(data)
    } catch (error: any) {
      toast.error('Failed to update employee', {
        description: error.message || 'An error occurred',
      })
    }
  }

  const handleEdit = (user: Employee) => {
    setEmployeeToEdit(user)
    setCreateDialogOpen(true)
  }

  const handleOpenCreateDialog = () => {
    setEmployeeToEdit(null) // Clear any previous user data
    setCreateDialogOpen(true)
  }

  const handleFilterSubmit = (deviceIds: string[]) => {
    setSelectedDeviceIds(deviceIds)
  }

  const handleCreateAccount = (user: Employee) => {
    setEmployeeForAccount(user)
    setCreateAccountDialogOpen(true)
  }

  const handleCreateAccountSubmit = async (data: CreateEmployeeAccountRequest) => {
    setIsCreatingAccount(true)
    try {
      // Check if the user already has an account
      const isUpdateMode = !!employeeForAccount?.applicationUser
      let updatedEmployeeAccount: Account | undefined = undefined;
      if (isUpdateMode) {
        // Update existing account
        
        updatedEmployeeAccount = await updateEmployeeAccount.mutateAsync({
          employeeDeviceId: employeeForAccount!.id,
          data: {
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            phoneNumber: data.phoneNumber,
            password: data.password,
            userName: data.userName
          }
        })
      } else {
        // Create new account
        updatedEmployeeAccount = await createEmployeeAccount.mutateAsync(data)
      }

      setCreateAccountDialogOpen(false)
      setEmployeeForAccount(null)

      return updatedEmployeeAccount;

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
    employees,
    isLoading,
    devices,
    selectedDeviceIds,
    
    // Dialog states
    createDialogOpen,
    createAccountDialogOpen,
    deleteDialogOpen,
    employeeToEdit,
    employeeForAccount,
    employeeToDelete,
    isCreatingAccount,
    
    // Actions
    setCreateDialogOpen,
    setCreateAccountDialogOpen,
    setDeleteDialogOpen,
    setSelectedDeviceIds,
    handleDelete,
    handleConfirmDelete,
    handleAddEmployee,
    handleUpdateEmployee,
    handleEdit,
    handleOpenCreateDialog,
    handleCreateAccount,
    handleCreateAccountSubmit,
    handleFilterSubmit,
    
    // Mutation states
    isDeletePending: deleteEmployee.isPending,
  }

  return (
    <EmployeeContext.Provider value={value}>
      {children}
    </EmployeeContext.Provider>
  )
}
