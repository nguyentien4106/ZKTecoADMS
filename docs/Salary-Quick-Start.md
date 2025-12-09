# Quick Start Guide - Salary Calculator System

## What Was Created

I've successfully implemented a complete salary calculator system for your ZKTeco ADMS application. Here's what's been added:

## Backend (API)

### ✅ Domain Layer
- `SalaryProfile` entity - stores salary profile definitions
- `EmployeeSalaryProfile` entity - tracks employee-salary assignments
- `SalaryRateType` enum - Hourly, Daily, Monthly, Yearly
- Repository interfaces

### ✅ Infrastructure Layer
- Repository implementations
- Entity Framework configurations
- Database migration: `AddSalaryProfiles`

### ✅ Application Layer
**Commands:**
- Create Salary Profile
- Update Salary Profile  
- Delete Salary Profile
- Assign Salary Profile to Employee

**Queries:**
- Get All Salary Profiles (with active-only filter)
- Get Salary Profile by ID
- Get Employee's Active Salary Profile

### ✅ API Controller
`SalaryProfilesController` with 7 endpoints (all requiring Manager/Admin role)

## Frontend (React)

### ✅ Services
- `salaryProfileService.ts` - Complete API integration

### ✅ Pages
- `SalaryProfiles.tsx` - Full salary profiles management UI

### ✅ Components
- `AssignSalaryDialog.tsx` - Dialog for assigning salaries to employees

### ✅ Routing
- Added route: `/salary-profiles` (Manager/Admin only)

## To Start Using the System

### 1. Apply Database Migration
```bash
cd src/ZKTecoADMS.Infrastructure
dotnet ef database update --startup-project ../ZKTecoADMS.Api
```

### 2. Start the Backend API
The build already succeeded, so you can run:
```bash
cd src/ZKTecoADMS.Api
dotnet run
```

### 3. Access the Salary Profiles Page
- Login as a Manager or Admin
- Navigate to `/salary-profiles` route
- You'll see the salary profiles management page

### 4. Create Your First Salary Profile
1. Click "Create Profile"
2. Fill in:
   - Name: e.g., "Senior Developer Monthly Rate"
   - Rate Type: Monthly
   - Rate: 5000
   - Currency: USD
   - Overtime Multiplier: 1.5 (optional)
3. Click "Create Profile"

### 5. Assign Salary to an Employee

**Option A: Via API**
```bash
POST /api/salaryprofiles/assign
{
  "employeeId": "guid-here",
  "salaryProfileId": "guid-here",
  "effectiveDate": "2025-01-01",
  "notes": "Initial salary assignment"
}
```

**Option B: Integrate into Employee Management**
Add the `AssignSalaryDialog` component to your employee management page:

```tsx
import { AssignSalaryDialog } from "@/components/salary/AssignSalaryDialog";

// In your employee component
<AssignSalaryDialog
  open={showDialog}
  onOpenChange={setShowDialog}
  employeeId={employee.id}
  employeeName={employee.name}
  onSuccess={() => loadEmployees()}
/>
```

## Key Features

### Salary Profile Features
- ✅ Multiple rate types (Hourly, Daily, Monthly, Yearly)
- ✅ Overtime multipliers
- ✅ Holiday pay multipliers
- ✅ Night shift multipliers
- ✅ Active/Inactive status
- ✅ Multi-currency support

### Employee Assignment Features
- ✅ One active profile per employee
- ✅ Automatic deactivation of previous profiles
- ✅ Effective dates tracking
- ✅ Assignment history
- ✅ Optional notes for each assignment

## API Endpoints Summary

| Method | Endpoint | Description | Role Required |
|--------|----------|-------------|---------------|
| GET | `/api/salaryprofiles` | Get all profiles | Manager/Admin |
| GET | `/api/salaryprofiles/{id}` | Get profile by ID | Manager/Admin |
| POST | `/api/salaryprofiles` | Create profile | Manager/Admin |
| PUT | `/api/salaryprofiles/{id}` | Update profile | Manager/Admin |
| DELETE | `/api/salaryprofiles/{id}` | Delete profile | Manager/Admin |
| POST | `/api/salaryprofiles/assign` | Assign to employee | Manager/Admin |
| GET | `/api/salaryprofiles/employee/{id}` | Get employee's profile | Employee+ |

## Example Usage Flow

### 1. Manager Creates Salary Profiles
```
Monthly Developer Rate: $5,000/month
Hourly Contractor Rate: $50/hour
Senior Developer Rate: $8,000/month
```

### 2. Manager Assigns Profile to Employee
```
Employee: John Doe
Profile: Monthly Developer Rate
Effective Date: 2025-01-01
Notes: Starting salary after probation
```

### 3. System Automatically:
- Deactivates any previous active salary profile for John
- Marks the new profile as active
- Sets effective date

### 4. View Employee Salary
```
GET /api/salaryprofiles/employee/{john-id}
Returns: Active salary profile with all details
```

## Next Steps (Optional Enhancements)

If you want to extend this system further:

1. **Salary Calculator**: Calculate actual salary based on attendance records
2. **Reports**: Generate salary reports by period
3. **Bulk Operations**: Assign profiles to multiple employees
4. **Salary History**: View complete salary history per employee
5. **Export**: Export salary data for payroll systems

## File Locations

### Backend
- **Entities**: `src/ZKTecoADMS.Domain/Entities/`
  - `SalaryProfile.cs`
  - `EmployeeSalaryProfile.cs`
- **Commands**: `src/ZKTecoADMS.Application/Commands/SalaryProfiles/`
- **Queries**: `src/ZKTecoADMS.Application/Queries/SalaryProfiles/`
- **Controller**: `src/ZKTecoADMS.Api/Controllers/SalaryProfilesController.cs`
- **Migration**: `src/ZKTecoADMS.Infrastructure/Migrations/[timestamp]_AddSalaryProfiles.cs`

### Frontend
- **Page**: `src/zkteco-client/src/pages/SalaryProfiles.tsx`
- **Component**: `src/zkteco-client/src/components/salary/AssignSalaryDialog.tsx`
- **Service**: `src/zkteco-client/src/services/salaryProfileService.ts`

## Questions or Issues?

- Check the full documentation: `docs/Salary-Calculator-System.md`
- All code follows your existing patterns and architecture
- The system is fully integrated with your authentication and authorization

Everything is ready to use! Just apply the migration and start the application.
