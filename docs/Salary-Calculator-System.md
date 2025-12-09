# Salary Calculator System - Implementation Guide

## Overview
A comprehensive salary management system has been implemented for ZKTeco ADMS, allowing managers to create salary profiles with different rate types and assign them to employees.

## Features

### 1. Salary Profile Management
- **Create Salary Profiles**: Define salary rates with different types (Hourly, Daily, Monthly, Yearly)
- **Update Profiles**: Modify existing salary profiles and activate/deactivate them
- **Delete Profiles**: Remove salary profiles that are no longer needed
- **Rate Multipliers**: Support for overtime, holiday, and night shift multipliers

### 2. Employee Salary Assignment
- **Assign Profiles**: Assign salary profiles to employees with effective dates
- **One Active Profile**: Each employee can only have one active salary profile at a time
- **Profile History**: Track salary profile assignments over time with effective and end dates
- **Notes**: Add optional notes when assigning profiles

### 3. Salary Rate Types
- **Hourly**: Rate paid per hour worked
- **Daily**: Rate paid per day
- **Monthly**: Fixed monthly salary
- **Yearly**: Annual salary

## Database Schema

### SalaryProfile Table
```sql
- Id (Guid, Primary Key)
- Name (string, required, max 200 chars)
- Description (string, optional, max 500 chars)
- RateType (enum: Hourly/Daily/Monthly/Yearly)
- Rate (decimal, required)
- Currency (string, required, max 10 chars, default: "USD")
- OvertimeMultiplier (decimal, optional)
- HolidayMultiplier (decimal, optional)
- NightShiftMultiplier (decimal, optional)
- IsActive (bool, default: true)
- CreatedAt, UpdatedAt, etc. (inherited from AuditableEntity)
```

### EmployeeSalaryProfile Table
```sql
- Id (Guid, Primary Key)
- EmployeeId (Guid, Foreign Key to Employee)
- SalaryProfileId (Guid, Foreign Key to SalaryProfile)
- EffectiveDate (DateTime, required)
- EndDate (DateTime, optional)
- IsActive (bool, default: true)
- Notes (string, optional, max 500 chars)
- CreatedAt, UpdatedAt, etc. (inherited from AuditableEntity)
```

## API Endpoints

### Salary Profiles
- `GET /api/salaryprofiles?activeOnly={bool}` - Get all salary profiles
- `GET /api/salaryprofiles/{id}` - Get salary profile by ID
- `POST /api/salaryprofiles` - Create new salary profile
- `PUT /api/salaryprofiles/{id}` - Update salary profile
- `DELETE /api/salaryprofiles/{id}` - Delete salary profile

### Employee Salary Assignments
- `POST /api/salaryprofiles/assign` - Assign salary profile to employee
- `GET /api/salaryprofiles/employee/{employeeId}` - Get active salary profile for employee

## Backend Implementation

### Domain Layer
- **Entities**: `SalaryProfile`, `EmployeeSalaryProfile`
- **Enums**: `SalaryRateType`
- **Repositories**: `ISalaryProfileRepository`, `IEmployeeSalaryProfileRepository`

### Application Layer
- **Commands**:
  - `CreateSalaryProfileCommand`
  - `UpdateSalaryProfileCommand`
  - `DeleteSalaryProfileCommand`
  - `AssignSalaryProfileCommand`
- **Queries**:
  - `GetAllSalaryProfilesQuery`
  - `GetSalaryProfileByIdQuery`
  - `GetEmployeeSalaryProfileQuery`
- **DTOs**: Complete set of request/response DTOs

### Infrastructure Layer
- **Repository Implementations**: Full EF Core implementations
- **Configurations**: Entity configurations for both tables
- **Migration**: `AddSalaryProfiles` migration created

## Frontend Implementation

### Pages
- **SalaryProfiles.tsx**: Main salary profiles management page
  - List all salary profiles with filtering
  - Create new salary profiles
  - Edit existing salary profiles
  - Delete salary profiles
  - Toggle active/inactive status

### Components
- **AssignSalaryDialog.tsx**: Dialog component for assigning salary profiles to employees
  - Select from active salary profiles
  - Set effective date
  - Add optional notes
  - Preview profile details

### Services
- **salaryProfileService.ts**: Complete API integration service

### Routing
- `/salary-profiles` - Manager/Admin only route

## Usage Guide

### For Managers/Admins

#### Creating a Salary Profile
1. Navigate to "Salary Profiles" from the main menu
2. Click "Create Profile" button
3. Fill in the form:
   - Name (required): e.g., "Senior Developer Rate"
   - Description (optional): Additional details
   - Rate Type (required): Select Hourly, Daily, Monthly, or Yearly
   - Rate (required): Enter the base rate
   - Currency (required): e.g., "USD"
   - Optional multipliers:
     - Overtime Multiplier: e.g., 1.5 (150% of base rate)
     - Holiday Multiplier: e.g., 2.0 (200% of base rate)
     - Night Shift Multiplier: e.g., 1.3 (130% of base rate)
4. Click "Create Profile"

#### Assigning a Salary Profile to an Employee
1. Navigate to the Employees page
2. Use the AssignSalaryDialog component (integrate it into employee management)
3. Select the employee
4. Choose a salary profile
5. Set the effective date
6. Add optional notes
7. Click "Assign Profile"

Note: When assigning a new profile, any previously active profile for that employee will be automatically deactivated.

#### Viewing Employee Salary Information
- Use the API endpoint `GET /api/salaryprofiles/employee/{employeeId}` to view an employee's current active salary profile

### Example Data

#### Sample Salary Profiles
```json
{
  "name": "Junior Developer - Monthly",
  "rateType": 3,
  "rate": 3000,
  "currency": "USD",
  "overtimeMultiplier": 1.5,
  "holidayMultiplier": 2.0
}
```

```json
{
  "name": "Hourly Contractor",
  "rateType": 1,
  "rate": 50,
  "currency": "USD",
  "overtimeMultiplier": 1.5
}
```

## Security & Authorization
- All salary profile endpoints require **Manager** or **Admin** role
- Employees can view their own salary information
- Proper authorization policies are enforced at the controller level

## Database Migration
To apply the database migration:
```bash
cd src/ZKTecoADMS.Infrastructure
dotnet ef database update --startup-project ../ZKTecoADMS.Api
```

Or run the migrations automatically when the application starts.

## Integration with Employee Module

To fully integrate the salary assignment feature into the employee management workflow:

1. Add a "Assign Salary" button to the employee list/details page
2. Import the `AssignSalaryDialog` component
3. Display the current salary profile in employee details
4. Show salary history if needed

Example integration:
```tsx
import { AssignSalaryDialog } from "@/components/salary/AssignSalaryDialog";

const [showAssignDialog, setShowAssignDialog] = useState(false);
const [selectedEmployee, setSelectedEmployee] = useState(null);

// In your employee actions
<Button onClick={() => {
  setSelectedEmployee(employee);
  setShowAssignDialog(true);
}}>
  Assign Salary
</Button>

<AssignSalaryDialog
  open={showAssignDialog}
  onOpenChange={setShowAssignDialog}
  employeeId={selectedEmployee?.id}
  employeeName={selectedEmployee?.name}
  onSuccess={() => {
    // Refresh employee data
  }}
/>
```

## Future Enhancements

Potential improvements to consider:
1. **Salary Calculation Engine**: Calculate actual salaries based on attendance and work hours
2. **Payroll Integration**: Export salary data for payroll processing
3. **Salary Reports**: Generate salary reports and analytics
4. **Bulk Assignment**: Assign salary profiles to multiple employees at once
5. **Profile Templates**: Create reusable salary profile templates
6. **Currency Conversion**: Support multiple currencies with conversion
7. **Salary Increments**: Track and manage salary increases over time
8. **Performance-based Bonuses**: Add bonus calculation features

## Testing

### API Testing with cURL
```bash
# Get all salary profiles
curl -X GET "http://localhost:8080/api/salaryprofiles" \
  -H "Authorization: Bearer {token}"

# Create salary profile
curl -X POST "http://localhost:8080/api/salaryprofiles" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Senior Developer",
    "rateType": 3,
    "rate": 5000,
    "currency": "USD",
    "overtimeMultiplier": 1.5
  }'

# Assign to employee
curl -X POST "http://localhost:8080/api/salaryprofiles/assign" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "employeeId": "{employee-guid}",
    "salaryProfileId": "{profile-guid}",
    "effectiveDate": "2025-01-01",
    "notes": "Annual review increase"
  }'
```

## Summary

This implementation provides a complete, production-ready salary management system with:
✅ Full CRUD operations for salary profiles
✅ Employee salary assignment with history tracking
✅ Multiple rate types and multipliers
✅ Clean architecture with proper separation of concerns
✅ Type-safe frontend with React and TypeScript
✅ RESTful API with proper authorization
✅ Database migrations and configurations
✅ User-friendly UI with dialogs and forms

The system is ready to use and can be extended based on your specific business requirements.
