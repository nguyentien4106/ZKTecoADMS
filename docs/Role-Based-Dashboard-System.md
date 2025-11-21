# Role-Based Dashboard System

## Overview

The dashboard system now displays different content based on the user's role:
- **Admin**: Full system overview with quick management actions
- **Manager**: Team performance and attendance metrics
- **Employee**: Personal dashboard with shifts and attendance

## Architecture

### Main Dashboard Router
**File**: `src/pages/Dashboard.tsx`

The main dashboard acts as a router that detects the user's role and renders the appropriate dashboard component.

```tsx
import { Dashboard } from '@/pages/Dashboard';

// Automatically shows the right dashboard based on user role
<Route path="/dashboard" element={<Dashboard />} />
```

### Dashboard Components

#### 1. AdminDashboard
**File**: `src/pages/AdminDashboard.tsx`

**Features:**
- Full system overview
- Quick action cards for:
  - Manage Users
  - Manage Devices  
  - System Settings
- Summary cards (employees, devices, attendance)
- Attendance trends chart
- Top performers list
- Late employees list
- Department statistics
- Device status monitoring

**Tabs:**
- Overview
- Performance
- Departments
- Devices

#### 2. ManagerDashboard
**File**: `src/pages/ManagerDashboard.tsx`

**Features:**
- Team overview and metrics
- Summary cards
- Attendance trends
- Top performers
- Late employees
- Department statistics
- Device status

**Tabs:**
- Overview
- Performance
- Departments
- Devices

#### 3. EmployeeDashboard
**File**: `src/components/employee-dashboard/EmployeeDashboard.tsx`

**Features:**
- Today's shift schedule
- Next upcoming shift
- Current attendance status
- Late check-in tracking
- Early check-out tracking
- Attendance statistics (week/month/year)
- Quick action buttons

## Role Detection

The system uses the `useRoleAccess` hook to determine the user's role:

```typescript
import { useRoleAccess } from '@/hooks/useRoleAccess';

const { getUserRole, isAdmin, isManager, isEmployee } = useRoleAccess();
const userRole = getUserRole(); // Returns: UserRole.ADMIN | MANAGER | EMPLOYEE
```

## User Roles Hierarchy

```typescript
export enum UserRole {
  ADMIN = 'Admin',      // Highest level - full access
  MANAGER = 'Manager',  // Team management access
  EMPLOYEE = 'Employee' // Personal data access only
}

export const ROLE_HIERARCHY = {
  [UserRole.ADMIN]: 3,
  [UserRole.MANAGER]: 2,
  [UserRole.EMPLOYEE]: 1
}
```

## Dashboard Routing Flow

```
User Logs In
     ↓
Dashboard Route (/dashboard)
     ↓
Dashboard.tsx (Role Router)
     ↓
Detect User Role
     ↓
     ├─ ADMIN → AdminDashboard
     ├─ MANAGER → ManagerDashboard
     └─ EMPLOYEE → EmployeeDashboard
```

## Feature Comparison

| Feature | Admin | Manager | Employee |
|---------|-------|---------|----------|
| System Overview | ✅ | ✅ | ❌ |
| Quick Admin Actions | ✅ | ❌ | ❌ |
| Team Performance | ✅ | ✅ | ❌ |
| Department Stats | ✅ | ✅ | ❌ |
| Device Management | ✅ | ✅ | ❌ |
| Top Performers | ✅ | ✅ | ❌ |
| Late Employees | ✅ | ✅ | ❌ |
| Personal Shifts | ❌ | ❌ | ✅ |
| Personal Attendance | ❌ | ❌ | ✅ |
| Late/Early Tracking | ❌ | ❌ | ✅ |
| Period Selection | ❌ | ❌ | ✅ |

## Data Fetching

### Admin & Manager Dashboards
```typescript
// Shared hooks for management dashboards
const { data: summary } = useDashboardSummary()
const { data: trends } = useAttendanceTrends(30)
const { data: topPerformers } = useTopPerformers({ count: 10 })
const { data: lateEmployees } = useLateEmployees({ count: 10 })
const { data: departments } = useDepartmentStats()
const { data: devices } = useDeviceStatus()
```

### Employee Dashboard
```typescript
// Personal dashboard data
const { data, isLoading, refetch } = useEmployeeDashboard({ 
  period: 'week' | 'month' | 'year' 
})
```

## Customization

### Adding Features to Specific Dashboards

#### Admin Dashboard
Edit `src/pages/AdminDashboard.tsx` to add admin-only features:
```tsx
// Add new quick action card
<Card onClick={() => navigate('/your-route')}>
  <CardHeader>
    <CardTitle>New Feature</CardTitle>
    <YourIcon className="h-4 w-4" />
  </CardHeader>
  <CardContent>
    <p>Feature description</p>
  </CardContent>
</Card>
```

#### Manager Dashboard
Edit `src/pages/ManagerDashboard.tsx` to add manager-specific views.

#### Employee Dashboard
Edit `src/components/employee-dashboard/EmployeeDashboard.tsx` to add employee features.

### Modifying Dashboard Selection Logic

Edit `src/pages/Dashboard.tsx`:
```tsx
switch (userRole) {
  case UserRole.ADMIN:
    return <AdminDashboard />
    
  case UserRole.MANAGER:
    return <ManagerDashboard />
    
  case UserRole.EMPLOYEE:
    return <EmployeeDashboard {...props} />
    
  // Add custom roles
  case UserRole.CUSTOM_ROLE:
    return <CustomDashboard />
}
```

## Loading States

All dashboards include loading states:
```tsx
if (isLoading) {
  return <LoadingSpinner />
}
```

## Error Handling

Unknown or unauthorized roles show an access denied message:
```tsx
default:
  return (
    <div className="flex items-center justify-center h-screen">
      <div className="text-center">
        <h2>Access Denied</h2>
        <p>You don't have permission to view this dashboard.</p>
      </div>
    </div>
  )
```

## Testing Different Roles

### Development Testing
1. Login as different users with different roles
2. Navigate to `/dashboard`
3. Verify the correct dashboard appears

### Manual Role Testing
```typescript
// Temporarily override role for testing
const testRole = UserRole.EMPLOYEE; // Change this
const userRole = testRole; // Use instead of getUserRole()
```

## Backend Requirements

### Admin/Manager Dashboards
```
GET /api/dashboard/summary
GET /api/dashboard/attendance-trends?days=30
GET /api/dashboard/top-performers?count=10
GET /api/dashboard/late-employees?count=10
GET /api/dashboard/department-stats
GET /api/dashboard/device-status
```

### Employee Dashboard
```
GET /api/employee/dashboard?period={week|month|year}
GET /api/shifts/today
GET /api/shifts/next
GET /api/attendances/current
GET /api/attendances/stats?period={week|month|year}
```

## Security Considerations

1. **Role Validation**: Always validate user roles on the backend
2. **API Authorization**: Protect API endpoints with role-based access
3. **Client-Side Protection**: UI hiding only - never rely solely on frontend checks
4. **Token Verification**: Verify JWT tokens contain valid role claims

## Performance Optimization

- **Lazy Loading**: Dashboards only load when accessed
- **Conditional Fetching**: Employee data only fetches for employee role
- **Auto-Refresh**: Manager/Admin dashboards can auto-refresh metrics
- **Caching**: React Query caches dashboard data

## Future Enhancements

### Potential Features
- [ ] Custom dashboard layouts per role
- [ ] Dashboard widgets drag-and-drop
- [ ] Exportable reports
- [ ] Real-time notifications
- [ ] Dashboard personalization
- [ ] Mobile-optimized views
- [ ] Dark mode support
- [ ] Multi-language support

### Additional Roles
- Supervisor (between Manager and Employee)
- Department Head
- HR Administrator
- Auditor (read-only access)

## Troubleshooting

### Dashboard Not Showing
1. Check user is logged in
2. Verify role in JWT token
3. Check console for errors
4. Verify API endpoints are accessible

### Wrong Dashboard Appears
1. Clear browser cache
2. Re-login to refresh token
3. Check role in user object
4. Verify role mapping in `getUserRole()`

### Data Not Loading
1. Check network tab for API calls
2. Verify backend endpoints
3. Check authentication token
4. Review React Query DevTools

## Migration Guide

If you have existing dashboard code:

1. **Backup**: Save your current `Dashboard.tsx`
2. **Move Logic**: Extract admin/manager code to new files
3. **Update Imports**: Import new dashboard components
4. **Test Roles**: Test with all user roles
5. **Deploy**: Deploy all three dashboard files together

## Summary

✅ **3 Role-Based Dashboards**
- Admin Dashboard with management quick actions
- Manager Dashboard with team metrics
- Employee Dashboard with personal information

✅ **Automatic Role Detection**
- Uses `useRoleAccess` hook
- Renders appropriate dashboard
- Handles unauthorized access

✅ **All Features Working**
- No TypeScript errors
- Loading states
- Error handling
- Responsive design

The system is production-ready and provides a personalized experience for each user role!
