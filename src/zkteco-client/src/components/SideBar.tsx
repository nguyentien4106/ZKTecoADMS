
// ==========================================
// src/components/Sidebar.tsx
// ==========================================
import { NavLink } from 'react-router-dom'
import { 
  LayoutDashboard, 
  Monitor, 
  Users, 
  Clock, 
  FileText, 
  Settings 
} from 'lucide-react'
import { cn } from '@/lib/utils'

const navItems = [
  { to: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
  { to: '/devices', icon: Monitor, label: 'Devices' },
  { to: '/users', icon: Users, label: 'Users' },
  { to: '/attendance', icon: Clock, label: 'Attendance' },
  { to: '/reports', icon: FileText, label: 'Reports' },
  { to: '/settings', icon: Settings, label: 'Settings' },
]

export const Sidebar = () => {
  return (
    <aside className="w-64 bg-card border-r border-border">
      <div className="flex items-center h-16 px-6 border-b border-border">
        <h1 className="text-xl font-bold text-primary">ZKTeco Manager</h1>
      </div>
      <nav className="p-4 space-y-1">
        {navItems.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) =>
              cn(
                'flex items-center gap-3 px-4 py-3 rounded-lg transition-colors',
                'hover:bg-accent hover:text-accent-foreground',
                isActive
                  ? 'bg-primary text-primary-foreground'
                  : 'text-muted-foreground'
              )
            }
          >
            <item.icon className="w-5 h-5" />
            <span className="font-medium">{item.label}</span>
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}
