// ==========================================
// src/pages/ManagerDashboard.tsx
// ==========================================
import { PageHeader } from '@/components/PageHeader'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { useDashboardSummary, useAttendanceTrends, useTopPerformers, useLateEmployees, useDepartmentStats, useDeviceStatus } from '@/hooks/useDashboard'
import { SummaryCards } from '@/components/dashboard/SummaryCards'
import { AttendanceTrendChart } from '@/components/dashboard/AttendanceTrendChart'
import { TopPerformersList } from '@/components/dashboard/TopPerformersList'
import { LateEmployeesList } from '@/components/dashboard/LateEmployeesList'
import { DepartmentStatsCard } from '@/components/dashboard/DepartmentStatsCard'
import { DeviceStatusList } from '@/components/dashboard/DeviceStatusList'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Button } from '@/components/ui/button'
import { RefreshCw } from 'lucide-react'

export const ManagerDashboard = () => {
  const trendDays = 30
  const performerCount = 10

  // Fetch all dashboard data
  const { data: summary, isLoading: summaryLoading, refetch: refetchSummary } = useDashboardSummary()
  const { data: trends, isLoading: trendsLoading } = useAttendanceTrends(trendDays)
  const { data: topPerformers, isLoading: performersLoading } = useTopPerformers({ count: performerCount })
  const { data: lateEmployees, isLoading: lateLoading } = useLateEmployees({ count: performerCount })
  const { data: departments, isLoading: deptLoading } = useDepartmentStats()
  const { data: devices, isLoading: devicesLoading } = useDeviceStatus()

  const isLoading = summaryLoading

  if (isLoading) {
    return <LoadingSpinner />
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <PageHeader
          title="Manager Dashboard"
          description="Overview of team attendance and performance metrics"
        />
        <Button
          variant="outline"
          size="sm"
          onClick={() => refetchSummary()}
          className="gap-2"
        >
          <RefreshCw className="w-4 h-4" />
          Refresh
        </Button>
      </div>

      {/* Summary Cards */}
      <SummaryCards summary={summary} isLoading={summaryLoading} />

      {/* Tabs for Different Views */}
      <Tabs defaultValue="overview" className="space-y-4">
        <TabsList>
          <TabsTrigger value="overview">Overview</TabsTrigger>
          <TabsTrigger value="performance">Performance</TabsTrigger>
          <TabsTrigger value="departments">Departments</TabsTrigger>
          <TabsTrigger value="devices">Devices</TabsTrigger>
        </TabsList>

        {/* Overview Tab */}
        <TabsContent value="overview" className="space-y-4">
          <div className="grid gap-4 md:grid-cols-2">
            <AttendanceTrendChart data={trends} isLoading={trendsLoading} />
            <DeviceStatusList devices={devices} isLoading={devicesLoading} />
          </div>
          
          <DepartmentStatsCard departments={departments} isLoading={deptLoading} />
        </TabsContent>

        {/* Performance Tab */}
        <TabsContent value="performance" className="space-y-4">
          <div className="grid gap-4 md:grid-cols-2">
            <TopPerformersList performers={topPerformers} isLoading={performersLoading} />
            <LateEmployeesList employees={lateEmployees} isLoading={lateLoading} />
          </div>
        </TabsContent>

        {/* Departments Tab */}
        <TabsContent value="departments" className="space-y-4">
          <DepartmentStatsCard departments={departments} isLoading={deptLoading} />
        </TabsContent>

        {/* Devices Tab */}
        <TabsContent value="devices" className="space-y-4">
          <DeviceStatusList devices={devices} isLoading={devicesLoading} />
        </TabsContent>
      </Tabs>
    </div>
  )
}
