# SQL Scripts Quick Reference Guide

## Available Scripts

### 1. `init_data.sql` - Initial Data Setup
**Purpose**: Creates minimal initial data to get started
**Contains**:
- 1 Admin user
- 3 Devices
- 8 Sample users
- Sample attendance records (today + yesterday)
- System configurations
- Device settings

**When to use**: First time setup, fresh database

```bash
psql -U postgres -d zkteco_db -f init_data.sql
```

---

### 2. `generate_test_data.sql` - Extended Test Data
**Purpose**: Generates larger dataset for testing and performance analysis
**Generates**:
- 10+ additional devices
- 50+ additional users
- 30 days of attendance history
- Device commands log
- Extended sync logs
- Reporting views

**When to use**: Testing, performance testing, demo environments

```bash
psql -U postgres -d zkteco_db -f generate_test_data.sql
```

**Note**: Run `init_data.sql` first before running this script

---

### 3. `reset_database.sql` - Clean Database
**Purpose**: Removes all data from the database
**⚠️ WARNING**: Destructive operation - use only in development

**When to use**: 
- Reset to clean state
- Before re-running initialization scripts
- Cleanup after testing

```bash
psql -U postgres -d zkteco_db -f reset_database.sql
```

---

## Common Workflows

### Full Setup (Development)
```bash
# 1. Apply migrations
cd ZKTecoADMS.Api
dotnet ef database update

# 2. Initialize with sample data
psql -U postgres -d zkteco_db -f ../init_data.sql
```

### Full Setup (Testing with Large Dataset)
```bash
# 1. Apply migrations
dotnet ef database update

# 2. Initialize with sample data
psql -U postgres -d zkteco_db -f init_data.sql

# 3. Generate extended test data
psql -U postgres -d zkteco_db -f generate_test_data.sql
```

### Reset and Reload
```bash
# 1. Clean database
psql -U postgres -d zkteco_db -f reset_database.sql

# 2. Reload initial data
psql -U postgres -d zkteco_db -f init_data.sql
```

---

## Docker Commands

### If using Docker Compose

```bash
# Copy scripts to container
docker cp init_data.sql zkteco_postgres:/tmp/
docker cp generate_test_data.sql zkteco_postgres:/tmp/
docker cp reset_database.sql zkteco_postgres:/tmp/

# Execute scripts
docker exec -i zkteco_postgres psql -U postgres -d zkteco_db -f /tmp/init_data.sql
docker exec -i zkteco_postgres psql -U postgres -d zkteco_db -f /tmp/generate_test_data.sql

# Or pipe directly
docker exec -i zkteco_postgres psql -U postgres -d zkteco_db < init_data.sql
```

---

## Useful Queries

### Check Data Status
```sql
SELECT 
    'Devices' as table_name, COUNT(*) as count FROM "Devices"
UNION ALL
SELECT 'Users', COUNT(*) FROM "UserDevices"
UNION ALL
SELECT 'Attendance', COUNT(*) FROM "AttendanceLogs"
UNION ALL
SELECT 'Online Devices', COUNT(*) FROM "Devices" WHERE "DeviceStatus" = 'Online';
```

### Today's Activity
```sql
SELECT 
    d."DeviceName",
    COUNT(DISTINCT a."UserId") as "Unique Users",
    COUNT(*) as "Total Records"
FROM "AttendanceLogs" a
JOIN "Devices" d ON a."DeviceId" = d."Id"
WHERE DATE(a."AttendanceTime") = CURRENT_DATE
GROUP BY d."DeviceName";
```

### Database Size
```sql
SELECT 
    pg_size_pretty(pg_database_size(current_database())) as "Database Size",
    pg_size_pretty(pg_total_relation_size('"AttendanceLogs"')) as "Attendance Table Size",
    pg_size_pretty(pg_total_relation_size('"UserDevices"')) as "Users Table Size";
```

---

## Connection Strings

### Development
```
Host=localhost;Port=5432;Database=zkteco_db;Username=postgres;Password=your_password
```

### Docker Compose
```
Host=postgres;Port=5432;Database=zkteco_db;Username=postgres;Password=postgres
```

---

## Troubleshooting

### Problem: "role does not exist"
```bash
# Create the user
docker exec -it zkteco_postgres createuser -U postgres your_username
```

### Problem: "database does not exist"
```bash
# Create the database
docker exec -it zkteco_postgres createdb -U postgres zkteco_db
```

### Problem: "permission denied"
```sql
-- Grant permissions
GRANT ALL PRIVILEGES ON DATABASE zkteco_db TO your_username;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO your_username;
```

### Problem: Script hangs or takes too long
- Check PostgreSQL logs: `docker logs zkteco_postgres`
- Monitor progress: Add `-e` flag to psql commands
- Reduce data volume in `generate_test_data.sql`

---

## Best Practices

1. **Always backup** before running reset script
2. **Test scripts** in development environment first
3. **Use transactions** - scripts use BEGIN/COMMIT
4. **Monitor performance** when generating large datasets
5. **Document customizations** if you modify the scripts

---

## Views Created by Scripts

After running `generate_test_data.sql`, these views are available:

### `vw_DailyAttendanceSummary`
Daily summary of attendance by device and location

### `vw_DeviceHealth`
Device health status and activity metrics

### `vw_UserAttendanceReport`
User attendance statistics and history

**Usage**:
```sql
SELECT * FROM "vw_DailyAttendanceSummary" WHERE "Date" = CURRENT_DATE;
SELECT * FROM "vw_DeviceHealth" ORDER BY "HoursOffline" DESC;
SELECT * FROM "vw_UserAttendanceReport" ORDER BY "DaysPresent" DESC;
```

---

## Support

For issues or questions:
1. Check PostgreSQL logs
2. Verify connection settings
3. Ensure migrations are applied
4. Review `DATABASE_INIT_README.md` for detailed information
