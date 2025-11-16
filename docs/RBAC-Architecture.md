# Role-Based Access Control - Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           FRONTEND RBAC SYSTEM                           │
└─────────────────────────────────────────────────────────────────────────┘

                            ┌──────────────┐
                            │  JWT Token   │
                            │  from Backend│
                            └──────┬───────┘
                                   │
                                   ▼
                        ┌──────────────────┐
                        │  AuthContext     │
                        │  - Decodes JWT   │
                        │  - Extracts role │
                        └────────┬─────────┘
                                 │
                                 │ provides user data
                                 │
                ┌────────────────┴────────────────┐
                │                                 │
                ▼                                 ▼
        ┌───────────────┐                ┌───────────────┐
        │ useRoleAccess │                │  Components   │
        │     Hook      │                │  & Routes     │
        └───────┬───────┘                └───────┬───────┘
                │                                │
                │ provides utilities             │ consumes
                │                                │
                └────────────┬───────────────────┘
                             │
             ┌───────────────┼───────────────┐
             │               │               │
             ▼               ▼               ▼
    ┌────────────┐  ┌────────────┐  ┌────────────┐
    │  Sidebar   │  │   Routes   │  │    UI      │
    │  Filter    │  │   Guard    │  │  Elements  │
    └────────────┘  └────────────┘  └────────────┘


┌─────────────────────────────────────────────────────────────────────────┐
│                         ROLE HIERARCHY                                   │
└─────────────────────────────────────────────────────────────────────────┘

    ┌─────────────────┐
    │     ADMIN       │  Level 3 - Highest
    │  (All Access)   │
    └────────┬────────┘
             │ inherits
             ▼
    ┌─────────────────┐
    │    MANAGER      │  Level 2 - Medium
    │ (Team & Devices)│
    └────────┬────────┘
             │ inherits
             ▼
    ┌─────────────────┐
    │    EMPLOYEE     │  Level 1 - Lowest
    │  (Own Data)     │
    └─────────────────┘


┌─────────────────────────────────────────────────────────────────────────┐
│                      PERMISSION FLOW                                     │
└─────────────────────────────────────────────────────────────────────────┘

User clicks Navigation Item or enters URL
              ↓
    ┌─────────────────┐
    │ Is user logged  │ NO → Redirect to /login
    │      in?        │
    └────────┬────────┘
             │ YES
             ▼
    ┌─────────────────┐
    │ Check route in  │ NO → Redirect to /dashboard
    │ ROLE_PERMISSIONS│
    └────────┬────────┘
             │ YES
             ▼
    ┌─────────────────┐
    │ RoleProtected   │ NO → Redirect to /dashboard
    │ Route Check     │
    └────────┬────────┘
             │ YES
             ▼
    ┌─────────────────┐
    │  Render Page    │
    └────────┬────────┘
             │
             ▼
    ┌─────────────────┐
    │ RoleGuard for   │ → Hide/Show UI elements
    │  UI Elements    │
    └─────────────────┘


┌─────────────────────────────────────────────────────────────────────────┐
│                    COMPONENT RELATIONSHIPS                               │
└─────────────────────────────────────────────────────────────────────────┘

constants/roles.ts
    │
    │ exports: UserRole enum, ROLE_HIERARCHY, ROLE_PERMISSIONS
    │
    ├──→ hooks/useRoleAccess.ts
    │       │
    │       │ exports: useRoleAccess hook
    │       │
    │       ├──→ components/RoleProtectedRoute.tsx
    │       │       (protects routes)
    │       │
    │       ├──→ components/RoleGuard.tsx
    │       │       (protects UI elements)
    │       │
    │       └──→ components/SideBar.tsx
    │               (filters navigation)
    │
    └──→ App.tsx
            (configures route protection)


┌─────────────────────────────────────────────────────────────────────────┐
│                    DATA FLOW EXAMPLE                                     │
└─────────────────────────────────────────────────────────────────────────┘

1. User logs in
   Backend sends JWT with role: "Manager"
   
2. Frontend decodes JWT
   AuthContext stores: { role: "Manager", ... }
   
3. User navigates to /devices
   - Sidebar checks: canAccessRoute("/devices") → TRUE (Manager allowed)
   - Route guard checks: hasMinimumRole(MANAGER) → TRUE
   - Page renders
   
4. In Devices page, UI renders:
   <RoleGuard requiredRole={UserRole.ADMIN}>
     <DeleteAllButton />  ← NOT rendered (Manager < Admin)
   </RoleGuard>
   
   <RoleGuard requiredRole={UserRole.MANAGER}>
     <AddDeviceButton />  ← Rendered (Manager >= Manager)
   </RoleGuard>

5. Backend API call to /api/devices
   - Backend checks JWT role
   - If Manager: returns only assigned devices
   - If Admin: returns all devices


┌─────────────────────────────────────────────────────────────────────────┐
│                    SECURITY LAYERS                                       │
└─────────────────────────────────────────────────────────────────────────┘

Layer 1: Navigation (UX)
    └─ Sidebar filters menu items
       ✓ Prevents confusion
       ✗ NOT security (can be bypassed)

Layer 2: Route Protection
    └─ RoleProtectedRoute guards routes
       ✓ Redirects unauthorized access
       ✗ NOT security (frontend only)

Layer 3: Component Protection
    └─ RoleGuard hides UI elements
       ✓ Clean interface
       ✗ NOT security (can be manipulated)

Layer 4: Backend API (CRITICAL) ⚠️
    └─ MUST enforce all permissions
       ✓ TRUE security
       ✓ Cannot be bypassed
       Required: Role-based API filtering


┌─────────────────────────────────────────────────────────────────────────┐
│                    FILE STRUCTURE                                        │
└─────────────────────────────────────────────────────────────────────────┘

src/
├── constants/
│   ├── auth.ts              (JWT claims)
│   └── roles.ts             (Role definitions) ✨ NEW
│
├── hooks/
│   └── useRoleAccess.ts     (Role utilities) ✨ NEW
│
├── components/
│   ├── SideBar.tsx          (Modified - filters nav) ⚡ UPDATED
│   ├── RoleProtectedRoute.tsx  (Route guard) ✨ NEW
│   └── RoleGuard.tsx        (Component guard) ✨ NEW
│
├── contexts/
│   └── AuthContext.tsx      (User auth state)
│
└── App.tsx                  (Route config) ⚡ UPDATED

docs/
├── RBAC-Implementation.md     (Full docs) ✨ NEW
├── RBAC-Summary.md            (Quick reference) ✨ NEW
└── examples/
    └── RoleGuard-Usage-Examples.tsx  (Examples) ✨ NEW
```

## Quick Reference

### Check if user has specific role
```typescript
const { hasRole } = useRoleAccess()
if (hasRole(UserRole.ADMIN)) { /* ... */ }
```

### Check if user has minimum role level
```typescript
const { hasMinimumRole } = useRoleAccess()
if (hasMinimumRole(UserRole.MANAGER)) { 
  // True for Manager and Admin
}
```

### Protect a route
```tsx
<RoleProtectedRoute requiredRole={UserRole.MANAGER}>
  <Component />
</RoleProtectedRoute>
```

### Conditionally render UI
```tsx
<RoleGuard requiredRole={UserRole.ADMIN}>
  <AdminButton />
</RoleGuard>
```
