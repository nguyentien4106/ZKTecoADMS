
// ==========================================
// src/components/Header.tsx
// ==========================================
import { Bell, Moon, Sun } from 'lucide-react'
import { Button } from '@/components/ui/button'
import { useDarkMode } from '@/hooks/useDarkMode'

export const Header = () => {
  const { isDark, toggleDark } = useDarkMode()

  return (
    <header className="h-16 border-b border-border bg-card px-6 flex items-center justify-between">
      <div className="flex items-center gap-4">
        <h2 className="text-lg font-semibold">
          {document.title || 'ZKTeco Management'}
        </h2>
      </div>
      
      <div className="flex items-center gap-2">
        <Button variant="ghost" size="icon">
          <Bell className="w-5 h-5" />
        </Button>
        
        <Button variant="ghost" size="icon" onClick={toggleDark}>
          {isDark ? (
            <Sun className="w-5 h-5" />
          ) : (
            <Moon className="w-5 h-5" />
          )}
        </Button>

        <div className="flex items-center gap-3 ml-4 pl-4 border-l border-border">
          <div className="text-right">
            <p className="text-sm font-medium">Admin User</p>
            <p className="text-xs text-muted-foreground">admin@zkteco.com</p>
          </div>
          <div className="w-10 h-10 rounded-full bg-primary flex items-center justify-center text-primary-foreground font-semibold">
            A
          </div>
        </div>
      </div>
    </header>
  )
}