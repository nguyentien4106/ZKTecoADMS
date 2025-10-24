import {
  Table,
  TableBody,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { DeviceTableRow } from './DeviceTableRow'
import { Device } from '@/types'

interface DevicesTableProps {
  devices: Device[]
  onDelete: (id: string) => void
  onToggleActive: (id: string) => void
  onShowInfo?: (id: string) => void

}

export const DevicesTable = ({
  devices,
  onDelete,
  onToggleActive,
  onShowInfo,
}: DevicesTableProps) => {
  return (
    <Table>
      <TableHeader>
        <TableRow>
          <TableHead>Device Name</TableHead>
          <TableHead>Serial Number</TableHead>
          <TableHead>Model</TableHead>
          <TableHead>IP Address</TableHead>
          <TableHead>Location</TableHead>
          <TableHead>Status</TableHead>
          <TableHead>Is Active</TableHead>
          <TableHead>Last Online</TableHead>
          <TableHead className="text-right">Actions</TableHead>
        </TableRow>
      </TableHeader>
      <TableBody>
        {devices.map((device) => (
          <DeviceTableRow
            key={device.id}
            device={device}
            onDelete={onDelete}
            onToggleActive={onToggleActive}
            onShowInfo={onShowInfo}

          />
        ))}
      </TableBody>
    </Table>
  )
}
