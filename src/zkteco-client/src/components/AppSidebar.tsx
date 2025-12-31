import { NavLink } from 'react-router-dom'
import {
  LayoutDashboard,
  Monitor,
  Users,
  Clock,
  Settings,
  Calendar,
  CalendarCheck,
  Receipt,
  UserCircle,
  ChevronDown,
  LucideIcon,
} from 'lucide-react'
import { useRoleAccess } from '@/hooks/useRoleAccess'
import { useMemo, useState } from 'react'
import { PATHS } from '@/constants/path'
import logoIcon from '@/assets/logo-icon.png'
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarGroup,
  SidebarGroupContent,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubButton,
  SidebarMenuSubItem,
  SidebarProvider,
  SidebarRail,
  SidebarTrigger,
} from '@/components/ui/sidebar'
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from '@/components/ui/collapsible'

type MenuItem = {
  to: string
  icon: LucideIcon
  label: string
  children?: {
    to: string
    label: string
  }[]
}

const navItems: MenuItem[] = [
  { to: PATHS.DASHBOARD, icon: LayoutDashboard, label: 'Dashboard' },
  {
    to: PATHS.EMPLOYEES,
    icon: Users,
    label: 'Employees',
    children: [
      { to: PATHS.EMPLOYEES, label: 'Your Employees' },
      { to: PATHS.EMPLOYEE_BENEFITS, label: 'Employee Benefits' },
    ],
  },
  {
    to: PATHS.DEVICES,
    icon: Monitor,
    label: 'Devices',
    children: [
      { to: PATHS.DEVICES, label: 'Your Devices' },
      { to: PATHS.DEVICE_COMMANDS, label: 'Commands' },
      { to: PATHS.DEVICE_USERS, label: 'Device\' Users' },
    ],
  },
  {
    to: PATHS.ATTENDANCE,
    icon: Clock,
    label: 'Attendance',
    children: [
      { to: PATHS.ATTENDANCE, label: 'Records' },
      { to: PATHS.ATTENDANCE_SUMMARY, label: 'Summary' },
    ],
  },
  {
    to: PATHS.SHIFTS,
    icon: Calendar,
    label: 'Shifts',
    children: [
      { to: PATHS.MY_SHIFTS, label: 'My Shifts' },
      { to: PATHS.PENDING_SHIFTS, label: 'Pending Shifts' },
      { to: PATHS.SHIFTS, label: 'All Shifts' },
      { to: PATHS.SHIFT_TEMPLATES, label: 'Templates' },
    ],
  },
  { to: PATHS.LEAVES, icon: CalendarCheck, label: 'Leaves' },
  { to: PATHS.PAYSLIPS, icon: Receipt, label: 'Payslips' },
  { 
    to: PATHS.BENEFITS, 
    icon: Settings, 
    label: 'Benefits',
    children: [
        { to: PATHS.BENEFITS, label: 'Salary Profiles' },
    ]
},
]

export function AppSidebar() {
  const { canAccessRoute } = useRoleAccess()
  const [openMenus, setOpenMenus] = useState<Record<string, boolean>>({})

  // Filter navigation items based on user's role
  const allowedNavItems = useMemo(() => {
    return navItems
      .map((item) => {
        // If item has children, filter the children based on access
        if (item.children && item.children.length > 0) {
          const allowedChildren = item.children.filter((child) =>
            canAccessRoute(child.to)
          )
          
          // Only include parent if it has at least one accessible child
          if (allowedChildren.length > 0) {
            return { ...item, children: allowedChildren }
          }
          return null
        }
        
        // For items without children, check if user can access the route
        return canAccessRoute(item.to) ? item : null
      })
      .filter((item): item is MenuItem => item !== null)
  }, [canAccessRoute])

  const toggleMenu = (label: string) => {
    setOpenMenus((prev) => ({ ...prev, [label]: !prev[label] }))
  }

  return (
    <Sidebar collapsible="icon" style={{ width: 200 }}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size="lg" asChild>
              <a href="/">
                <div className="flex aspect-square size-8 items-center justify-center rounded-lg bg-sidebar-primary text-sidebar-primary-foreground">
                    <img src={logoIcon} alt="Logo" className="w-15 h-10 inline-block" />
                </div>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-semibold">
                    <i>work</i>
                    <b style={{ color: '#FFD700' }}>Fina</b>
                  </span>
                  <span className="truncate text-xs">Enterprise</span>
                </div>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupContent>
            <SidebarMenu>
              {allowedNavItems.map((item) => {
                if (item.children && item.children.length > 0) {
                  return (
                    <Collapsible
                      key={item.label}
                      open={openMenus[item.label] || false}
                      onOpenChange={() => toggleMenu(item.label)}
                      className="group/collapsible"
                    >
                      <SidebarMenuItem>
                        <CollapsibleTrigger asChild>
                          <SidebarMenuButton tooltip={item.label}>
                            <item.icon className="w-5 h-5" />
                            <span>{item.label}</span>
                            <ChevronDown className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-180" />
                          </SidebarMenuButton>
                        </CollapsibleTrigger>
                        <CollapsibleContent>
                          <SidebarMenuSub>
                            {item.children.map((child) => (
                              <SidebarMenuSubItem key={child.to}>
                                <SidebarMenuSubButton asChild>
                                  <NavLink
                                    to={child.to}
                                    className={({ isActive }) =>
                                      isActive ? 'font-semibold' : ''
                                    }
                                  >
                                    <span>{child.label}</span>
                                  </NavLink>
                                </SidebarMenuSubButton>
                              </SidebarMenuSubItem>
                            ))}
                          </SidebarMenuSub>
                        </CollapsibleContent>
                      </SidebarMenuItem>
                    </Collapsible>
                  )
                }

                return (
                  <SidebarMenuItem key={item.to}>
                    <SidebarMenuButton asChild tooltip={item.label}>
                      <NavLink
                        to={item.to}
                        className={({ isActive }) =>
                          isActive ? 'font-semibold' : ''
                        }
                      >
                        <item.icon className="w-5 h-5" />
                        <span>{item.label}</span>
                      </NavLink>
                    </SidebarMenuButton>
                  </SidebarMenuItem>
                )
              })}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>
      <SidebarFooter>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton asChild size="lg">
              <a href={PATHS.PROFILE}>
                <UserCircle className="w-5 h-5" />
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate font-semibold">User Profile</span>
                  <span className="truncate text-xs">Manage Account</span>
                </div>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarFooter>
      <SidebarRail />
    </Sidebar>
  )
}

// Export the wrapped version with provider
export function SidebarWithProvider({ children }: { children: React.ReactNode }) {
  return (
    <SidebarProvider>
      <AppSidebar />
      <main className="flex-1 w-full">
        <div className="flex items-center gap-2 border-b px-4 py-2">
          <SidebarTrigger />
          <div className="flex-1" />
        </div>
        <div className="p-4">{children}</div>
      </main>
    </SidebarProvider>
  )
}
