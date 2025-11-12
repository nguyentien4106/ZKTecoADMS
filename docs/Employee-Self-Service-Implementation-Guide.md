# Employee Self Service (ESS) Implementation Guide

## Overview

This document provides a comprehensive guide for implementing Employee Self Service functionality in the ZKTecoADMS system. It serves as a bridge between the detailed requirements analysis and API documentation, offering practical implementation guidance.

## Document References

1. **[Requirements Analysis](./Employee-Self-Service-Requirements-Analysis.md)** - Comprehensive business requirements, database design, and system architecture
2. **[API Documentation](./Employee-Self-Service-API-Documentation.md)** - Complete API specifications, endpoints, and integration examples

## Implementation Roadmap

### Phase 1: Foundation Setup (4-6 weeks)

#### Week 1-2: Database and Core Entities
**Priority: Critical**

1. **Create new entities and enumerations**
   - Implement all entity classes from requirements document
   - Add new enumerations (RequestStatus, ApprovalStatus, etc.)
   - Create Entity Framework configurations

2. **Database migrations**
   - Create migration scripts for new tables
   - Add indexes for performance optimization
   - Set up foreign key relationships

**Key Deliverables:**
```csharp
// Example Entity Implementation
public class EmployeeProfile : Entity<Guid>
{
    public Guid ApplicationUserId { get; set; }
    public string EmployeeId { get; set; }
    public string Department { get; set; }
    public Guid? ManagerId { get; set; }
    // ... other properties
}
```

#### Week 3-4: Extended Authentication & Authorization
**Priority: Critical**

1. **Extend role system**
   - Add Manager, HR roles to existing system
   - Implement permission-based authorization
   - Update JWT token claims

2. **Employee Profile Management**
   - Create EmployeeProfile service and repository
   - Implement manager-employee relationships
   - Add profile management APIs

#### Week 5-6: Core Infrastructure
**Priority: High**

1. **Repository Pattern Extension**
   - Implement repositories for new entities
   - Add generic repository methods for ESS operations
   - Set up unit of work pattern

2. **CQRS Commands and Queries**
   - Create base command/query structures
   - Implement MediatR handlers
   - Add validation behaviors

### Phase 2: Leave Management System (6-8 weeks)

#### Week 7-10: Leave Core Features
**Priority: Critical**

1. **Leave Types and Balances**
   ```csharp
   // Example API Implementation
   [HttpGet("leave/types")]
   public async Task<ActionResult<AppResponse<List<LeaveTypeDto>>>> GetLeaveTypes()
   {
       var query = new GetLeaveTypesQuery();
       var result = await mediator.Send(query);
       return Ok(result);
   }
   ```

2. **Leave Request Management**
   - Implement leave request submission
   - Add balance validation logic
   - Create request tracking system

3. **Frontend Components**
   ```typescript
   // Example React Component
   export const LeaveRequestForm: React.FC = () => {
     const [leaveTypes, setLeaveTypes] = useState<LeaveType[]>([]);
     const [balances, setBalances] = useState<LeaveBalance[]>([]);
     
     // Component implementation
   };
   ```

#### Week 11-14: Leave Advanced Features
**Priority: High**

1. **Leave Calendar Integration**
   - Visual calendar component
   - Team leave visibility
   - Conflict detection

2. **Document Management**
   - File upload functionality
   - Document storage and retrieval
   - Security and access control

### Phase 3: Approval Workflow Engine (4-6 weeks)

#### Week 15-18: Workflow Core
**Priority: Critical**

1. **Workflow Template System**
   ```csharp
   public class WorkflowEngine
   {
       public async Task<WorkflowInstance> StartWorkflow(
           WorkflowTemplate template, 
           IRequest request)
       {
           // Workflow initialization logic
       }
       
       public async Task<bool> ProcessApproval(
           Guid approvalId, 
           ApprovalAction action)
       {
           // Approval processing logic
       }
   }
   ```

2. **Approval Dashboard**
   - Manager approval interface
   - Bulk approval functionality
   - Delegation management

#### Week 19-20: Notification System
**Priority: High**

1. **Email Notification Service**
   ```csharp
   public class NotificationService
   {
       public async Task SendApprovalRequiredNotification(
           LeaveRequest request, 
           EmployeeProfile approver)
       {
           var template = await GetEmailTemplate("ApprovalRequired");
           await emailService.SendAsync(template, approver.Email);
       }
   }
   ```

2. **In-App Notifications**
   - Real-time notification system
   - Notification preferences
   - Mark as read functionality

### Phase 4: Extended Features (6-8 weeks)

#### Week 21-24: Overtime Management
**Priority: Medium**

1. **Overtime Request System**
   - Pre-approval workflow
   - Actual hours tracking
   - Integration with attendance system

2. **Overtime Analytics**
   - Manager reporting
   - Budget tracking
   - Trend analysis

#### Week 25-28: Shift Management
**Priority: Medium**

1. **Shift Request System**
   - Shift swap functionality
   - Schedule management
   - Team coordination

2. **Shift Calendar**
   - Visual shift planning
   - Availability tracking
   - Conflict resolution

### Phase 5: Reporting & Analytics (4-6 weeks)

#### Week 29-32: Comprehensive Reporting
**Priority: High**

1. **Report Generation Engine**
   ```csharp
   public class ReportService
   {
       public async Task<ReportData> GenerateLeaveReport(
           ReportParameters parameters)
       {
           var data = await repository.GetLeaveData(parameters);
           return reportGenerator.Generate(data, parameters.Format);
       }
   }
   ```

2. **Dashboard Analytics**
   - Employee dashboard
   - Manager dashboard
   - HR administrative dashboard

#### Week 33-34: Advanced Features
**Priority: Low**

1. **Export Functionality**
   - PDF report generation
   - CSV export
   - Excel integration

2. **Performance Optimization**
   - Query optimization
   - Caching implementation
   - Response time improvements

## Technical Implementation Details

### Project Structure
```
ZKTecoADMS.ESS/
├── Controllers/
│   ├── EmployeeProfileController.cs
│   ├── LeaveController.cs
│   ├── OvertimeController.cs
│   ├── ShiftController.cs
│   ├── ApprovalController.cs
│   └── ReportController.cs
├── Commands/
│   ├── Leave/
│   ├── Overtime/
│   ├── Shift/
│   └── Approval/
├── Queries/
├── DTOs/
├── Validators/
├── Services/
│   ├── WorkflowEngine/
│   ├── NotificationService/
│   └── ReportService/
└── Hubs/
    └── NotificationHub.cs (SignalR)
```

### Configuration Setup

#### Dependency Injection
```csharp
// In Program.cs or Startup.cs
public static void AddESSServices(this IServiceCollection services)
{
    services.AddScoped<IWorkflowEngine, WorkflowEngine>();
    services.AddScoped<INotificationService, NotificationService>();
    services.AddScoped<ILeaveBalanceService, LeaveBalanceService>();
    services.AddScoped<IReportService, ReportService>();
    
    // Add repositories
    services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
    services.AddScoped<IOvertimeRequestRepository, OvertimeRequestRepository>();
    services.AddScoped<IEmployeeProfileRepository, EmployeeProfileRepository>();
}
```

#### Database Configuration
```csharp
// In ZKTecoDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // ESS Entity Configurations
    modelBuilder.ApplyConfiguration(new EmployeeProfileConfiguration());
    modelBuilder.ApplyConfiguration(new LeaveRequestConfiguration());
    modelBuilder.ApplyConfiguration(new LeaveBalanceConfiguration());
    modelBuilder.ApplyConfiguration(new WorkflowTemplateConfiguration());
}
```

### Frontend Implementation

#### React Components Structure
```
src/
├── components/
│   ├── employee-self-service/
│   │   ├── leave/
│   │   │   ├── LeaveRequestForm.tsx
│   │   │   ├── LeaveCalendar.tsx
│   │   │   ├── LeaveBalance.tsx
│   │   │   └── LeaveHistory.tsx
│   │   ├── approval/
│   │   │   ├── ApprovalDashboard.tsx
│   │   │   ├── ApprovalCard.tsx
│   │   │   └── BulkApproval.tsx
│   │   ├── overtime/
│   │   └── shift/
│   └── shared/
│       ├── Calendar/
│       ├── FileUpload/
│       └── NotificationCenter/
├── hooks/
│   ├── useLeaveRequests.ts
│   ├── useApprovals.ts
│   └── useNotifications.ts
├── services/
│   ├── essApi.ts
│   └── notificationService.ts
└── types/
    └── ess-types.ts
```

#### API Service Implementation
```typescript
// essApi.ts
export class ESSApiService {
  private baseUrl: string;
  
  constructor() {
    this.baseUrl = process.env.REACT_APP_API_URL + '/api/v1';
  }
  
  async submitLeaveRequest(request: CreateLeaveRequest): Promise<LeaveRequest> {
    const response = await this.http.post('/leave/requests', request);
    return response.data;
  }
  
  async getLeaveBalances(): Promise<LeaveBalance[]> {
    const response = await this.http.get('/leave/balances');
    return response.data;
  }
  
  async getPendingApprovals(): Promise<PaginatedResult<Approval>> {
    const response = await this.http.get('/approvals/pending');
    return response.data;
  }
}
```

### Security Implementation

#### Role-Based Authorization
```csharp
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ESSAuthorizeAttribute : AuthorizeAttribute
{
    public ESSAuthorizeAttribute(params ESSPermission[] permissions)
    {
        Roles = string.Join(",", permissions.Select(p => p.ToString()));
    }
}

// Usage
[ESSAuthorize(ESSPermission.LeaveManage)]
[HttpPost("leave/requests/{id}/approve")]
public async Task<IActionResult> ApproveLeaveRequest(Guid id)
{
    // Implementation
}
```

#### Data Security
```csharp
public class EmployeeDataAccessService
{
    public async Task<bool> CanAccessEmployeeData(
        Guid currentUserId, 
        Guid targetEmployeeId)
    {
        // Check if same user
        if (currentUserId == targetEmployeeId) return true;
        
        // Check if manager
        var isManager = await employeeRepository
            .IsManagerOf(currentUserId, targetEmployeeId);
        if (isManager) return true;
        
        // Check if HR role
        var hasHRRole = await userManager
            .IsInRoleAsync(currentUserId, "HR");
        return hasHRRole;
    }
}
```

## Testing Strategy

### Unit Testing
```csharp
[TestFixture]
public class LeaveRequestServiceTests
{
    private Mock<ILeaveRequestRepository> _mockRepository;
    private Mock<ILeaveBalanceService> _mockBalanceService;
    private LeaveRequestService _service;
    
    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ILeaveRequestRepository>();
        _mockBalanceService = new Mock<ILeaveBalanceService>();
        _service = new LeaveRequestService(_mockRepository.Object, _mockBalanceService.Object);
    }
    
    [Test]
    public async Task SubmitLeaveRequest_InsufficientBalance_ThrowsException()
    {
        // Arrange
        _mockBalanceService.Setup(x => x.GetAvailableBalance(It.IsAny<Guid>(), It.IsAny<Guid>()))
                          .ReturnsAsync(0);
        
        // Act & Assert
        Assert.ThrowsAsync<InsufficientLeaveBalanceException>(
            () => _service.SubmitLeaveRequest(new CreateLeaveRequestCommand()));
    }
}
```

### Integration Testing
```csharp
[TestFixture]
public class LeaveRequestIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task LeaveRequestWorkflow_CompleteFlow_Success()
    {
        // Arrange
        await SeedTestData();
        
        // Act - Submit request
        var submitResponse = await Client.PostAsJsonAsync("/api/v1/leave/requests", testRequest);
        
        // Act - Approve request
        var approveResponse = await ManagerClient.PostAsJsonAsync(
            $"/api/v1/approvals/{approvalId}/process", 
            new { action = "Approve" });
        
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, approveResponse.StatusCode);
        
        var finalStatus = await GetLeaveRequestStatus(requestId);
        Assert.AreEqual("Approved", finalStatus);
    }
}
```

## Performance Considerations

### Database Optimization
```sql
-- Key indexes for performance
CREATE INDEX IX_LeaveRequest_EmployeeId_Status ON LeaveRequests (EmployeeProfileId, Status);
CREATE INDEX IX_LeaveRequest_DateRange ON LeaveRequests (StartDate, EndDate);
CREATE INDEX IX_Approval_Approver_Status ON RequestApprovals (ApproverId, Status);
CREATE INDEX IX_EmployeeProfile_Manager ON EmployeeProfiles (ManagerId);
```

### Caching Strategy
```csharp
public class LeaveBalanceService
{
    private readonly IMemoryCache _cache;
    private readonly ILeaveBalanceRepository _repository;
    
    public async Task<LeaveBalance> GetLeaveBalance(Guid employeeId, Guid leaveTypeId)
    {
        var cacheKey = $"leave_balance_{employeeId}_{leaveTypeId}";
        
        if (_cache.TryGetValue(cacheKey, out LeaveBalance cachedBalance))
        {
            return cachedBalance;
        }
        
        var balance = await _repository.GetByEmployeeAndType(employeeId, leaveTypeId);
        
        _cache.Set(cacheKey, balance, TimeSpan.FromMinutes(15));
        
        return balance;
    }
}
```

## Deployment Considerations

### Environment Configuration
```json
{
  "ESS": {
    "WorkflowTimeout": "48:00:00",
    "MaxFileUploadSize": "10MB",
    "NotificationRetryCount": 3,
    "EmailTemplatesPath": "/templates/email",
    "ReportCacheDuration": "01:00:00"
  },
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string",
    "RedisCache": "your_redis_connection"
  }
}
```

### Docker Configuration
```dockerfile
# Add to existing Dockerfile
COPY ["src/ZKTecoADMS.ESS/", "ZKTecoADMS.ESS/"]
RUN dotnet restore "ZKTecoADMS.ESS/ZKTecoADMS.ESS.csproj"
RUN dotnet build "ZKTecoADMS.ESS/ZKTecoADMS.ESS.csproj" -c Release -o /app/build
```

## Monitoring and Maintenance

### Logging Strategy
```csharp
public class LeaveRequestService
{
    private readonly ILogger<LeaveRequestService> _logger;
    
    public async Task<LeaveRequest> SubmitLeaveRequest(CreateLeaveRequestCommand command)
    {
        _logger.LogInformation("Processing leave request for employee {EmployeeId}", 
                              command.EmployeeId);
        
        try
        {
            // Implementation
            _logger.LogInformation("Leave request {RequestId} submitted successfully", 
                                  result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit leave request for employee {EmployeeId}", 
                            command.EmployeeId);
            throw;
        }
    }
}
```

### Health Checks
```csharp
public class ESSHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
                                                         CancellationToken cancellationToken = default)
    {
        try
        {
            // Check workflow engine
            await workflowEngine.HealthCheck();
            
            // Check notification service
            await notificationService.HealthCheck();
            
            return HealthCheckResult.Healthy("ESS services are healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("ESS services are unhealthy", ex);
        }
    }
}
```

## Migration from Existing System

### Data Migration Strategy
```csharp
public class ESSDataMigrator
{
    public async Task MigrateExistingData()
    {
        // 1. Create employee profiles from existing users
        await CreateEmployeeProfiles();
        
        // 2. Set up manager relationships
        await EstablishManagerRelationships();
        
        // 3. Initialize leave balances
        await InitializeLeaveBalances();
        
        // 4. Migrate historical data if needed
        await MigrateHistoricalData();
    }
    
    private async Task CreateEmployeeProfiles()
    {
        var users = await userRepository.GetAllAsync();
        
        foreach (var user in users)
        {
            var profile = new EmployeeProfile
            {
                ApplicationUserId = user.Id,
                EmployeeId = GenerateEmployeeId(user),
                Department = user.Department ?? "Unassigned",
                JoinDate = user.Created,
                Status = EmployeeStatus.Active
            };
            
            await employeeProfileRepository.AddAsync(profile);
        }
    }
}
```

## Success Metrics and KPIs

### Technical Metrics
- **API Response Time**: < 500ms for 95% of requests
- **System Availability**: 99.9% uptime
- **Database Query Performance**: < 200ms for complex queries
- **Cache Hit Ratio**: > 80% for frequently accessed data

### Business Metrics
- **User Adoption Rate**: 80% of employees using system within 3 months
- **Process Efficiency**: 70% reduction in manual processing time
- **Approval Time**: Average approval time < 24 hours
- **User Satisfaction**: > 4.5/5 rating

### Monitoring Dashboard
```csharp
public class ESSMetricsService
{
    public async Task<ESSMetrics> GetSystemMetrics()
    {
        return new ESSMetrics
        {
            TotalRequests = await GetTotalRequests(),
            AverageApprovalTime = await GetAverageApprovalTime(),
            UserAdoptionRate = await GetUserAdoptionRate(),
            SystemPerformance = await GetPerformanceMetrics()
        };
    }
}
```

This implementation guide provides a structured approach to building the Employee Self Service system, ensuring all components work together seamlessly while maintaining high quality, security, and performance standards.