// ==========================================
// src/components/device-commands/CommandHistoryTable.tsx
// ==========================================
import { DeviceCommand } from '@/types'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table'
import { Badge } from '@/components/ui/badge'
import { getStatusBadge, formatDate } from './commandUtils.tsx'
import { DeviceCommandTypes } from '@/types/device.ts'

interface CommandHistoryTableProps {
  commands: DeviceCommand[]
}

export const CommandHistoryTable = ({ commands }: CommandHistoryTableProps) => {
    console.log('Rendering CommandHistoryTable with commands:', commands);
  return (
    <div className="rounded-md border">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Command</TableHead>
            <TableHead>Priority</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Created</TableHead>
            <TableHead>Sent</TableHead>
            <TableHead>Completed</TableHead>
            <TableHead>Error Message</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {commands.map((command: DeviceCommand) => (
            <TableRow key={command.id}>
              <TableCell className="font-medium">
                {DeviceCommandTypes[command.commandType]}
              </TableCell>
              <TableCell>
                <Badge variant={command.priority >= 5 ? 'destructive' : 'secondary'}>
                  {command.priority}
                </Badge>
              </TableCell>
              <TableCell>
                {getStatusBadge(command.status)}
              </TableCell>
              <TableCell className="text-muted-foreground text-sm">
                {formatDate(command.createdAt)}
              </TableCell>
              <TableCell className="text-muted-foreground text-sm">
                {formatDate(command.sentAt)}
              </TableCell>
              <TableCell className="text-muted-foreground text-sm">
                {formatDate(command.completedAt)}
              </TableCell>
              <TableCell className="text-destructive text-sm">
                {command.errorMessage || '-'}
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  )
}
