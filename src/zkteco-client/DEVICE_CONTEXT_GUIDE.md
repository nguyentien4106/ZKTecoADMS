# Device Context Implementation

## Overview
Converted device management logic to use React Context pattern for better state management and code organization. The DeviceContext centralizes all device-related actions and their loading states.

## Benefits

### 1. **Centralized Logic**
- All device actions are in one place
- Easier to maintain and update
- Consistent error handling across the app

### 2. **Cleaner Components**
- Pages and components are simpler
- No need to pass multiple hooks through props
- Reduced prop drilling

### 3. **Loading States**
- Centralized loading state management
- Easy to show loading indicators
- Better UX with disabled states during operations

### 4. **Reusability**
- Any component can access device actions
- Consistent behavior across the app
- Easy to add new actions

## File Structure

```
src/
├── contexts/
│   └── DeviceContext.tsx       # Device context and provider
├── hooks/
│   └── useDevices.ts           # React Query hooks
├── pages/
│   └── Devices.tsx             # Main devices page (simplified)
└── components/
    └── devices/
        ├── DeviceTableRow.tsx
        ├── DevicesTable.tsx
        ├── DevicesContent.tsx
        ├── DeviceInfoDialog.tsx
        └── DeviceSettingsDropdown.tsx
```

## DeviceContext API

### Context Type
```typescript
interface DeviceContextType {
  // Actions
  handleDelete: (id: string) => Promise<void>
  handleToggleActive: (id: string) => Promise<void>
  handleSyncUsers: (id: string) => Promise<void>
  handleSyncAttendance: (id: string) => Promise<void>
  handleClearAttendance: (id: string) => Promise<void>
  handleRestartDevice: (id: string) => Promise<void>
  handleLockDevice: (id: string) => Promise<void>
  handleUnlockDevice: (id: string) => Promise<void>
  
  // Loading States
  isDeleting: boolean
  isTogglingActive: boolean
  isSyncingUsers: boolean
  isSyncingAttendance: boolean
  isClearingAttendance: boolean
  isRestartingDevice: boolean
  isLockingDevice: boolean
  isUnlockingDevice: boolean
}
```

## Usage

### 1. Wrap Your App/Page with DeviceProvider

```tsx
import { DeviceProvider } from '@/contexts/DeviceContext'

export const Devices = () => {
  return (
    <DeviceProvider>
      <DevicesContent_Internal />
    </DeviceProvider>
  )
}
```

### 2. Use the Context in Components

```tsx
import { useDeviceContext } from '@/contexts/DeviceContext'

const MyComponent = () => {
  const {
    handleDelete,
    handleSyncUsers,
    isDeleting,
    isSyncingUsers,
  } = useDeviceContext()

  return (
    <div>
      <button 
        onClick={() => handleDelete(deviceId)}
        disabled={isDeleting}
      >
        Delete Device
      </button>
      
      <button 
        onClick={() => handleSyncUsers(deviceId)}
        disabled={isSyncingUsers}
      >
        Sync Users
      </button>
    </div>
  )
}
```

## Available Actions

### Device Management
- **handleDelete**: Delete a device (with confirmation)
- **handleToggleActive**: Toggle device active/inactive status

### Data Synchronization
- **handleSyncUsers**: Sync users from device to server
- **handleSyncAttendance**: Sync attendance records from device

### Device Control
- **handleRestartDevice**: Restart the device (with confirmation)
- **handleLockDevice**: Lock the device
- **handleUnlockDevice**: Unlock the device

### Data Management
- **handleClearAttendance**: Clear all attendance records (with confirmation)

## Device Commands

Each action sends a specific command to the device via the API:

| Action | Command | Priority | Description |
|--------|---------|----------|-------------|
| Sync Users | `DATA QUERY USERINFO` | 1 | Retrieve all users from device |
| Sync Attendance | `DATA QUERY ATTLOG` | 1 | Retrieve attendance logs |
| Clear Attendance | `DATA DELETE ATTLOG` | 1 | Delete all attendance records |
| Restart Device | `RESTART` | 10 | Restart the device |
| Lock Device | `CHECK Lock` | 5 | Lock the device |
| Unlock Device | `CHECK Unlock` | 5 | Unlock the device |

## Error Handling

All actions include built-in error handling:
- Try-catch blocks for all async operations
- Console error logging for debugging
- Toast notifications for user feedback (in service layer)
- Confirmation dialogs for destructive actions

## Loading States

Each action has a corresponding loading state:
```tsx
const { isSyncingUsers, handleSyncUsers } = useDeviceContext()

<Button 
  onClick={() => handleSyncUsers(deviceId)}
  disabled={isSyncingUsers}
>
  {isSyncingUsers ? 'Syncing...' : 'Sync Users'}
</Button>
```

## Migration Guide

### Before (Without Context)
```tsx
const Devices = () => {
  const syncUsers = useSyncUsers()
  const syncAttendance = useSyncAttendance()
  const clearAttendance = useClearAttendance()
  // ... many more hooks
  
  const handleSyncUsers = async (id: string) => {
    try {
      await syncUsers.mutateAsync(id)
    } catch (error) {
      console.error('Error syncing users:', error)
    }
  }
  // ... many more handlers
  
  return (
    <DevicesContent
      onSyncUsers={handleSyncUsers}
      onSyncAttendance={handleSyncAttendance}
      // ... many more props
    />
  )
}
```

### After (With Context)
```tsx
const DevicesContent_Internal = () => {
  const {
    handleSyncUsers,
    handleSyncAttendance,
    // ... all actions from context
  } = useDeviceContext()
  
  return (
    <DevicesContent
      onSyncUsers={handleSyncUsers}
      onSyncAttendance={handleSyncAttendance}
      // ... pass actions directly
    />
  )
}

export const Devices = () => (
  <DeviceProvider>
    <DevicesContent_Internal />
  </DeviceProvider>
)
```

## Best Practices

1. **Always wrap with DeviceProvider** at the page level
2. **Use loading states** to provide feedback
3. **Handle errors gracefully** with user-friendly messages
4. **Confirm destructive actions** (delete, clear, restart)
5. **Invalidate queries** after mutations for fresh data

## Testing

### Testing Components with Context

```tsx
import { render } from '@testing-library/react'
import { DeviceProvider } from '@/contexts/DeviceContext'

const renderWithContext = (component) => {
  return render(
    <DeviceProvider>
      {component}
    </DeviceProvider>
  )
}

test('handles device deletion', async () => {
  const { getByText } = renderWithContext(<DevicesList />)
  // ... test implementation
})
```

## Future Enhancements

- Add batch operations (delete multiple devices)
- Add device command history
- Add undo/redo functionality
- Add optimistic updates
- Add offline support with queue
- Add real-time status updates via WebSocket
