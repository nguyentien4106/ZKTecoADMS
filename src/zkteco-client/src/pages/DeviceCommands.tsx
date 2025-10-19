// ==========================================
// src/pages/Dashboard.tsx
// ==========================================
import { PageHeader } from '@/components/PageHeader'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { useDevices } from '@/hooks/useDevices'
import { useUsers } from '@/hooks/useUsers'
import { useUnprocessedAttendance } from '@/hooks/useAttendance'
import { Monitor, Users, Clock, AlertCircle } from 'lucide-react'
import { Badge } from '@/components/ui/badge'
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts'

export const DeviceCommands = () => {
  const { data: devices, isLoading: devicesLoading } = useDevices()
  const { data: users, isLoading: usersLoading } = useUsers()
  const { data: unprocessedLogs } = useUnprocessedAttendance()

  if (devicesLoading || usersLoading) {
    return <LoadingSpinner />
  }

  const activeDevices = devices?.filter((d) => d.isActive && d.deviceStatus === 'Online').length || 0
  const totalDevices = devices?.length || 0
  const totalUsers = users?.length || 0
  const activeUsers = users?.filter((u) => u.isActive).length || 0

  // Mock data for chart
  const chartData = [
    { name: 'Mon', attendance: 45 },
    { name: 'Tue', attendance: 52 },
    { name: 'Wed', attendance: 48 },
    { name: 'Thu', attendance: 61 },
    { name: 'Fri', attendance: 55 },
    { name: 'Sat', attendance: 23 },
    { name: 'Sun', attendance: 12 },
  ]

  return (
    <div>
      <PageHeader
        title="Dashboard"
        description="Overview of your ZKTeco attendance system"
      />

      {/* Stats Cards */}
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4 mb-6">
        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Total Devices</CardTitle>
            <Monitor className="w-4 h-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{totalDevices}</div>
            <div className="flex items-center gap-2 mt-2">
              <Badge variant="success">{activeDevices} Online</Badge>
              {totalDevices - activeDevices > 0 && (
                <Badge variant="secondary">
                  {totalDevices - activeDevices} Offline
                </Badge>
              )}
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">Total Users</CardTitle>
            <Users className="w-4 h-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">{totalUsers}</div>
            <p className="text-xs text-muted-foreground mt-2">
              {activeUsers} active users
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">
              Today's Attendance
            </CardTitle>
            <Clock className="w-4 h-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">156</div>
            <p className="text-xs text-muted-foreground mt-2">
              +12% from yesterday
            </p>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium">
              Unprocessed Logs
            </CardTitle>
            <AlertCircle className="w-4 h-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <div className="text-2xl font-bold">
              {unprocessedLogs?.length || 0}
            </div>
            <p className="text-xs text-muted-foreground mt-2">
              Awaiting processing
            </p>
          </CardContent>
        </Card>
      </div>

      {/* Charts and Recent Activity */}
      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle>Attendance Trend</CardTitle>
          </CardHeader>
          <CardContent>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={chartData}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip />
                <Line
                  type="monotone"
                  dataKey="attendance"
                  stroke="hsl(var(--primary))"
                  strokeWidth={2}
                />
              </LineChart>
            </ResponsiveContainer>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Device Status</CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {devices?.slice(0, 5).map((device) => (
                <div
                  key={device.id}
                  className="flex items-center justify-between"
                >
                  <div className="flex items-center gap-3">
                    <div
                      className={`w-2 h-2 rounded-full ${
                        device.deviceStatus === 'Online'
                          ? 'bg-green-500'
                          : 'bg-gray-400'
                      }`}
                    />
                    <div>
                      <p className="font-medium">{device.deviceName}</p>
                      <p className="text-xs text-muted-foreground">
                        {device.serialNumber}
                      </p>
                    </div>
                  </div>
                  <Badge
                    variant={
                      device.deviceStatus === 'Online' ? 'success' : 'secondary'
                    }
                  >
                    {device.deviceStatus}
                  </Badge>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}