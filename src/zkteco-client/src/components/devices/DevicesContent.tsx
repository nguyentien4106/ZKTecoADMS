import { Button } from '@/components/ui/button'
import { Card, CardContent } from '@/components/ui/card'
import { EmptyState } from '@/components/EmptyState'
import { DevicesTable } from './DevicesTable'
import { Monitor, Plus } from 'lucide-react'
import { Device } from '@/types'

interface DevicesContentProps {
  devices: Device[] | undefined
  onCreateDevice: () => void
  onDelete: (id: string) => void
  onToggleActive: (id: string) => void
  onShowInfo?: (id: string) => void
}

export const DevicesContent = ({
  devices,
  onCreateDevice,
  onDelete,
  onToggleActive,
  onShowInfo,
}: DevicesContentProps) => {
  return (
    <Card>
      <CardContent className="p-0">
        {!devices || devices.length === 0 ? (
          <EmptyState
            icon={Monitor}
            title="No devices found"
            description="Get started by adding your own device."
            action={
              <Button onClick={onCreateDevice}>
                <Plus className="w-4 h-4 mr-2" />
                Add Device
              </Button>
            }
          />
        ) : (
          <DevicesTable
            devices={devices}
            onDelete={onDelete}
            onToggleActive={onToggleActive}
            onShowInfo={onShowInfo}
          />
        )}
      </CardContent>
    </Card>
  )
}
