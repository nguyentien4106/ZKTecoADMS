# Dashboard Controller - Manager Performance Monitoring

## Overview

The Dashboard Controller provides comprehensive analytics and insights for managers to monitor employee performance, attendance patterns, and device status. This API offers real-time and historical data to help make informed decisions about workforce management.

**Security**: All dashboard data is automatically filtered to show only information from devices belonging to the authenticated user. This ensures data isolation and security in multi-tenant environments.

## Features

### 1. **Comprehensive Dashboard Data**
- Complete overview of all metrics in a single endpoint
- Customizable date ranges and filters
- Department-specific insights
- **Automatic filtering by user's devices**

### 2. **Summary Statistics**
- Total and active employees count (from user's devices only)
- Device status (online/offline) for user's devices
- Today's check-ins, check-outs, absences, and late arrivals
- Average attendance rate across the organization

### 3. **Employee Performance Tracking**
- **Top Performers**: Identify employees with the best attendance and punctuality records
- **Late Employees**: Track employees with frequent late arrivals
- Performance metrics include:
  - Attendance rate
  - Punctuality rate
  - Average work hours
  - Average late time
  - On-time vs. late days
  - Absent days

### 4. **Department Analytics**
- Compare performance across departments
- Track department-wide attendance and punctuality
- Monitor active vs. absent employees per department

### 5. **Attendance Trends**
- Historical trend analysis (up to 90 days)
- Daily check-in/check-out patterns
- Late arrival trends
- Absence patterns
- Visual data for trend charts

### 6. **Device Status Monitoring**
- Real-time device status (online/offline)
- Device location tracking
- Number of registered users per device
- Today's attendance count per device
- Last online timestamp

## API Endpoints

### Get Complete Dashboard Data
```http
GET /api/dashboard?startDate=2024-01-01&endDate=2024-01-31&department=Engineering&topPerformersCount=10&lateEmployeesCount=10&trendDays=30
```

**Query Parameters:**
- `startDate` (optional): Start date for analysis (defaults to 30 days ago)
- `endDate` (optional): End date for analysis (defaults to today)
- `department` (optional): Filter by specific department
- `topPerformersCount` (optional): Number of top performers to return (1-100, default: 10)
- `lateEmployeesCount` (optional): Number of late employees to return (1-100, default: 10)
- `trendDays` (optional): Number of days for trend analysis (1-90, default: 30)

**Response:**
```json
{
  "isSuccess": true,
  "errors": [],
  "data": {
    "summary": {
      "totalEmployees": 150,
      "activeEmployees": 145,
      "inactiveEmployees": 5,
      "totalDevices": 10,
      "onlineDevices": 9,
      "offlineDevices": 1,
      "todayCheckIns": 140,
      "todayCheckOuts": 135,
      "todayAbsences": 5,
      "todayLateArrivals": 12,
      "averageAttendanceRate": 96.67
    },
    "topPerformers": [...],
    "lateEmployees": [...],
    "departmentStats": [...],
    "attendanceTrends": [...],
    "deviceStatuses": [...]
  }
}
```

### Get Today's Summary
```http
GET /api/dashboard/summary
```

Returns key metrics for today only.

### Get Top Performers
```http
GET /api/dashboard/top-performers?startDate=2024-01-01&endDate=2024-01-31&count=10&department=Engineering
```

Returns employees with the best attendance and punctuality records.

**Employee Performance Metrics:**
```json
{
  "userId": "guid",
  "fullName": "John Doe",
  "department": "Engineering",
  "totalAttendanceDays": 20,
  "onTimeDays": 18,
  "lateDays": 2,
  "absentDays": 3,
  "attendanceRate": 87.0,
  "punctualityRate": 90.0,
  "averageWorkHours": "08:15:00",
  "averageLateTime": "00:12:00",
  "lastCheckIn": "2024-01-31T09:05:00",
  "lastCheckOut": "2024-01-31T17:30:00"
}
```

### Get Late Employees
```http
GET /api/dashboard/late-employees?startDate=2024-01-01&endDate=2024-01-31&count=10&department=Engineering
```

Returns employees with frequent late arrivals.

### Get Department Statistics
```http
GET /api/dashboard/department-stats?startDate=2024-01-01&endDate=2024-01-31
```

Returns performance statistics grouped by department.

**Department Statistics:**
```json
{
  "department": "Engineering",
  "totalEmployees": 50,
  "activeToday": 48,
  "absentToday": 2,
  "lateToday": 5,
  "attendanceRate": 96.0,
  "punctualityRate": 89.6,
  "averageWorkHours": "08:20:00"
}
```

### Get Attendance Trends
```http
GET /api/dashboard/attendance-trends?days=30
```

Returns daily attendance trends for the specified number of days.

**Trend Data:**
```json
{
  "date": "2024-01-15",
  "totalCheckIns": 145,
  "totalCheckOuts": 142,
  "lateArrivals": 8,
  "absences": 5,
  "attendanceRate": 96.67
}
```

### Get Device Status
```http
GET /api/dashboard/device-status
```

Returns current status of all attendance devices.

**Device Status:**
```json
{
  "deviceId": "guid",
  "deviceName": "Main Entrance",
  "location": "Building A - Floor 1",
  "status": "Online",
  "lastOnline": "2024-01-31T14:30:00",
  "registeredUsers": 50,
  "todayAttendances": 98
}
```

## Key Performance Indicators

### Attendance Rate
Percentage of days an employee checked in during the period:
```
Attendance Rate = (Days Present / Total Working Days) × 100
```

### Punctuality Rate
Percentage of on-time check-ins:
```
Punctuality Rate = (On-time Check-ins / Total Check-ins) × 100
```

### Late Arrival Definition
An employee is considered late if they check in after **9:15 AM** (configurable in the handler).

### Average Work Hours
Calculated by matching check-in and check-out times for each day and averaging the difference.

## Use Cases

### 1. Daily Morning Review
```http
GET /api/dashboard/summary
```
Get a quick overview of today's attendance status during morning meetings.

### 2. Monthly Performance Review
```http
GET /api/dashboard?startDate=2024-01-01&endDate=2024-01-31&topPerformersCount=20
```
Identify top performers for recognition and rewards.

### 3. Addressing Tardiness Issues
```http
GET /api/dashboard/late-employees?startDate=2024-01-01&endDate=2024-01-31&count=20
```
Identify employees who need attendance counseling.

### 4. Department Comparison
```http
GET /api/dashboard/department-stats?startDate=2024-01-01&endDate=2024-01-31
```
Compare performance across departments to identify best practices or areas needing improvement.

### 5. Trend Analysis
```http
GET /api/dashboard/attendance-trends?days=90
```
Identify patterns in attendance over time (seasonal variations, day-of-week patterns, etc.).

### 6. Device Maintenance
```http
GET /api/dashboard/device-status
```
Quickly identify offline devices that need attention.

## Authorization

**All endpoints require authentication.** Include a valid JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

**Data Isolation**: The dashboard automatically filters all data based on the authenticated user's devices. Each user can only see:
- Employees registered to their devices
- Attendance records from their devices
- Statistics for their devices only
- Performance metrics for their employees only

This ensures that in a multi-tenant environment, users cannot access data from devices they don't own.

## Error Handling

All endpoints return standard error responses:
```json
{
  "isSuccess": false,
  "errors": ["Error message here"],
  "data": null
}
```

Common HTTP status codes:
- `200 OK`: Successful request
- `400 Bad Request`: Invalid parameters
- `401 Unauthorized`: Missing or invalid authentication
- `500 Internal Server Error`: Server-side error

## Best Practices

1. **Use Date Ranges Wisely**: Don't query extremely long date ranges as they may impact performance
2. **Cache Results**: Consider caching dashboard data on the client side if real-time updates aren't critical
3. **Filter by Department**: When analyzing large organizations, filter by department to improve performance
4. **Schedule Reports**: Consider running trend analysis during off-peak hours
5. **Monitor Device Status**: Set up alerts when devices go offline

## Configuration

The following parameters can be adjusted in the handler:
- `StandardWorkStartHour`: Default work start time (currently 9 AM)
- `LateThresholdMinutes`: Minutes after start time before marking as late (currently 15 minutes)

## Future Enhancements

Potential improvements for future versions:
- Real-time notifications for absences
- Predictive analytics for attendance patterns
- Export functionality (PDF/Excel reports)
- Customizable work schedules per employee
- Integration with HR systems
- Absence request tracking
- Overtime calculations
- Leave balance integration
