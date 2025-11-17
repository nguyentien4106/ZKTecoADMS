
// ==========================================
// src/App.tsx
// ==========================================
import { Routes, Route, Navigate } from 'react-router-dom'
import { ProtectedRoute } from './components/ProtectedRoute'
import { RoleProtectedRoute } from './components/RoleProtectedRoute'
import { MainLayout } from './layouts/MainLayout'
import { Login } from './pages/auth/Login'
import { Dashboard } from './pages/Dashboard'
import { Devices } from './pages/Devices'
import { Employees } from './pages/Employees'
import { Attendance } from './pages/Attendance'
import { Reports } from './pages/Reports'
import { Settings } from './pages/Settings'
import { DeviceCommands } from './pages/DeviceCommands'
import { ForgotPassword } from './pages/auth/ForgotPassword'
import { ResetPassword } from './pages/auth/ResetPassword'
import { UserRole } from './constants/roles'

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />
      <Route path="/forgot-password" element={<ForgotPassword />} />
      <Route path="/reset-password" element={<ResetPassword />} />
      
      <Route
        path="/"
        element={
          <ProtectedRoute>
            <MainLayout />
          </ProtectedRoute>
        }
      >
        <Route index element={<Navigate to="/dashboard" replace />} />
        
        {/* Routes accessible by all authenticated users */}
        <Route path="dashboard" element={<Dashboard />} />
        <Route path="settings" element={<Settings />} />
        
        {/* Employee can only access attendance (their own) */}
        <Route path="attendance" element={<Attendance />} />
        
        {/* Manager and Admin only routes */}
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
          path="employees"
          element={
            <RoleProtectedRoute requiredRole={UserRole.EMPLOYEE}>
              <Employees />
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
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}

export default App