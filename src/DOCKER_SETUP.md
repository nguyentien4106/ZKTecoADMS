# Docker Setup Guide

## Overview
This docker-compose configuration sets up a complete ZKTeco Attendance Management System with:
- **PostgreSQL** database
- **.NET Backend API**
- **React Frontend** (Vite)
- **pgAdmin** (optional database management tool)

## Prerequisites
- Docker Desktop installed
- Docker Compose v3.8 or higher
- At least 4GB of available RAM

## Quick Start

### 1. Build and Start All Services
```bash
cd src
docker-compose up -d --build
```

### 2. View Logs
```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f zkteco_api
docker-compose logs -f zkteco_frontend
docker-compose logs -f zkteco_postgres
```

### 3. Stop Services
```bash
docker-compose down
```

### 4. Stop and Remove Volumes (Clean Reset)
```bash
docker-compose down -v
```

## Services & Ports

| Service | Container Name | Port | URL |
|---------|---------------|------|-----|
| PostgreSQL | zkteco_postgres | 5432 | localhost:5432 |
| Backend API | zkteco_api | 7070 (HTTP), 7071 (HTTPS) | http://localhost:7070 |
| Frontend | zkteco_frontend | 3000 | http://localhost:3000 |
| pgAdmin | zkteco_pgadmin | 5050 | http://localhost:5050 |

## Database Configuration

### PostgreSQL Credentials
- **Host**: postgres (within Docker network) or localhost (from host machine)
- **Port**: 5432
- **Database**: ZKTecoIntegration
- **Username**: postgres
- **Password**: Ti100600@

### Initial Data
The database is automatically initialized with sample data from `init_data.sql` on first run.

To skip initialization, comment out this line in docker-compose.yaml:
```yaml
# - ./init_data.sql:/docker-entrypoint-initdb.d/init_data.sql
```

## pgAdmin Setup

1. Open http://localhost:5050
2. Login with:
   - **Email**: admin@zkteco.com
   - **Password**: admin

3. Add PostgreSQL server:
   - Right-click "Servers" → "Register" → "Server"
   - **General Tab**:
     - Name: ZKTeco DB
   - **Connection Tab**:
     - Host: postgres
     - Port: 5432
     - Database: ZKTecoIntegration
     - Username: postgres
     - Password: Ti100600@

## Environment Variables

### Backend API
Edit in `docker-compose.yaml` under `zktecoadms.api`:
```yaml
environment:
  - ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=ZKTecoIntegration;Username=postgres;Password=Ti100600@;
  - JwtSettings__AccessTokenSecret=your_secret_here
  - JwtSettings__RefreshTokenSecret=your_refresh_secret_here
```

### Frontend
Edit in `docker-compose.yaml` under `zkteco-client`:
```yaml
environment:
  - VITE_API_URL=http://localhost:7070
  - NODE_ENV=production
```

Or create `.env.production` in `zkteco-client/`:
```bash
VITE_API_URL=http://localhost:7070
```

## Development vs Production

### Development Mode (Recommended for Development)
```bash
# Start only database
docker-compose up postgres -d

# Run API locally
cd ZKTecoADMS.Api
dotnet run

# Run frontend locally
cd zkteco-client
npm run dev
```

### Production Mode (All in Docker)
```bash
# Build and start all services
docker-compose up -d --build
```

## Common Commands

### Rebuild a Specific Service
```bash
docker-compose up -d --build zkteco_api
docker-compose up -d --build zkteco_frontend
```

### Execute Commands in Containers
```bash
# Access PostgreSQL CLI
docker exec -it zkteco_postgres psql -U postgres -d ZKTecoIntegration

# Run SQL script
docker exec -i zkteco_postgres psql -U postgres -d ZKTecoIntegration < init_data.sql

# Access API container
docker exec -it zkteco_api bash

# Access frontend container
docker exec -it zkteco_frontend sh
```

### Database Backup & Restore
```bash
# Backup
docker exec zkteco_postgres pg_dump -U postgres ZKTecoIntegration > backup.sql

# Restore
docker exec -i zkteco_postgres psql -U postgres -d ZKTecoIntegration < backup.sql
```

## Troubleshooting

### Database Connection Issues
```bash
# Check if postgres is healthy
docker-compose ps

# Check postgres logs
docker-compose logs postgres

# Verify network connectivity
docker exec -it zkteco_api ping postgres
```

### API Not Starting
```bash
# Check API logs
docker-compose logs zkteco_api

# Verify connection string
docker exec -it zkteco_api env | grep ConnectionStrings

# Restart with fresh build
docker-compose down
docker-compose up -d --build zkteco_api
```

### Frontend Build Failures
```bash
# Check build logs
docker-compose logs zkteco_frontend

# Rebuild with no cache
docker-compose build --no-cache zkteco_frontend
docker-compose up -d zkteco_frontend
```

### Port Already in Use
```bash
# Find and kill process using port
# Windows PowerShell:
Get-Process -Id (Get-NetTCPConnection -LocalPort 3000).OwningProcess | Stop-Process

# Linux/Mac:
lsof -ti:3000 | xargs kill
```

### Clean Everything and Start Fresh
```bash
# Stop all containers and remove volumes
docker-compose down -v

# Remove all images
docker-compose down --rmi all

# Rebuild and start
docker-compose up -d --build
```

## Performance Optimization

### Reduce Build Time
Create `.dockerignore` files to exclude unnecessary files from Docker context.

### Database Performance
```bash
# Monitor database performance
docker exec -it zkteco_postgres psql -U postgres -d ZKTecoIntegration -c "SELECT * FROM pg_stat_activity;"

# Check database size
docker exec -it zkteco_postgres psql -U postgres -d ZKTecoIntegration -c "SELECT pg_size_pretty(pg_database_size('ZKTecoIntegration'));"
```

## Security Considerations

### Production Checklist
- [ ] Change all default passwords
- [ ] Use environment files for secrets (not in docker-compose.yaml)
- [ ] Enable HTTPS for all services
- [ ] Configure firewall rules
- [ ] Use Docker secrets for sensitive data
- [ ] Regular security updates for base images
- [ ] Implement backup strategy

### Using Docker Secrets
```yaml
secrets:
  db_password:
    file: ./secrets/db_password.txt

services:
  postgres:
    secrets:
      - db_password
    environment:
      - POSTGRES_PASSWORD_FILE=/run/secrets/db_password
```

## Monitoring

### Health Checks
All services have health checks configured. View status:
```bash
docker-compose ps
```

### Resource Usage
```bash
docker stats
```

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [PostgreSQL Docker Image](https://hub.docker.com/_/postgres)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet-aspnet)
- [Nginx Docker Image](https://hub.docker.com/_/nginx)

## Support

For issues specific to:
- **Docker**: Check Docker logs and documentation
- **Database**: Check PostgreSQL logs and pgAdmin
- **API**: Check .NET logs and appsettings
- **Frontend**: Check browser console and nginx logs
