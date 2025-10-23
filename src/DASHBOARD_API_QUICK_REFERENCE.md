# Dashboard API Quick Reference

## Base URL
```
/api/dashboard
```

## Authentication
All endpoints require JWT Bearer token:
```
Authorization: Bearer <your-jwt-token>
```

---

## 📊 Complete Dashboard
Get all metrics in one call

**Endpoint:** `GET /api/dashboard`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| startDate | DateTime? | 30 days ago | Analysis start date |
| endDate | DateTime? | Today | Analysis end date |
| department | string? | null | Filter by department |
| topPerformersCount | int | 10 | Number of top performers (1-100) |
| lateEmployeesCount | int | 10 | Number of late employees (1-100) |
| trendDays | int | 30 | Days for trend analysis (1-90) |

**Example:**
```http
GET /api/dashboard?startDate=2024-10-01&endDate=2024-10-23&department=Engineering&topPerformersCount=15
```

---

## 📈 Today's Summary
Quick overview of today's metrics

**Endpoint:** `GET /api/dashboard/summary`

**Example:**
```http
GET /api/dashboard/summary
```

**Response:**
- Total/active employees
- Device status
- Today's check-ins/check-outs
- Absences and late arrivals
- Attendance rate

---

## 🏆 Top Performers
Best performing employees

**Endpoint:** `GET /api/dashboard/top-performers`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| startDate | DateTime? | 30 days ago | Analysis start date |
| endDate | DateTime? | Today | Analysis end date |
| count | int | 10 | Number of results |
| department | string? | null | Filter by department |

**Example:**
```http
GET /api/dashboard/top-performers?count=20&startDate=2024-10-01
```

---

## ⏰ Late Employees
Employees with tardiness issues

**Endpoint:** `GET /api/dashboard/late-employees`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| startDate | DateTime? | 30 days ago | Analysis start date |
| endDate | DateTime? | Today | Analysis end date |
| count | int | 10 | Number of results |
| department | string? | null | Filter by department |

**Example:**
```http
GET /api/dashboard/late-employees?count=20&department=Sales
```

---

## 🏢 Department Statistics
Compare performance across departments

**Endpoint:** `GET /api/dashboard/department-stats`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| startDate | DateTime? | 30 days ago | Analysis start date |
| endDate | DateTime? | Today | Analysis end date |

**Example:**
```http
GET /api/dashboard/department-stats?startDate=2024-10-01&endDate=2024-10-31
```

---

## 📉 Attendance Trends
Historical attendance patterns

**Endpoint:** `GET /api/dashboard/attendance-trends`

**Query Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| days | int | 30 | Number of days (1-90) |

**Example:**
```http
GET /api/dashboard/attendance-trends?days=60
```

---

## 🖥️ Device Status
Monitor device health

**Endpoint:** `GET /api/dashboard/device-status`

**Example:**
```http
GET /api/dashboard/device-status
```

---

## Response Format

All endpoints return:
```json
{
  "isSuccess": true,
  "errors": [],
  "message": "",
  "data": { ... }
}
```

**Success Response (200):**
```json
{
  "isSuccess": true,
  "errors": [],
  "data": {
    // Endpoint-specific data
  }
}
```

**Error Response (400/500):**
```json
{
  "isSuccess": false,
  "errors": ["Error message"],
  "message": "Error message",
  "data": null
}
```

---

## Common Use Cases

### Morning Standup
```http
GET /api/dashboard/summary
```

### Weekly Review
```http
GET /api/dashboard?startDate=2024-10-16&endDate=2024-10-23
```

### Monthly Report
```http
GET /api/dashboard?startDate=2024-10-01&endDate=2024-10-31&topPerformersCount=20
```

### Identify Issues
```http
GET /api/dashboard/late-employees?count=30&startDate=2024-10-01
```

### Department Comparison
```http
GET /api/dashboard/department-stats?startDate=2024-10-01&endDate=2024-10-31
```

### Trend Analysis
```http
GET /api/dashboard/attendance-trends?days=90
```

---

## Key Metrics Explained

### Attendance Rate
```
(Days Present / Total Working Days) × 100
```

### Punctuality Rate
```
(On-time Check-ins / Total Check-ins) × 100
```

### Late Definition
Check-in after **9:15 AM**

### Work Hours
Time between first check-in and last check-out of the day

---

## HTTP Status Codes

| Code | Description |
|------|-------------|
| 200 | Success |
| 400 | Bad Request (invalid parameters) |
| 401 | Unauthorized (missing/invalid token) |
| 500 | Internal Server Error |

---

## Rate Limiting
(To be implemented)
- Recommended: 100 requests per minute per user
- Heavy endpoints may have lower limits

---

## Data Scope
🔒 **Important:** All data is automatically filtered to show only:
- Devices owned by the authenticated user
- Employees registered to those devices
- Attendance from those devices

You cannot access data from other users' devices.
