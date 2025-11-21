# Employee Dashboard API Documentation

## Overview
This document describes the Employee Dashboard API endpoints that provide personalized dashboard data for employees including shift information, attendance tracking, and statistics.

## Base URL
```
/api/dashboard
```

## Authentication
All endpoints require authentication with at least Employee role (`PolicyNames.AtLeastEmployee`).

## Endpoints

### 1. Get Complete Employee Dashboard
Get all dashboard data in a single request.

**Endpoint:** `GET /api/dashboard/employee`

**Authorization:** At least Employee role

**Query Parameters:**
- `period` (optional, string): Period for attendance stats. Valid values: `week`, `month`, `year`. Default: `week`

**Response:**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "todayShift": {
      "id": "guid",
      "startTime": "2024-01-15T09:00:00Z",
      "endTime": "2024-01-15T17:00:00Z",
      "description": "Morning shift",
      "status": 1,
      "totalHours": 8.0,
      "isToday": true
    },
    "nextShift": {
      "id": "guid",
      "startTime": "2024-01-16T09:00:00Z",
      "endTime": "2024-01-16T17:00:00Z",
      "description": "Morning shift",
      "status": 1,
      "totalHours": 8.0,
      "isToday": false
    },
    "currentAttendance": {
      "checkInTime": "2024-01-15T09:05:00Z",
      "checkOutTime": null,
      "status": "checked-in",
      "workHours": 4.5,
      "isLate": true,
      "lateMinutes": 5,
      "isEarlyOut": false,
      "earlyOutMinutes": null
    },
    "attendanceStats": {
      "period": "week",
      "totalWorkDays": 5,
      "presentDays": 4,
      "absentDays": 1,
      "lateCheckIns": 2,
      "earlyCheckOuts": 1,
      "attendanceRate": 80.0,
      "punctualityRate": 50.0,
      "avgWorkHours": "7.5"
    }
  }
}
```

### 2. Get Today's Shift
Get shift information for the current day.

**Endpoint:** `GET /api/dashboard/shifts/today`

**Authorization:** At least Employee role

**Response:**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "id": "guid",
    "startTime": "2024-01-15T09:00:00Z",
    "endTime": "2024-01-15T17:00:00Z",
    "description": "Morning shift",
    "status": 1,
    "totalHours": 8.0,
    "isToday": true
  }
}
```

**Response (No shift today):**
```json
{
  "isSuccess": true,
  "message": "",
  "data": null
}
```

### 3. Get Next Shift
Get the next upcoming shift.

**Endpoint:** `GET /api/dashboard/shifts/next`

**Authorization:** At least Employee role

**Response:**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "id": "guid",
    "startTime": "2024-01-16T09:00:00Z",
    "endTime": "2024-01-16T17:00:00Z",
    "description": "Morning shift",
    "status": 1,
    "totalHours": 8.0,
    "isToday": false
  }
}
```

### 4. Get Current Attendance
Get attendance status for the current day.

**Endpoint:** `GET /api/dashboard/attendance/current`

**Authorization:** At least Employee role

**Response (Checked in):**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "checkInTime": "2024-01-15T09:05:00Z",
    "checkOutTime": null,
    "status": "checked-in",
    "workHours": 4.5,
    "isLate": true,
    "lateMinutes": 5,
    "isEarlyOut": false,
    "earlyOutMinutes": null
  }
}
```

**Response (Checked out):**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "checkInTime": "2024-01-15T09:05:00Z",
    "checkOutTime": "2024-01-15T17:02:00Z",
    "status": "checked-out",
    "workHours": 7.95,
    "isLate": true,
    "lateMinutes": 5,
    "isEarlyOut": false,
    "earlyOutMinutes": null
  }
}
```

**Response (No attendance today):**
```json
{
  "isSuccess": true,
  "message": "",
  "data": null
}
```

### 5. Get Attendance Statistics
Get attendance statistics for a specific period.

**Endpoint:** `GET /api/dashboard/attendance/stats`

**Authorization:** At least Employee role

**Query Parameters:**
- `period` (optional, string): Period for stats. Valid values: `week`, `month`, `year`. Default: `week`

**Response:**
```json
{
  "isSuccess": true,
  "message": "",
  "data": {
    "period": "week",
    "totalWorkDays": 5,
    "presentDays": 4,
    "absentDays": 1,
    "lateCheckIns": 2,
    "earlyCheckOuts": 1,
    "attendanceRate": 80.0,
    "punctualityRate": 50.0,
    "avgWorkHours": "7.5"
  }
}
```

## Data Models

### EmployeeDashboardDto
```csharp
public class EmployeeDashboardDto
{
    public ShiftInfoDto? TodayShift { get; set; }
    public ShiftInfoDto? NextShift { get; set; }
    public AttendanceInfoDto? CurrentAttendance { get; set; }
    public AttendanceStatsDto? AttendanceStats { get; set; }
}
```

### ShiftInfoDto
```csharp
public class ShiftInfoDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public double TotalHours { get; set; }
    public bool IsToday { get; set; }
}
```

### AttendanceInfoDto
```csharp
public class AttendanceInfoDto
{
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string Status { get; set; } = string.Empty; // "not-started", "checked-in", "checked-out"
    public double WorkHours { get; set; }
    public bool IsLate { get; set; }
    public int? LateMinutes { get; set; }
    public bool IsEarlyOut { get; set; }
    public int? EarlyOutMinutes { get; set; }
}
```

### AttendanceStatsDto
```csharp
public class AttendanceStatsDto
{
    public string Period { get; set; } = string.Empty; // "week", "month", "year"
    public int TotalWorkDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateCheckIns { get; set; }
    public int EarlyCheckOuts { get; set; }
    public double AttendanceRate { get; set; }
    public double PunctualityRate { get; set; }
    public string AvgWorkHours { get; set; } = string.Empty;
}
```

## Shift Status Values
- `0` - Pending
- `1` - Approved
- `2` - Rejected

## Attendance Status Values
- `not-started` - Employee hasn't checked in yet
- `checked-in` - Employee has checked in but not checked out
- `checked-out` - Employee has completed the day (both check-in and check-out)

## Period Values
- `week` - Last 7 days
- `month` - Last 30 days
- `year` - Last 365 days

## Error Responses

### 401 Unauthorized
```json
{
  "isSuccess": false,
  "message": "Unauthorized",
  "data": null
}
```

### 500 Internal Server Error
```json
{
  "isSuccess": false,
  "message": "An error occurred while retrieving employee dashboard",
  "data": null
}
```

## Implementation Details

### CQRS Pattern
All endpoints use the CQRS pattern with MediatR:
- Each endpoint corresponds to a Query with a Handler
- Queries are immutable and return data without side effects
- Handlers contain the business logic for data retrieval

### Query Handlers
1. `GetEmployeeDashboardHandler` - Aggregates all dashboard data
2. `GetTodayShiftHandler` - Retrieves today's approved shift
3. `GetNextShiftHandler` - Retrieves next upcoming approved shift
4. `GetCurrentAttendanceHandler` - Calculates current day attendance status
5. `GetAttendanceStatsHandler` - Computes attendance statistics for a period

### Business Logic

#### Late Check-in Detection
An employee is considered late if their check-in time is after the shift start time. The late minutes are calculated as the difference between check-in time and expected start time.

#### Early Check-out Detection
An employee is considered to have left early if their check-out time is before the shift end time. The early-out minutes are calculated as the difference between expected end time and actual check-out time.

#### Attendance Rate Calculation
```
Attendance Rate = (Present Days / Total Work Days) * 100
```

#### Punctuality Rate Calculation
```
Punctuality Rate = ((Present Days - Late Check-ins) / Present Days) * 100
```

#### Average Work Hours Calculation
```
Average Work Hours = Total Work Hours / Present Days
```

## Frontend Integration

### React Query Hook Example
```typescript
export const useEmployeeDashboard = (period: 'week' | 'month' | 'year' = 'week') => {
  return useQuery({
    queryKey: ['employee-dashboard', period],
    queryFn: () => employeeDashboardService.getEmployeeDashboard(period),
    refetchInterval: 60000, // Refresh every minute
  });
};
```

### Service Example
```typescript
const getEmployeeDashboard = async (period: string = 'week') => {
  const response = await api.get<ApiResponse<EmployeeDashboardData>>(
    `/api/dashboard/employee?period=${period}`
  );
  return response.data.data;
};
```

## Security Considerations

1. **Authorization**: All endpoints require authentication and at least Employee role
2. **User Context**: All queries filter data by the authenticated user's ID
3. **Data Isolation**: Employees can only see their own data
4. **Input Validation**: Period parameter is validated to prevent invalid inputs

## Performance Considerations

1. **Caching**: Frontend implements React Query caching with 5-minute stale time
2. **Aggregation**: Main dashboard endpoint aggregates multiple queries server-side
3. **Filtering**: All database queries filter by user ID and date ranges
4. **Indexing**: Ensure database indexes on `ApplicationUserId`, `StartTime`, `AttendanceTime`, and `Status` columns

## Future Enhancements

1. Add pagination for attendance history
2. Implement real-time attendance updates using SignalR
3. Add attendance trends and predictions
4. Support custom date ranges for statistics
5. Add export functionality for attendance reports
