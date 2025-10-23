# Docker Management Script for ZKTeco ADMS
# PowerShell script for Windows

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('start', 'stop', 'restart', 'logs', 'clean', 'reset', 'backup', 'restore', 'status')]
    [string]$Command = 'help'
)

function Show-Help {
    Write-Host ""
    Write-Host "ZKTeco ADMS Docker Management Script" -ForegroundColor Cyan
    Write-Host "====================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage: .\docker-manage.ps1 [command]" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor Green
    Write-Host "  start     - Start all services"
    Write-Host "  stop      - Stop all services"
    Write-Host "  restart   - Restart all services"
    Write-Host "  logs      - Show logs from all services"
    Write-Host "  clean     - Stop and remove containers (keep volumes)"
    Write-Host "  reset     - Stop and remove everything including volumes"
    Write-Host "  backup    - Backup PostgreSQL database"
    Write-Host "  restore   - Restore PostgreSQL database from backup"
    Write-Host "  status    - Show status of all services"
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor Magenta
    Write-Host "  .\docker-manage.ps1 start"
    Write-Host "  .\docker-manage.ps1 logs"
    Write-Host "  .\docker-manage.ps1 backup"
    Write-Host ""
}

function Start-Services {
    Write-Host "Starting all services..." -ForegroundColor Green
    docker-compose up -d --build
    Write-Host "Services started! Waiting for health checks..." -ForegroundColor Green
    Start-Sleep -Seconds 5
    docker-compose ps
    Write-Host ""
    Write-Host "Access the application at:" -ForegroundColor Cyan
    Write-Host "  Frontend: http://localhost:3000" -ForegroundColor Yellow
    Write-Host "  API:      http://localhost:7070" -ForegroundColor Yellow
    Write-Host "  pgAdmin:  http://localhost:5050" -ForegroundColor Yellow
}

function Stop-Services {
    Write-Host "Stopping all services..." -ForegroundColor Yellow
    docker-compose down
    Write-Host "All services stopped." -ForegroundColor Green
}

function Restart-Services {
    Write-Host "Restarting all services..." -ForegroundColor Yellow
    docker-compose restart
    Write-Host "All services restarted." -ForegroundColor Green
    docker-compose ps
}

function Show-Logs {
    Write-Host "Showing logs (Ctrl+C to exit)..." -ForegroundColor Cyan
    docker-compose logs -f
}

function Clean-Services {
    Write-Host "Cleaning up containers..." -ForegroundColor Yellow
    $confirm = Read-Host "This will stop and remove all containers. Continue? (y/N)"
    if ($confirm -eq 'y' -or $confirm -eq 'Y') {
        docker-compose down
        Write-Host "Cleanup complete. Volumes preserved." -ForegroundColor Green
    } else {
        Write-Host "Operation cancelled." -ForegroundColor Red
    }
}

function Reset-Services {
    Write-Host "WARNING: This will remove all data!" -ForegroundColor Red
    $confirm = Read-Host "This will delete all containers, images, and volumes. Are you sure? (y/N)"
    if ($confirm -eq 'y' -or $confirm -eq 'Y') {
        docker-compose down -v --rmi all
        Write-Host "Full reset complete." -ForegroundColor Green
    } else {
        Write-Host "Operation cancelled." -ForegroundColor Red
    }
}

function Backup-Database {
    $backupFile = "backup_$(Get-Date -Format 'yyyyMMdd_HHmmss').sql"
    Write-Host "Creating database backup: $backupFile" -ForegroundColor Cyan
    
    docker exec zkteco_postgres pg_dump -U postgres ZKTecoIntegration > $backupFile
    
    if (Test-Path $backupFile) {
        Write-Host "Backup created successfully: $backupFile" -ForegroundColor Green
        $size = (Get-Item $backupFile).Length / 1KB
        Write-Host "Size: $([math]::Round($size, 2)) KB" -ForegroundColor Yellow
    } else {
        Write-Host "Backup failed!" -ForegroundColor Red
    }
}

function Restore-Database {
    $backupFiles = Get-ChildItem -Filter "backup_*.sql" | Sort-Object LastWriteTime -Descending
    
    if ($backupFiles.Count -eq 0) {
        Write-Host "No backup files found!" -ForegroundColor Red
        return
    }
    
    Write-Host "Available backup files:" -ForegroundColor Cyan
    for ($i = 0; $i -lt $backupFiles.Count; $i++) {
        Write-Host "  [$i] $($backupFiles[$i].Name) - $($backupFiles[$i].LastWriteTime)"
    }
    
    $selection = Read-Host "Select backup file number to restore"
    
    if ($selection -match '^\d+$' -and [int]$selection -lt $backupFiles.Count) {
        $backupFile = $backupFiles[[int]$selection].Name
        Write-Host "Restoring from: $backupFile" -ForegroundColor Yellow
        
        $confirm = Read-Host "This will overwrite existing data. Continue? (y/N)"
        if ($confirm -eq 'y' -or $confirm -eq 'Y') {
            Get-Content $backupFile | docker exec -i zkteco_postgres psql -U postgres -d ZKTecoIntegration
            Write-Host "Database restored successfully!" -ForegroundColor Green
        } else {
            Write-Host "Restore cancelled." -ForegroundColor Red
        }
    } else {
        Write-Host "Invalid selection!" -ForegroundColor Red
    }
}

function Show-Status {
    Write-Host "Service Status:" -ForegroundColor Cyan
    docker-compose ps
    Write-Host ""
    Write-Host "Resource Usage:" -ForegroundColor Cyan
    docker stats --no-stream --format "table {{.Container}}\t{{.CPUPerc}}\t{{.MemUsage}}\t{{.NetIO}}"
}

# Main script execution
switch ($Command) {
    'start' { Start-Services }
    'stop' { Stop-Services }
    'restart' { Restart-Services }
    'logs' { Show-Logs }
    'clean' { Clean-Services }
    'reset' { Reset-Services }
    'backup' { Backup-Database }
    'restore' { Restore-Database }
    'status' { Show-Status }
    default { Show-Help }
}
