import { useQuery } from '@tanstack/react-query'
import { dashboardService } from '@/services/dashboardService'
import type {
  DashboardData,
  DashboardSummary,
  EmployeePerformance,
  DepartmentStatistics,
  AttendanceTrend,
  DeviceStatus,
  DashboardParams,
} from '@/types/dashboard'

// Get complete dashboard data
export const useDashboardData = (params?: DashboardParams) => {
  return useQuery({
    queryKey: ['dashboard', params],
    queryFn: async () => {
      const response = await dashboardService.getAll(params)
      return response.data
    },
  })
}

// Get today's summary
export const useDashboardSummary = () => {
  return useQuery({
    queryKey: ['dashboard', 'summary'],
    queryFn: async () => {
      const response = await dashboardService.getSummary()
      return response.data
    },
    refetchInterval: 60000, // Refetch every minute
  })
}

// Get top performers
export const useTopPerformers = (params?: {
  startDate?: string
  endDate?: string
  count?: number
  department?: string
}) => {
  return useQuery({
    queryKey: ['dashboard', 'top-performers', params],
    queryFn: async () => {
      const response = await dashboardService.getTopPerformers(params)
      return response.data
    },
  })
}

// Get late employees
export const useLateEmployees = (params?: {
  startDate?: string
  endDate?: string
  count?: number
  department?: string
}) => {
  return useQuery({
    queryKey: ['dashboard', 'late-employees', params],
    queryFn: async () => {
      const response = await dashboardService.getLateEmployees(params)
      return response.data
    },
  })
}

// Get department statistics
export const useDepartmentStats = (params?: {
  startDate?: string
  endDate?: string
}) => {
  return useQuery({
    queryKey: ['dashboard', 'department-stats', params],
    queryFn: async () => {
      const response = await dashboardService.getDepartmentStats(params)
      return response.data
    },
  })
}

// Get attendance trends
export const useAttendanceTrends = (days?: number) => {
  return useQuery({
    queryKey: ['dashboard', 'attendance-trends', days],
    queryFn: async () => {
      const response = await dashboardService.getAttendanceTrends(days)
      return response.data
    },
  })
}

// Get device status
export const useDeviceStatus = () => {
  return useQuery({
    queryKey: ['dashboard', 'device-status'],
    queryFn: async () => {
      const response = await dashboardService.getDeviceStatus()
      return response.data
    },
    refetchInterval: 60000, // Refetch every minute
  })
}

// Additional convenience hooks

// Get weekly dashboard data
export const useWeeklyDashboard = () => {
  return useQuery({
    queryKey: ['dashboard', 'weekly'],
    queryFn: async () => {
      const response = await dashboardService.getWeeklyData()
      return response.data
    },
  })
}

// Get monthly dashboard data
export const useMonthlyDashboard = () => {
  return useQuery({
    queryKey: ['dashboard', 'monthly'],
    queryFn: async () => {
      const response = await dashboardService.getMonthlyData()
      return response.data
    },
  })
}

// Get quarterly dashboard data
export const useQuarterlyDashboard = () => {
  return useQuery({
    queryKey: ['dashboard', 'quarterly'],
    queryFn: async () => {
      const response = await dashboardService.getQuarterlyData()
      return response.data
    },
  })
}

// Get dashboard by date range
export const useDashboardByDateRange = (
  startDate: string,
  endDate: string,
  department?: string
) => {
  return useQuery({
    queryKey: ['dashboard', 'date-range', startDate, endDate, department],
    queryFn: async () => {
      const response = await dashboardService.getByDateRange(startDate, endDate, department)
      return response.data
    },
    enabled: !!startDate && !!endDate,
  })
}

// Get dashboard by department
export const useDashboardByDepartment = (
  department: string,
  startDate?: string,
  endDate?: string
) => {
  return useQuery({
    queryKey: ['dashboard', 'department', department, startDate, endDate],
    queryFn: async () => {
      const response = await dashboardService.getByDepartment(department, startDate, endDate)
      return response.data
    },
    enabled: !!department,
  })
}

// Export types for convenience
export type {
  DashboardData,
  DashboardSummary,
  EmployeePerformance,
  DepartmentStatistics,
  AttendanceTrend,
  DeviceStatus,
  DashboardParams,
}

