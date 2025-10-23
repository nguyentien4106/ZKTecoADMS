# Docker Configuration Summary

## What Was Created

### 1. **Updated docker-compose.yaml**
Complete orchestration file with 4 services:
- PostgreSQL database (port 5432)
- .NET Backend API (ports 7070, 7071)
- React Frontend (port 3000)
- pgAdmin (port 5050)

### 2. **Frontend Docker Files**
- `zkteco-client/Dockerfile` - Multi-stage build for React app
- `zkteco-client/nginx.conf` - Nginx configuration with SPA routing
- `zkteco-client/.dockerignore` - Exclude unnecessary files

### 3. **Management Tools**
- `docker-manage.ps1` - PowerShell script for easy Docker management
- `DOCKER_SETUP.md` - Comprehensive documentation

### 4. **Database Scripts**
- `init_data.sql` - Initial database population script
- `generate_test_data.sql` - Extended test data generation
- `reset_database.sql` - Clean database script

## Quick Start Guide

### Option 1: Using PowerShell Script (Recommended)
```powershell
# Start all services
.\docker-manage.ps1 start

# View logs
.\docker-manage.ps1 logs

# Check status
.\docker-manage.ps1 status

# Stop services
.\docker-manage.ps1 stop
```

### Option 2: Using Docker Compose Directly
```bash
# Start all services
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

## Access URLs

After starting the services:

| Service | URL | Credentials |
|---------|-----|-------------|
| **Frontend** | http://localhost:3000 | admin@zkteco.com / Admin@123 |
| **API** | http://localhost:7070 | (JWT based) |
| **pgAdmin** | http://localhost:5050 | admin@zkteco.com / admin |
| **PostgreSQL** | localhost:5432 | postgres / Ti100600@ |

## Service Startup Order

Docker Compose automatically handles dependencies:
1. PostgreSQL starts first
2. Backend API waits for PostgreSQL health check
3. Frontend waits for Backend API
4. pgAdmin starts after PostgreSQL

## Database Initialization

On first run, the database is automatically initialized with:
- 1 admin user
- 3 devices
- 8 sample users
- Sample attendance records
- System configurations

## Development Workflow

### For Development (Recommended)
```bash
# Start only database with pgAdmin
docker-compose up postgres pgadmin -d

# Run API locally
cd ZKTecoADMS.Api
dotnet run

# Run frontend locally
cd zkteco-client
npm run dev
```

### For Production Testing
```bash
# Start all services
docker-compose up -d --build

# Access at http://localhost:3000
```

## Troubleshooting

### If transaction error occurs:
```bash
# Connect to postgres
docker exec -it zkteco_postgres psql -U postgres -d ZKTecoIntegration

# Rollback any pending transactions
ROLLBACK;

# Or reset the database
docker-compose down -v
docker-compose up -d
```

### If frontend build fails:
```bash
# Check Node.js version in container
docker run --rm node:20-alpine node --version

# Rebuild with no cache
docker-compose build --no-cache zkteco_frontend
```

### If API can't connect to database:
```bash
# Check postgres health
docker-compose ps postgres

# Check connection from API container
docker exec -it zkteco_api ping postgres

# View API logs
docker-compose logs zkteco_api
```

## Important Files

```
src/
├── docker-compose.yaml          # Main orchestration file
├── docker-manage.ps1            # Management script
├── DOCKER_SETUP.md              # Detailed documentation
├── init_data.sql                # Initial data
├── generate_test_data.sql       # Extended test data
├── reset_database.sql           # Database reset
├── zkteco-client/
│   ├── Dockerfile               # Frontend build
│   ├── nginx.conf               # Web server config
│   └── .dockerignore            # Exclude files
└── ZKTecoADMS.Api/
    └── Dockerfile               # Backend build (if needed)
```

## Environment Variables

### Backend (in docker-compose.yaml)
```yaml
ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=ZKTecoIntegration;Username=postgres;Password=Ti100600@;
JwtSettings__AccessTokenSecret=...
JwtSettings__RefreshTokenSecret=...
```

### Frontend (in docker-compose.yaml)
```yaml
VITE_API_URL=http://localhost:7070
NODE_ENV=production
```

## Volume Management

### Persistent Data
- `postgres_data` - Database files
- `pgadmin_data` - pgAdmin configuration

### Backup/Restore
```bash
# Backup
.\docker-manage.ps1 backup

# Restore
.\docker-manage.ps1 restore
```

## Next Steps

1. **Test the setup:**
   ```bash
   .\docker-manage.ps1 start
   ```

2. **Access the application:**
   - Open http://localhost:3000
   - Login with admin@zkteco.com / Admin@123

3. **Check pgAdmin:**
   - Open http://localhost:5050
   - Add server connection to postgres database

4. **Monitor logs:**
   ```bash
   .\docker-manage.ps1 logs
   ```

5. **For production deployment:**
   - Change all default passwords
   - Configure HTTPS
   - Set up proper secrets management
   - Configure firewall rules
   - Set up monitoring and alerts

## Support

- Check `DOCKER_SETUP.md` for detailed documentation
- Check `SQL_SCRIPTS_GUIDE.md` for database scripts
- Check `DATABASE_INIT_README.md` for data initialization
- View logs: `.\docker-manage.ps1 logs`
- Check status: `.\docker-manage.ps1 status`

## Clean Up

```bash
# Stop services (keep data)
.\docker-manage.ps1 stop

# Remove containers (keep data)
.\docker-manage.ps1 clean

# Remove everything including data
.\docker-manage.ps1 reset
```
