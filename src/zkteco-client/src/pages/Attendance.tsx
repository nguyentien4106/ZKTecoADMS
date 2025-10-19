
// ==========================================
// src/pages/Attendance.tsx
// ==========================================
import { PageHeader } from '@/components/PageHeader'
import { Card, CardContent } from '@/components/ui/card'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { LoadingSpinner } from '@/components/LoadingSpinner'
import { useUnprocessedAttendance } from '@/hooks/useAttendance'
import { format } from 'date-fns'
import { Clock, Calendar } from 'lucide-react'

const verifyTypeMap: { [key: number]: string } = {
  0: 'Password',
  1: 'Fingerprint',
  2: 'Card',
  15: 'Face',
}

const verifyStateMap: { [key: number]: string } = {
  0: 'Check In',
  1: 'Check Out',
  2: 'Break Out',
  3: 'Break In',
}

export const Attendance = () => {
  const { data: logs, isLoading } = useUnprocessedAttendance()

  if (isLoading) {
    return <LoadingSpinner />
  }

  return (
    <div>
      <PageHeader
        title="Attendance Logs"
        description="View and manage attendance records"
      />

      <Card>
        <CardContent className="p-0">
          {!logs || logs.length === 0 ? (
            <div className="text-center py-12">
              <Clock className="w-12 h-12 mx-auto text-muted-foreground mb-4" />
              <h3 className="text-lg font-semibold mb-2">No attendance logs</h3>
              <p className="text-muted-foreground">
                Attendance logs will appear here when devices push data
              </p>
            </div>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Date & Time</TableHead>
                  <TableHead>PIN</TableHead>
                  <TableHead>User Name</TableHead>
                  <TableHead>Device</TableHead>
                  <TableHead>Verify Type</TableHead>
                  <TableHead>State</TableHead>
                  <TableHead>Temperature</TableHead>
                  <TableHead>Status</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {logs.map((log) => (
                  <TableRow key={log.id}>
                    <TableCell>
                      <div className="flex items-center gap-2">
                        <Calendar className="w-4 h-4 text-muted-foreground" />
                        <span>
                          {format(
                            new Date(log.attendanceTime),
                            'MMM dd, yyyy HH:mm:ss'
                          )}
                        </span>
                      </div>
                    </TableCell>
                    <TableCell className="font-mono">{log.pin}</TableCell>
                    <TableCell className="font-medium">
                      {log.user?.fullName || '-'}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {log.device?.deviceName || '-'}
                    </TableCell>
                    <TableCell>
                      <Badge variant="outline">
                        {verifyTypeMap[log.verifyType || 0] || 'Unknown'}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <Badge
                        variant={log.verifyState === 0 ? 'success' : 'secondary'}
                      >
                        {verifyStateMap[log.verifyState] || 'Unknown'}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      {log.temperature ? `${log.temperature}°C` : '-'}
                    </TableCell>
                    <TableCell>
                      <Badge variant={log.isProcessed ? 'success' : 'warning'}>
                        {log.isProcessed ? 'Processed' : 'Pending'}
                      </Badge>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  )
}