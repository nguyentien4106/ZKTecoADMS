# PostgreSQL Initial Data Scripts

## Overview
This directory contains SQL scripts to initialize the ZKTeco Attendance Management System database with sample data.

## Files

### 1. `init_data.sql`
Main initialization script that creates:
- **1 Admin User** (username: `admin@zkteco.com`, password: `Admin@123`)
- **3 Devices** (Main Entrance, Office Floor, Warehouse)
- **8 Sample Users** across different departments
- **Attendance Records** (today's and yesterday's logs)
- **System Configurations** (sync intervals, timeouts, etc.)
- **Device Settings** (volume, sleep time)
- **Sync Logs** (historical sync records)

## How to Use

### Prerequisites
- PostgreSQL 12 or higher installed
- Database created with migrations applied
- PostgreSQL client (psql) or any SQL client tool

### Option 1: Using psql Command Line

```bash
# Connect to your database
psql -U your_username -d your_database_name

# Run the script
\i init_data.sql

# Or in one command
psql -U your_username -d your_database_name -f init_data.sql
```

### Option 2: Using pgAdmin
1. Open pgAdmin
2. Connect to your server
3. Select your database
4. Click on Tools → Query Tool
5. Open `init_data.sql`
6. Click Execute (F5)

### Option 3: Using Docker (if using docker-compose)

```bash
# Copy script to container
docker cp init_data.sql <container_name>:/tmp/init_data.sql

# Execute script
docker exec -i <container_name> psql -U postgres -d zkteco_db -f /tmp/init_data.sql
```

## Sample Data Details

### Admin User
- **Username**: admin@zkteco.com
- **Password**: Admin@123
- **Name**: System Administrator
- **Phone**: +1234567890

### Devices
| Device Name | Serial Number | IP Address | Port | Location | Status |
|------------|---------------|------------|------|----------|--------|
| Main Entrance Device | ZK001234567890 | 192.168.1.100 | 4370 | Main Building - Ground Floor | Online |
| Office Floor 2 Device | ZK001234567891 | 192.168.1.101 | 4370 | 2nd Floor Office Area | Online |
| Warehouse Entrance | ZK001234567892 | 192.168.1.102 | 4370 | Warehouse Building | Offline |

### Sample Users
| PIN | Name | Email | Department | Card Number |
|-----|------|-------|------------|-------------|
| 1001 | John Doe | john.doe@company.com | Engineering | CARD001 |
| 1002 | Jane Smith | jane.smith@company.com | Human Resources | CARD002 |
| 1003 | Robert Johnson | robert.johnson@company.com | Management | CARD003 |
| 1004 | Emily Davis | emily.davis@company.com | Marketing | CARD004 |
| 1005 | Michael Brown | michael.brown@company.com | Operations | CARD005 |
| 1006 | Sarah Wilson | sarah.wilson@company.com | Finance | CARD006 |
| 1007 | David Martinez | david.martinez@company.com | IT Support | CARD007 |
| 1008 | Lisa Anderson | lisa.anderson@company.com | Sales | CARD008 |

### Attendance Records
- Today's check-in and check-out records for active users
- Yesterday's attendance history
- Various verify modes and attendance states

### System Configurations
- Sync Interval: 300 seconds (5 minutes)
- Max Retry Attempts: 3
- Session Timeout: 30 minutes
- Push Notifications: Enabled
- Default Timezone: UTC
- Max Devices Per User: 10

## Verification

After running the script, verify the data:

```sql
-- Check all tables
SELECT 'AspNetUsers' as table_name, COUNT(*) as count FROM "AspNetUsers"
UNION ALL
SELECT 'Devices', COUNT(*) FROM "Devices"
UNION ALL
SELECT 'UserDevices', COUNT(*) FROM "UserDevices"
UNION ALL
SELECT 'AttendanceLogs', COUNT(*) FROM "AttendanceLogs"
UNION ALL
SELECT 'SystemConfigurations', COUNT(*) FROM "SystemConfigurations"
UNION ALL
SELECT 'DeviceSettings', COUNT(*) FROM "DeviceSettings"
UNION ALL
SELECT 'SyncLogs', COUNT(*) FROM "SyncLogs";
```

Expected results:
- AspNetUsers: 1
- Devices: 3
- UserDevices: 8
- AttendanceLogs: ~15-20
- SystemConfigurations: 6
- DeviceSettings: 4
- SyncLogs: 3

## Useful Queries

### Today's Attendance Summary
```sql
SELECT 
    d."DeviceName",
    u."FullName",
    a."AttendanceTime",
    CASE a."AttendanceState"
        WHEN 0 THEN 'Check In'
        WHEN 1 THEN 'Check Out'
        ELSE 'Unknown'
    END as "Status"
FROM "AttendanceLogs" a
JOIN "Devices" d ON a."DeviceId" = d."Id"
LEFT JOIN "UserDevices" u ON a."UserId" = u."Id"
WHERE DATE(a."AttendanceTime") = CURRENT_DATE
ORDER BY a."AttendanceTime" DESC;
```

### Device Status Summary
```sql
SELECT 
    "DeviceName",
    "IpAddress",
    "DeviceStatus",
    "LastOnline",
    CASE 
        WHEN "LastOnline" >= NOW() - INTERVAL '5 minutes' THEN 'Active'
        WHEN "LastOnline" >= NOW() - INTERVAL '1 hour' THEN 'Idle'
        ELSE 'Inactive'
    END as "CurrentStatus"
FROM "Devices"
ORDER BY "DeviceStatus", "LastOnline" DESC;
```

### User Attendance Report
```sql
SELECT 
    u."FullName",
    u."Department",
    COUNT(*) as "TotalRecords",
    MIN(a."AttendanceTime") as "FirstCheckIn",
    MAX(a."AttendanceTime") as "LastCheckOut"
FROM "UserDevices" u
LEFT JOIN "AttendanceLogs" a ON u."Id" = a."UserId"
WHERE DATE(a."AttendanceTime") = CURRENT_DATE
GROUP BY u."FullName", u."Department"
ORDER BY u."FullName";
```

## Clean Up

To remove all sample data (keep this for testing only):

```sql
-- WARNING: This will delete all data!
DELETE FROM "AttendanceLogs";
DELETE FROM "DeviceSettings";
DELETE FROM "SyncLogs";
DELETE FROM "FingerprintTemplates";
DELETE FROM "FaceTemplates";
DELETE FROM "UserDevices";
DELETE FROM "Devices";
DELETE FROM "SystemConfigurations";
DELETE FROM "UserRefreshTokens";
DELETE FROM "AspNetUserTokens";
DELETE FROM "AspNetUserLogins";
DELETE FROM "AspNetUserClaims";
DELETE FROM "AspNetUserRoles";
DELETE FROM "AspNetUsers";
```

## Notes

1. **Password Hash**: The admin password is hashed using ASP.NET Core Identity. The hash provided is for demonstration. In production, you should generate a new hash or use the application's registration endpoint.

2. **UUIDs**: All IDs are generated as UUIDs (v4). If you need to reference these IDs in your application code, use the UUIDs provided in the script.

3. **Timestamps**: All timestamps use UTC timezone. Adjust the `SET timezone` command if needed.

4. **Foreign Keys**: The script respects foreign key constraints. Data is inserted in the correct order to avoid constraint violations.

5. **Customization**: Feel free to modify the script to add more sample data or adjust existing records to match your testing needs.

## Troubleshooting

### Error: "duplicate key value violates unique constraint"
- The data already exists. Either drop and recreate the database or modify the UUIDs in the script.

### Error: "relation does not exist"
- Make sure to run Entity Framework migrations first: `dotnet ef database update`

### Error: "permission denied"
- Ensure your PostgreSQL user has INSERT permissions on all tables.

## Support

For issues or questions, please refer to the main project documentation or contact the development team.
