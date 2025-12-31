
// ==========================================
// src/App.tsx
// ==========================================
import { Routes, Route, Navigate } from 'react-router-dom'
import { ProtectedRoute } from './components/ProtectedRoute'
import { RoleProtectedRoute } from './components/RoleProtectedRoute'
import { RouteErrorBoundary } from './components/RouteErrorBoundary'
import { MainLayout } from './layouts/MainLayout'
import { Login } from './pages/auth/Login'
import { Dashboard } from './pages/Dashboard'
import { Devices } from './pages/devices/Devices'
import { DeviceUsers } from './pages/devices/DeviceUsers'
import { Attendance } from './pages/Attendance'
import { Reports } from './pages/Reports'
import { Settings } from './pages/Settings'
import { DeviceCommands } from './pages/devices/DeviceCommands'
import { ForgotPassword } from './pages/auth/ForgotPassword'
import { ResetPassword } from './pages/auth/ResetPassword'
import { MyShifts } from './pages/shifts/MyShifts'
import { Leaves } from './pages/Leaves'
import { AllShifts } from './pages/shifts/AllShifts'
import { PendingShifts } from './pages/shifts/PendingShifts'
import { ShiftTemplate } from './pages/shifts/ShiftTemplate'
import { Profile } from './pages/Profile'
import { UserRole } from './constants/roles'
import { PATHS } from './constants/path'
import EmployeeDashboardDemo from './pages/EmployeeDashboardDemo'
import { MonthlyAttendanceSummary } from './pages/MonthlyAttendanceSummary'
import { Benefits } from './pages/Benifits'
import { Payslips } from './pages/Payslips'
import Employees from './pages/employees/Employees'
import { EmployeeBenefits } from './pages/employees/EmployeeBenefits'

function App() {
  return (
    <RouteErrorBoundary>
      <Routes>
        <Route path={PATHS.LOGIN} element={<Login />} />
        <Route path={PATHS.FORGOT_PASSWORD} element={<ForgotPassword />} />
        <Route path={PATHS.RESET_PASSWORD} element={<ResetPassword />} />
        <Route path={PATHS.DEMO_DASHBOARD} element={<EmployeeDashboardDemo />} />
        <Route
          path={PATHS.ROOT}
          element={
            <ProtectedRoute>
              <MainLayout />
            </ProtectedRoute>
          }
        >
        <Route index element={<Navigate to={PATHS.DASHBOARD} replace />} />
        
        {/* Routes accessible by all authenticated users */}
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="settings" element={<Settings />} />
        <Route path="profile" element={<Profile />} />
        
        {/* Employee can only access attendance (their own) */}
        <Route path="attendance" element={<Attendance />} />
        <Route path="attendance-summary" element={<MonthlyAttendanceSummary />} />
        
        {/* Employee can access their shifts */}
        <Route
          path="my-shifts"
          element={
            <RoleProtectedRoute requiredRole={UserRole.EMPLOYEE}>
              <MyShifts />
            </RoleProtectedRoute>
          }
        />

        <Route
          path="leaves"
          element={
            <RoleProtectedRoute requiredRole={UserRole.EMPLOYEE}>
              <Leaves />
            </RoleProtectedRoute>
          }
        />

        {/* Payslips - accessible by all authenticated users */}
        <Route path="payslips" element={<Payslips />} />
        
        {/* Manager and Admin only routes */}
        
        <Route
          path="shifts"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <AllShifts />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="pending-shifts"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <PendingShifts />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="shift-templates"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <ShiftTemplate />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="devices"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <Devices />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="device-commands"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <DeviceCommands />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="device-users"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <DeviceUsers />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="reports"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <Reports />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="benefits"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <Benefits />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="employee-benefits"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <EmployeeBenefits />
            </RoleProtectedRoute>
          }
        />
        <Route
          path="employees"
          element={
            <RoleProtectedRoute requiredRole={UserRole.MANAGER}>
              <Employees />
            </RoleProtectedRoute>
          }
        />
      </Route>

      <Route path="*" element={<Navigate to={PATHS.ROOT} replace />} />
    </Routes>
    </RouteErrorBoundary>
  )
}

export default App