# Employee Self Service (ESS) - API Documentation

## Table of Contents
1. [API Overview](#api-overview)
2. [Authentication & Authorization](#authentication--authorization)
3. [Common Patterns](#common-patterns)
4. [Employee Profile Management](#employee-profile-management)
5. [Leave Management APIs](#leave-management-apis)
6. [Overtime Management APIs](#overtime-management-apis)
7. [Shift Management APIs](#shift-management-apis)
8. [Approval Workflow APIs](#approval-workflow-apis)
9. [Notification APIs](#notification-apis)
10. [Reporting APIs](#reporting-apis)
11. [Administrative APIs](#administrative-apis)
12. [Error Handling](#error-handling)
13. [Rate Limiting](#rate-limiting)
14. [API Versioning](#api-versioning)

## API Overview

### Base URL
```
Production: https://api.zktecoADMS.com/api/v1
Development: https://dev-api.zktecoADMS.com/api/v1
Local: https://localhost:7001/api/v1
```

### Common Headers
```http
Content-Type: application/json
Authorization: Bearer {jwt_token}
X-API-Version: 1.0
X-Client-ID: {client_identifier}
```

### Response Format
All APIs follow a consistent response format:

```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {},
  "errors": [],
  "timestamp": "2024-11-05T10:30:00Z",
  "correlationId": "uuid-correlation-id"
}
```

## Authentication & Authorization

### JWT Token Structure
```json
{
  "sub": "user-id",
  "email": "user@company.com",
  "role": ["Employee", "Manager"],
  "permissions": ["leave:read", "leave:create", "approval:manage"],
  "employeeId": "EMP001",
  "departmentId": "DEPT001",
  "managerId": "MGR001",
  "exp": 1699200000,
  "iat": 1699113600
}
```

### Permission System
- **leave:read** - View own leave requests and balances
- **leave:create** - Submit leave requests
- **leave:manage** - Manage team leave requests (managers)
- **overtime:read** - View own overtime requests
- **overtime:create** - Submit overtime requests
- **overtime:approve** - Approve overtime requests
- **shift:read** - View own shift requests
- **shift:create** - Submit shift requests
- **shift:swap** - Participate in shift swapping
- **approval:manage** - Manage approval workflows
- **report:view** - Access reporting features
- **admin:manage** - Administrative functions

## Common Patterns

### Pagination Request
```json
{
  "pageNumber": 1,
  "pageSize": 20,
  "sortBy": "createdAt",
  "sortDirection": "desc",
  "filters": {}
}
```

### Pagination Response
```json
{
  "success": true,
  "data": {
    "items": [],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 20,
    "totalPages": 5,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

### Filter Objects
```json
{
  "dateFrom": "2024-01-01",
  "dateTo": "2024-12-31",
  "status": ["Approved", "Pending"],
  "department": "IT",
  "employeeId": "EMP001"
}
```

## Employee Profile Management

### Get Current Employee Profile
```http
GET /api/v1/employees/profile
Authorization: Bearer {jwt_token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "employeeId": "EMP001",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john.doe@company.com",
    "department": "Information Technology",
    "position": "Senior Developer",
    "managerId": "uuid",
    "managerName": "Jane Smith",
    "joinDate": "2023-01-15",
    "status": "Active",
    "workLocation": "New York Office",
    "phoneNumber": "+1-555-0123",
    "leaveBalances": [
      {
        "leaveTypeId": "uuid",
        "leaveTypeName": "Annual Leave",
        "year": 2024,
        "entitledDays": 25,
        "usedDays": 10,
        "pendingDays": 3,
        "availableDays": 12,
        "carriedOverDays": 0
      }
    ]
  }
}
```

### Update Employee Profile
```http
PUT /api/v1/employees/profile
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "phoneNumber": "+1-555-0124",
  "workLocation": "Remote",
  "emergencyContactName": "Jane Doe",
  "emergencyContactPhone": "+1-555-0125"
}
```

### Get Team Members (Managers Only)
```http
GET /api/v1/employees/team
Authorization: Bearer {jwt_token}
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "uuid",
      "employeeId": "EMP002",
      "firstName": "Alice",
      "lastName": "Johnson",
      "position": "Developer",
      "status": "Active",
      "currentLeaveStatus": {
        "isOnLeave": false,
        "leaveEndDate": null,
        "leaveType": null
      }
    }
  ]
}
```

## Leave Management APIs

### Get Leave Types
```http
GET /api/v1/leave/types
Authorization: Bearer {jwt_token}
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "uuid",
      "name": "Annual Leave",
      "code": "AL",
      "description": "Yearly vacation leave",
      "maxDaysPerYear": 25,
      "requiresApproval": true,
      "requiresDocumentation": false,
      "minimumNoticeHours": 24,
      "isActive": true,
      "carryOverLimit": 5
    }
  ]
}
```

### Get Leave Balances
```http
GET /api/v1/leave/balances
Authorization: Bearer {jwt_token}
Query Parameters:
  - year (optional): 2024
  - employeeId (optional, managers only): uuid
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "leaveTypeId": "uuid",
      "leaveTypeName": "Annual Leave",
      "year": 2024,
      "entitledDays": 25,
      "usedDays": 10,
      "pendingDays": 3,
      "carriedOverDays": 2,
      "availableDays": 14,
      "balanceHistory": [
        {
          "date": "2024-01-01",
          "action": "Initial Allocation",
          "days": 25,
          "balance": 25
        }
      ]
    }
  ]
}
```

### Submit Leave Request
```http
POST /api/v1/leave/requests
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "leaveTypeId": "uuid",
  "startDate": "2024-12-20",
  "endDate": "2024-12-24",
  "reason": "Christmas vacation with family",
  "isEmergency": false,
  "comments": "Will complete all pending tasks before leave",
  "documents": [
    {
      "fileName": "medical_certificate.pdf",
      "fileSize": 1024000,
      "contentType": "application/pdf",
      "base64Content": "base64_encoded_content"
    }
  ]
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "requestNumber": "LR-2024-001",
    "status": "Submitted",
    "totalDays": 5,
    "submittedAt": "2024-11-05T10:30:00Z",
    "nextApprover": {
      "name": "Jane Smith",
      "position": "Manager"
    }
  }
}
```

### Get Leave Requests
```http
GET /api/v1/leave/requests
Authorization: Bearer {jwt_token}
Query Parameters:
  - pageNumber: 1
  - pageSize: 20
  - status: Pending,Approved,Rejected
  - dateFrom: 2024-01-01
  - dateTo: 2024-12-31
  - employeeId (managers only): uuid
```

### Get Leave Request Details
```http
GET /api/v1/leave/requests/{requestId}
Authorization: Bearer {jwt_token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "requestNumber": "LR-2024-001",
    "employeeName": "John Doe",
    "leaveType": "Annual Leave",
    "startDate": "2024-12-20",
    "endDate": "2024-12-24",
    "totalDays": 5,
    "reason": "Christmas vacation with family",
    "status": "Under Review",
    "isEmergency": false,
    "submittedAt": "2024-11-05T10:30:00Z",
    "approvalHistory": [
      {
        "approverName": "Jane Smith",
        "approverPosition": "Manager",
        "level": 1,
        "status": "Approved",
        "comments": "Approved - good timing",
        "actionDate": "2024-11-05T14:30:00Z"
      }
    ],
    "documents": [
      {
        "id": "uuid",
        "fileName": "medical_certificate.pdf",
        "uploadedAt": "2024-11-05T10:30:00Z",
        "downloadUrl": "/api/v1/documents/{documentId}"
      }
    ],
    "canModify": false,
    "canCancel": true
  }
}
```

### Update Leave Request
```http
PUT /api/v1/leave/requests/{requestId}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "startDate": "2024-12-21",
  "endDate": "2024-12-25",
  "reason": "Updated vacation dates",
  "comments": "Changed dates due to project requirements"
}
```

### Cancel Leave Request
```http
DELETE /api/v1/leave/requests/{requestId}
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "reason": "Plans changed - no longer needed"
}
```

### Get Leave Calendar
```http
GET /api/v1/leave/calendar
Authorization: Bearer {jwt_token}
Query Parameters:
  - month: 2024-12
  - view: personal|team|department
```

**Response:**
```json
{
  "success": true,
  "data": {
    "month": "2024-12",
    "events": [
      {
        "employeeId": "uuid",
        "employeeName": "John Doe",
        "leaveType": "Annual Leave",
        "startDate": "2024-12-20",
        "endDate": "2024-12-24",
        "status": "Approved",
        "isPartialDay": false
      }
    ]
  }
}
```

## Overtime Management APIs

### Submit Overtime Request
```http
POST /api/v1/overtime/requests
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "startTime": "2024-11-05T18:00:00Z",
  "endTime": "2024-11-05T22:00:00Z",
  "justification": "Critical bug fix for production deployment",
  "overtimeType": "Regular",
  "isPreApproved": false,
  "projectCode": "PROJ-001",
  "estimatedHours": 4
}
```

### Get Overtime Requests
```http
GET /api/v1/overtime/requests
Authorization: Bearer {jwt_token}
Query Parameters:
  - pageNumber: 1
  - pageSize: 20
  - status: Pending,Approved,Rejected
  - dateFrom: 2024-01-01
  - dateTo: 2024-12-31
```

### Update Actual Overtime Hours
```http
PUT /api/v1/overtime/requests/{requestId}/actual-hours
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "actualStartTime": "2024-11-05T18:00:00Z",
  "actualEndTime": "2024-11-05T21:30:00Z",
  "actualHours": 3.5,
  "workCompleted": "Successfully fixed critical bug and deployed to production"
}
```

### Get Overtime Summary
```http
GET /api/v1/overtime/summary
Authorization: Bearer {jwt_token}
Query Parameters:
  - month: 2024-11
  - employeeId (managers only): uuid
```

**Response:**
```json
{
  "success": true,
  "data": {
    "totalRequestedHours": 40,
    "totalApprovedHours": 35,
    "totalActualHours": 32,
    "totalPendingHours": 8,
    "overtimeByType": {
      "Regular": 20,
      "Weekend": 8,
      "Holiday": 4
    },
    "monthlyTrend": [
      {
        "date": "2024-11-01",
        "hours": 8
      }
    ]
  }
}
```

## Shift Management APIs

### Get Shift Schedule
```http
GET /api/v1/shifts/schedule
Authorization: Bearer {jwt_token}
Query Parameters:
  - startDate: 2024-11-01
  - endDate: 2024-11-30
  - view: personal|team
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "date": "2024-11-05",
      "employeeId": "uuid",
      "employeeName": "John Doe",
      "shiftName": "Morning Shift",
      "startTime": "08:00",
      "endTime": "16:00",
      "status": "Scheduled",
      "canSwap": true
    }
  ]
}
```

### Submit Shift Request
```http
POST /api/v1/shifts/requests
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "type": "ShiftSwap",
  "requestedDate": "2024-11-15",
  "originalShift": "Morning Shift",
  "requestedShift": "Evening Shift",
  "swapWithEmployeeId": "uuid",
  "reason": "Doctor appointment in the morning",
  "isRecurring": false
}
```

### Get Available Shifts for Swap
```http
GET /api/v1/shifts/available-for-swap
Authorization: Bearer {jwt_token}
Query Parameters:
  - date: 2024-11-15
  - shiftType: Morning,Evening,Night
```

### Respond to Shift Swap Request
```http
POST /api/v1/shifts/requests/{requestId}/respond
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "response": "Accept",
  "comments": "Happy to help with the swap"
}
```

## Approval Workflow APIs

### Get Pending Approvals
```http
GET /api/v1/approvals/pending
Authorization: Bearer {jwt_token}
Query Parameters:
  - pageNumber: 1
  - pageSize: 20
  - requestType: Leave,Overtime,Shift
  - priority: Normal,High,Urgent
```

**Response:**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "requestType": "Leave",
        "requestId": "uuid",
        "requestNumber": "LR-2024-001",
        "employeeName": "John Doe",
        "requestSummary": "Annual Leave: Dec 20-24, 2024 (5 days)",
        "submittedAt": "2024-11-05T10:30:00Z",
        "priority": "Normal",
        "daysOld": 2,
        "approvalLevel": 1,
        "canApprove": true,
        "canDelegate": true
      }
    ],
    "totalCount": 15,
    "urgentCount": 2,
    "overdueCount": 1
  }
}
```

### Process Approval
```http
POST /api/v1/approvals/{approvalId}/process
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "action": "Approve",
  "comments": "Approved - adequate coverage arranged",
  "conditions": [],
  "delegateTo": null
}
```

**Actions:**
- `Approve` - Fully approve the request
- `Reject` - Reject the request
- `RequestMoreInfo` - Request additional information
- `ModifyAndApprove` - Approve with modifications
- `Delegate` - Delegate to another approver

### Bulk Approve Requests
```http
POST /api/v1/approvals/bulk-process
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "approvalIds": ["uuid1", "uuid2", "uuid3"],
  "action": "Approve",
  "comments": "Bulk approval for team vacation requests"
}
```

### Get Approval History
```http
GET /api/v1/approvals/history
Authorization: Bearer {jwt_token}
Query Parameters:
  - pageNumber: 1
  - pageSize: 20
  - dateFrom: 2024-01-01
  - dateTo: 2024-12-31
  - requestType: Leave,Overtime,Shift
```

### Delegate Approval Authority
```http
POST /api/v1/approvals/delegate
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "delegateToUserId": "uuid",
  "startDate": "2024-12-15",
  "endDate": "2024-12-25",
  "requestTypes": ["Leave", "Overtime"],
  "reason": "Christmas vacation delegation"
}
```

### Get Delegation Settings
```http
GET /api/v1/approvals/delegations
Authorization: Bearer {jwt_token}
```

## Notification APIs

### Get Notifications
```http
GET /api/v1/notifications
Authorization: Bearer {jwt_token}
Query Parameters:
  - pageNumber: 1
  - pageSize: 20
  - unreadOnly: true
  - type: RequestSubmitted,ApprovalRequired,StatusUpdate
```

**Response:**
```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "uuid",
        "type": "ApprovalRequired",
        "title": "Leave Request Requires Your Approval",
        "message": "John Doe has requested 5 days of Annual Leave (Dec 20-24)",
        "isRead": false,
        "createdAt": "2024-11-05T10:30:00Z",
        "actionUrl": "/approvals/leave-request/uuid",
        "metadata": {
          "requestId": "uuid",
          "requestType": "Leave",
          "employeeName": "John Doe"
        }
      }
    ],
    "unreadCount": 5
  }
}
```

### Mark Notification as Read
```http
PUT /api/v1/notifications/{notificationId}/read
Authorization: Bearer {jwt_token}
```

### Mark All Notifications as Read
```http
PUT /api/v1/notifications/mark-all-read
Authorization: Bearer {jwt_token}
```

### Get Notification Settings
```http
GET /api/v1/notifications/settings
Authorization: Bearer {jwt_token}
```

### Update Notification Settings
```http
PUT /api/v1/notifications/settings
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "emailNotifications": true,
  "pushNotifications": false,
  "notificationTypes": {
    "requestSubmitted": true,
    "approvalRequired": true,
    "statusUpdate": true,
    "reminderAlerts": false
  },
  "digestFrequency": "Daily"
}
```

## Reporting APIs

### Get Leave Report
```http
GET /api/v1/reports/leave
Authorization: Bearer {jwt_token}
Query Parameters:
  - startDate: 2024-01-01
  - endDate: 2024-12-31
  - department: IT
  - employeeId: uuid
  - format: json|csv|pdf
```

**Response:**
```json
{
  "success": true,
  "data": {
    "summary": {
      "totalRequests": 150,
      "approvedRequests": 140,
      "rejectedRequests": 5,
      "pendingRequests": 5,
      "totalDaysRequested": 750,
      "totalDaysApproved": 700
    },
    "breakdown": {
      "byLeaveType": [
        {
          "leaveType": "Annual Leave",
          "totalRequests": 80,
          "totalDays": 400
        }
      ],
      "byMonth": [
        {
          "month": "2024-01",
          "requests": 15,
          "days": 75
        }
      ],
      "byDepartment": [
        {
          "department": "IT",
          "requests": 50,
          "days": 250
        }
      ]
    },
    "trends": {
      "averageRequestsPerEmployee": 3.2,
      "averageDaysPerRequest": 5,
      "peakMonths": ["July", "December"]
    }
  }
}
```

### Get Attendance Analytics
```http
GET /api/v1/reports/attendance-analytics
Authorization: Bearer {jwt_token}
Query Parameters:
  - startDate: 2024-01-01
  - endDate: 2024-12-31
  - department: IT
  - metric: attendance-rate|punctuality|overtime
```

### Get Manager Dashboard Data
```http
GET /api/v1/reports/manager-dashboard
Authorization: Bearer {jwt_token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "teamSummary": {
      "totalEmployees": 15,
      "presentToday": 12,
      "onLeaveToday": 2,
      "absentToday": 1
    },
    "pendingApprovals": {
      "leave": 3,
      "overtime": 1,
      "shifts": 2,
      "total": 6
    },
    "upcomingLeaves": [
      {
        "employeeName": "John Doe",
        "leaveType": "Annual Leave",
        "startDate": "2024-11-15",
        "days": 3
      }
    ],
    "teamMetrics": {
      "attendanceRate": 95.5,
      "punctualityRate": 92.0,
      "averageOvertimeHours": 8.5
    }
  }
}
```

### Export Report
```http
GET /api/v1/reports/export/{reportType}
Authorization: Bearer {jwt_token}
Query Parameters:
  - format: csv|pdf|excel
  - startDate: 2024-01-01
  - endDate: 2024-12-31
  - filters: json_encoded_filters
```

## Administrative APIs

### Manage Leave Types
```http
GET /api/v1/admin/leave-types
POST /api/v1/admin/leave-types
PUT /api/v1/admin/leave-types/{id}
DELETE /api/v1/admin/leave-types/{id}
Authorization: Bearer {jwt_token}
Permission: admin:manage
```

### Manage Workflow Templates
```http
GET /api/v1/admin/workflows
POST /api/v1/admin/workflows
PUT /api/v1/admin/workflows/{id}
DELETE /api/v1/admin/workflows/{id}
Authorization: Bearer {jwt_token}
Permission: admin:manage
```

### Bulk Update Leave Balances
```http
POST /api/v1/admin/leave-balances/bulk-update
Authorization: Bearer {jwt_token}
Content-Type: application/json

{
  "year": 2024,
  "updates": [
    {
      "employeeId": "uuid",
      "leaveTypeId": "uuid",
      "adjustment": 5,
      "reason": "Additional leave allocation"
    }
  ]
}
```

### System Settings
```http
GET /api/v1/admin/settings
PUT /api/v1/admin/settings
Authorization: Bearer {jwt_token}
Permission: admin:manage
```

### Audit Logs
```http
GET /api/v1/admin/audit-logs
Authorization: Bearer {jwt_token}
Query Parameters:
  - startDate: 2024-01-01
  - endDate: 2024-12-31
  - userId: uuid
  - action: Create,Update,Delete,Approve,Reject
  - entityType: LeaveRequest,OvertimeRequest,ShiftRequest
```

## Error Handling

### Error Response Format
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    {
      "field": "startDate",
      "code": "INVALID_DATE",
      "message": "Start date must be in the future"
    }
  ],
  "timestamp": "2024-11-05T10:30:00Z",
  "correlationId": "uuid-correlation-id"
}
```

### HTTP Status Codes
- **200 OK** - Successful operation
- **201 Created** - Resource created successfully
- **400 Bad Request** - Invalid request data
- **401 Unauthorized** - Authentication required
- **403 Forbidden** - Insufficient permissions
- **404 Not Found** - Resource not found
- **409 Conflict** - Business rule violation
- **422 Unprocessable Entity** - Validation errors
- **429 Too Many Requests** - Rate limit exceeded
- **500 Internal Server Error** - Server error

### Common Error Codes
- **VALIDATION_ERROR** - Input validation failed
- **INSUFFICIENT_BALANCE** - Not enough leave balance
- **APPROVAL_REQUIRED** - Request requires approval
- **DUPLICATE_REQUEST** - Duplicate request detected
- **BUSINESS_RULE_VIOLATION** - Business rule not met
- **RESOURCE_NOT_FOUND** - Requested resource not found
- **PERMISSION_DENIED** - Insufficient permissions
- **WORKFLOW_ERROR** - Approval workflow error

## Rate Limiting

### Rate Limits
- **General API calls**: 1000 requests per hour per user
- **Bulk operations**: 50 requests per hour per user
- **Report generation**: 20 requests per hour per user
- **File uploads**: 100 requests per hour per user

### Rate Limit Headers
```http
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 999
X-RateLimit-Reset: 1699200000
X-RateLimit-Window: 3600
```

## API Versioning

### Version Strategy
- **URL Versioning**: `/api/v1/`, `/api/v2/`
- **Header Versioning**: `X-API-Version: 1.0`
- **Backward Compatibility**: Maintained for 2 major versions
- **Deprecation Notice**: 6 months advance notice

### Version History
- **v1.0** - Initial ESS implementation
- **v1.1** - Enhanced reporting features (planned)
- **v1.2** - Mobile app optimizations (planned)
- **v2.0** - Major restructuring (future)

## SDK and Integration

### Authentication Example (C#)
```csharp
public class ESSApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string _accessToken;

    public async Task<AuthResponse> AuthenticateAsync(string username, string password)
    {
        var request = new AuthRequest { Username = username, Password = password };
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/login", request);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        
        if (authResponse.Success)
        {
            _accessToken = authResponse.Data.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", _accessToken);
        }
        
        return authResponse;
    }

    public async Task<ApiResponse<LeaveRequest>> SubmitLeaveRequestAsync(CreateLeaveRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/leave/requests", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<LeaveRequest>>();
    }
}
```

### JavaScript/TypeScript Example
```typescript
class ESSApiClient {
  private baseUrl: string;
  private accessToken: string | null = null;

  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  async authenticate(username: string, password: string): Promise<AuthResponse> {
    const response = await fetch(`${this.baseUrl}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password })
    });

    const authResponse: AuthResponse = await response.json();
    
    if (authResponse.success) {
      this.accessToken = authResponse.data.accessToken;
    }
    
    return authResponse;
  }

  async submitLeaveRequest(request: CreateLeaveRequest): Promise<ApiResponse<LeaveRequest>> {
    const response = await fetch(`${this.baseUrl}/leave/requests`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.accessToken}`
      },
      body: JSON.stringify(request)
    });

    return await response.json();
  }

  async getLeaveBalances(): Promise<ApiResponse<LeaveBalance[]>> {
    const response = await fetch(`${this.baseUrl}/leave/balances`, {
      headers: {
        'Authorization': `Bearer ${this.accessToken}`
      }
    });

    return await response.json();
  }
}
```

## Testing

### API Testing Examples

#### Unit Test Example (C#)
```csharp
[Test]
public async Task SubmitLeaveRequest_ValidRequest_ReturnsSuccess()
{
    // Arrange
    var request = new CreateLeaveRequestCommand
    {
        LeaveTypeId = Guid.NewGuid(),
        StartDate = DateTime.Today.AddDays(30),
        EndDate = DateTime.Today.AddDays(35),
        Reason = "Vacation"
    };

    // Act
    var result = await _mediator.Send(request);

    // Assert
    Assert.IsTrue(result.Success);
    Assert.IsNotNull(result.Data);
    Assert.AreEqual("Submitted", result.Data.Status);
}
```

#### Integration Test Example
```csharp
[Test]
public async Task LeaveRequestWorkflow_EndToEnd_Success()
{
    // Submit leave request
    var submitResponse = await _client.PostAsJsonAsync("/api/v1/leave/requests", createRequest);
    var leaveRequest = await submitResponse.Content.ReadFromJsonAsync<ApiResponse<LeaveRequest>>();
    
    // Manager approval
    var approvalResponse = await _managerClient.PostAsJsonAsync(
        $"/api/v1/approvals/{approvalId}/process",
        new { action = "Approve", comments = "Approved" }
    );
    
    // Verify final status
    var statusResponse = await _client.GetAsync($"/api/v1/leave/requests/{leaveRequest.Data.Id}");
    var finalRequest = await statusResponse.Content.ReadFromJsonAsync<ApiResponse<LeaveRequest>>();
    
    Assert.AreEqual("Approved", finalRequest.Data.Status);
}
```

This comprehensive API documentation provides all the necessary information for implementing and integrating with the Employee Self Service system, including detailed endpoints, request/response formats, authentication patterns, and practical examples for developers.