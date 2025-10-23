-- ==========================================
-- PostgreSQL Database Initialization Script
-- ZKTeco Attendance Management System
-- Date: October 23, 2025
-- ==========================================
-- This script creates all tables and inserts initial sample data
-- ==========================================

-- Set timezone
SET timezone = 'UTC';

-- Enable required extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- ==========================================
-- 0. CREATE DATABASE SCHEMA (TABLES)
-- ==========================================

-- Drop tables if they exist (for clean reinstall)
-- Uncomment if you want to reset the database
-- DROP TABLE IF EXISTS "DeviceSettings" CASCADE;
-- DROP TABLE IF EXISTS "SyncLogs" CASCADE;
-- DROP TABLE IF EXISTS "DeviceCommands" CASCADE;
-- DROP TABLE IF EXISTS "AttendanceLogs" CASCADE;
-- DROP TABLE IF EXISTS "FaceTemplates" CASCADE;
-- DROP TABLE IF EXISTS "FingerprintTemplates" CASCADE;
-- DROP TABLE IF EXISTS "UserDevices" CASCADE;
-- DROP TABLE IF EXISTS "Devices" CASCADE;
-- DROP TABLE IF EXISTS "SystemConfigurations" CASCADE;
-- DROP TABLE IF EXISTS "UserRefreshTokens" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUserTokens" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUserRoles" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUserLogins" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUserClaims" CASCADE;
-- DROP TABLE IF EXISTS "AspNetRoleClaims" CASCADE;
-- DROP TABLE IF EXISTS "AspNetUsers" CASCADE;
-- DROP TABLE IF EXISTS "AspNetRoles" CASCADE;

-- ==========================================
-- Create ASP.NET Identity Tables
-- ==========================================

-- AspNetRoles Table
CREATE TABLE IF NOT EXISTS "AspNetRoles" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "Name" VARCHAR(256),
    "NormalizedName" VARCHAR(256),
    "ConcurrencyStamp" TEXT
);

-- AspNetUsers Table
CREATE TABLE IF NOT EXISTS "AspNetUsers" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "FirstName" VARCHAR(100),
    "LastName" VARCHAR(100),
    "UserName" VARCHAR(256),
    "NormalizedUserName" VARCHAR(256),
    "Email" VARCHAR(256),
    "NormalizedEmail" VARCHAR(256),
    "EmailConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL DEFAULT FALSE,
    "TwoFactorEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "LockoutEnd" TIMESTAMPTZ,
    "LockoutEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "AccessFailedCount" INTEGER NOT NULL DEFAULT 0,
    "Created" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256)
);

-- AspNetUserRoles Table
CREATE TABLE IF NOT EXISTS "AspNetUserRoles" (
    "UserId" UUID NOT NULL,
    "RoleId" UUID NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_Users" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_Roles" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- AspNetUserClaims Table
CREATE TABLE IF NOT EXISTS "AspNetUserClaims" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" UUID NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    CONSTRAINT "FK_AspNetUserClaims_Users" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetUserLogins Table
CREATE TABLE IF NOT EXISTS "AspNetUserLogins" (
    "LoginProvider" VARCHAR(128) NOT NULL,
    "ProviderKey" VARCHAR(128) NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" UUID NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_Users" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetUserTokens Table
CREATE TABLE IF NOT EXISTS "AspNetUserTokens" (
    "UserId" UUID NOT NULL,
    "LoginProvider" VARCHAR(128) NOT NULL,
    "Name" VARCHAR(128) NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_Users" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetRoleClaims Table
CREATE TABLE IF NOT EXISTS "AspNetRoleClaims" (
    "Id" SERIAL PRIMARY KEY,
    "RoleId" UUID NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    CONSTRAINT "FK_AspNetRoleClaims_Roles" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- ==========================================
-- Create Application Tables
-- ==========================================

-- UserRefreshTokens Table
CREATE TABLE IF NOT EXISTS "UserRefreshTokens" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "Token" TEXT NOT NULL,
    "ExpiresAt" TIMESTAMPTZ NOT NULL,
    "IsRevoked" BOOLEAN NOT NULL DEFAULT FALSE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_UserRefreshTokens_Users" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- Devices Table
CREATE TABLE IF NOT EXISTS "Devices" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "SerialNumber" VARCHAR(50) NOT NULL,
    "DeviceName" VARCHAR(100) NOT NULL,
    "Model" VARCHAR(50),
    "IpAddress" VARCHAR(50),
    "Port" INTEGER,
    "Location" VARCHAR(200),
    "Timezone" VARCHAR(50) NOT NULL DEFAULT 'UTC',
    "LastOnline" TIMESTAMPTZ,
    "FirmwareVersion" VARCHAR(50),
    "Platform" VARCHAR(50),
    "DeviceStatus" VARCHAR(20) NOT NULL DEFAULT 'Offline',
    "MaxUsers" INTEGER,
    "MaxFingerprints" INTEGER,
    "MaxFaces" INTEGER,
    "SupportsPushSDK" BOOLEAN NOT NULL DEFAULT TRUE,
    "ApplicationUserId" UUID NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    "LastModified" TIMESTAMPTZ,
    "LastModifiedBy" VARCHAR(256),
    "Deleted" TIMESTAMPTZ,
    "DeletedBy" VARCHAR(256),
    CONSTRAINT "FK_Devices_Users" FOREIGN KEY ("ApplicationUserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- UserDevices Table (Device Users)
CREATE TABLE IF NOT EXISTS "UserDevices" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "PIN" VARCHAR(20) NOT NULL,
    "FullName" VARCHAR(200) NOT NULL,
    "CardNumber" VARCHAR(50),
    "Password" VARCHAR(50),
    "GroupId" INTEGER NOT NULL DEFAULT 1,
    "Privilege" INTEGER NOT NULL DEFAULT 0,
    "VerifyMode" INTEGER NOT NULL DEFAULT 0,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "Email" VARCHAR(100),
    "PhoneNumber" VARCHAR(20),
    "Department" VARCHAR(100),
    "DeviceId" UUID NOT NULL,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_UserDevices_Devices" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE
);

-- AttendanceLogs Table
CREATE TABLE IF NOT EXISTS "AttendanceLogs" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "DeviceId" UUID NOT NULL,
    "UserId" UUID,
    "PIN" VARCHAR(20) NOT NULL,
    "VerifyMode" INTEGER NOT NULL,
    "AttendanceState" INTEGER NOT NULL,
    "AttendanceTime" TIMESTAMPTZ NOT NULL,
    "WorkCode" VARCHAR(10),
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_AttendanceLogs_Devices" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AttendanceLogs_Users" FOREIGN KEY ("UserId") REFERENCES "UserDevices" ("Id") ON DELETE SET NULL
);

-- FingerprintTemplates Table
CREATE TABLE IF NOT EXISTS "FingerprintTemplates" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "FingerIndex" INTEGER NOT NULL,
    "Template" TEXT NOT NULL,
    "TemplateVersion" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_FingerprintTemplates_Users" FOREIGN KEY ("UserId") REFERENCES "UserDevices" ("Id") ON DELETE CASCADE
);

-- FaceTemplates Table
CREATE TABLE IF NOT EXISTS "FaceTemplates" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "UserId" UUID NOT NULL,
    "FaceIndex" INTEGER NOT NULL,
    "Template" TEXT NOT NULL,
    "TemplateVersion" INTEGER NOT NULL DEFAULT 0,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_FaceTemplates_Users" FOREIGN KEY ("UserId") REFERENCES "UserDevices" ("Id") ON DELETE CASCADE
);

-- DeviceCommands Table
CREATE TABLE IF NOT EXISTS "DeviceCommands" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "DeviceId" UUID NOT NULL,
    "CommandType" VARCHAR(50) NOT NULL,
    "CommandData" TEXT,
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Pending',
    "Response" TEXT,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    "ExecutedAt" TIMESTAMPTZ,
    CONSTRAINT "FK_DeviceCommands_Devices" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE
);

-- SyncLogs Table
CREATE TABLE IF NOT EXISTS "SyncLogs" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "DeviceId" UUID NOT NULL,
    "SyncType" VARCHAR(50) NOT NULL,
    "SyncStatus" VARCHAR(50) NOT NULL,
    "RecordsProcessed" INTEGER,
    "ErrorMessage" VARCHAR(500),
    "StartTime" TIMESTAMPTZ NOT NULL,
    "EndTime" TIMESTAMPTZ,
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_SyncLogs_Devices" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE
);

-- DeviceSettings Table
CREATE TABLE IF NOT EXISTS "DeviceSettings" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "DeviceId" UUID NOT NULL,
    "SettingKey" VARCHAR(100) NOT NULL,
    "SettingValue" VARCHAR(500),
    "Description" VARCHAR(200),
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256),
    CONSTRAINT "FK_DeviceSettings_Devices" FOREIGN KEY ("DeviceId") REFERENCES "Devices" ("Id") ON DELETE CASCADE
);

-- SystemConfigurations Table
CREATE TABLE IF NOT EXISTS "SystemConfigurations" (
    "Id" UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    "ConfigKey" VARCHAR(100) NOT NULL UNIQUE,
    "ConfigValue" VARCHAR(500),
    "Description" VARCHAR(200),
    "CreatedAt" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "CreatedBy" VARCHAR(256)
);

-- ==========================================
-- Create Indexes for Performance
-- ==========================================

CREATE INDEX IF NOT EXISTS "IX_AspNetUsers_Email" ON "AspNetUsers" ("NormalizedEmail");
CREATE INDEX IF NOT EXISTS "IX_AspNetUsers_UserName" ON "AspNetUsers" ("NormalizedUserName");
CREATE INDEX IF NOT EXISTS "IX_Devices_SerialNumber" ON "Devices" ("SerialNumber");
CREATE INDEX IF NOT EXISTS "IX_Devices_ApplicationUserId" ON "Devices" ("ApplicationUserId");
CREATE INDEX IF NOT EXISTS "IX_UserDevices_PIN" ON "UserDevices" ("PIN");
CREATE INDEX IF NOT EXISTS "IX_UserDevices_DeviceId" ON "UserDevices" ("DeviceId");
CREATE INDEX IF NOT EXISTS "IX_AttendanceLogs_DeviceId" ON "AttendanceLogs" ("DeviceId");
CREATE INDEX IF NOT EXISTS "IX_AttendanceLogs_UserId" ON "AttendanceLogs" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_AttendanceLogs_AttendanceTime" ON "AttendanceLogs" ("AttendanceTime");
CREATE INDEX IF NOT EXISTS "IX_AttendanceLogs_PIN" ON "AttendanceLogs" ("PIN");
CREATE INDEX IF NOT EXISTS "IX_DeviceSettings_DeviceId" ON "DeviceSettings" ("DeviceId");
CREATE INDEX IF NOT EXISTS "IX_SyncLogs_DeviceId" ON "SyncLogs" ("DeviceId");
CREATE INDEX IF NOT EXISTS "IX_SyncLogs_StartTime" ON "SyncLogs" ("StartTime");


-- ==========================================
-- 1. CREATE ADMIN USER
-- ==========================================
-- Password: Admin@123 (hashed with ASP.NET Core Identity)
INSERT INTO "AspNetUsers" (
    "Id", 
    "FirstName", 
    "LastName", 
    "UserName", 
    "NormalizedUserName", 
    "Email", 
    "NormalizedEmail", 
    "EmailConfirmed", 
    "PasswordHash", 
    "SecurityStamp", 
    "ConcurrencyStamp",
    "PhoneNumber",
    "PhoneNumberConfirmed",
    "TwoFactorEnabled",
    "LockoutEnabled",
    "AccessFailedCount",
    "Created",
    "CreatedBy"
) VALUES (
    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d'::uuid,
    'System',
    'Administrator',
    'admin@zkteco.com',
    'ADMIN@ZKTECO.COM',
    'admin@zkteco.com',
    'ADMIN@ZKTECO.COM',
    true,
    'AANGrxMpCl7Flmh8nbd/iCyHgrzDCnyYiCz7PoOwo4Slt+fMA8zGWM50v7zpJpuycJQ==', -- Admin@123
    'WYXQJZ4VQJZQX4WVJQX4WVJQX4WV',
    'c1d2e3f4-a5b6-4c7d-8e9f-0a1b2c3d4e5f',
    '+1234567890',
    false,
    false,
    false,
    0,
    NOW(),
    'System'
);

-- ==========================================
-- 2. CREATE SAMPLE DEVICES
-- ==========================================
INSERT INTO "Devices" (
    "Id",
    "SerialNumber",
    "DeviceName",
    "Model",
    "IpAddress",
    "Port",
    "Location",
    "Timezone",
    "LastOnline",
    "FirmwareVersion",
    "Platform",
    "DeviceStatus",
    "MaxUsers",
    "MaxFingerprints",
    "MaxFaces",
    "SupportsPushSDK",
    "ApplicationUserId",
    "CreatedAt",
    "CreatedBy",
    "IsActive"
) VALUES 
-- Device 1: Main Entrance
(
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    'ZK001234567890',
    'Main Entrance Device',
    'ZKTeco F18',
    '192.168.1.100',
    4370,
    'Main Building - Ground Floor Entrance',
    'UTC',
    NOW() - INTERVAL '5 minutes',
    'Ver 6.60',
    'ZEM600',
    'Online',
    3000,
    5000,
    3000,
    true,
    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d'::uuid,
    NOW(),
    'admin@zkteco.com',
    true
),
-- Device 2: Office Floor
(
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    'ZK001234567891',
    'Office Floor 2 Device',
    'ZKTeco K40',
    '192.168.1.101',
    4370,
    'Main Building - 2nd Floor Office Area',
    'UTC',
    NOW() - INTERVAL '10 minutes',
    'Ver 6.60',
    'ZEM800',
    'Online',
    5000,
    10000,
    5000,
    true,
    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d'::uuid,
    NOW(),
    'admin@zkteco.com',
    true
),
-- Device 3: Warehouse
(
    'd3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a'::uuid,
    'ZK001234567892',
    'Warehouse Entrance',
    'ZKTeco MA300',
    '192.168.1.102',
    4370,
    'Warehouse Building - Main Gate',
    'UTC',
    NOW() - INTERVAL '1 hour',
    'Ver 6.50',
    'ZEM600',
    'Offline',
    2000,
    4000,
    2000,
    true,
    'a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d'::uuid,
    NOW(),
    'admin@zkteco.com',
    true
);

-- ==========================================
-- 3. CREATE SAMPLE USERS
-- ==========================================
INSERT INTO "UserDevices" (
    "Id",
    "PIN",
    "FullName",
    "CardNumber",
    "GroupId",
    "Privilege",
    "VerifyMode",
    "IsActive",
    "Email",
    "PhoneNumber",
    "Department",
    "DeviceId",
    "CreatedAt",
    "CreatedBy"
) VALUES
-- User 1: John Doe
(
    'e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b'::uuid,
    '1001',
    'John Doe',
    'CARD001',
    1,
    0,
    1,
    true,
    'john.doe@company.com',
    '+1234567891',
    'Engineering',
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 2: Jane Smith
(
    'f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c'::uuid,
    '1002',
    'Jane Smith',
    'CARD002',
    1,
    0,
    1,
    true,
    'jane.smith@company.com',
    '+1234567892',
    'Human Resources',
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 3: Robert Johnson
(
    'a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d'::uuid,
    '1003',
    'Robert Johnson',
    'CARD003',
    1,
    2,
    1,
    true,
    'robert.johnson@company.com',
    '+1234567893',
    'Management',
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 4: Emily Davis
(
    'b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e'::uuid,
    '1004',
    'Emily Davis',
    'CARD004',
    1,
    0,
    1,
    true,
    'emily.davis@company.com',
    '+1234567894',
    'Marketing',
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 5: Michael Brown
(
    'c8d9e0f1-a2b3-4c4d-5e6f-7a8b9c0d1e2f'::uuid,
    '1005',
    'Michael Brown',
    'CARD005',
    1,
    0,
    1,
    true,
    'michael.brown@company.com',
    '+1234567895',
    'Operations',
    'd3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 6: Sarah Wilson
(
    'd9e0f1a2-b3c4-4d5e-6f7a-8b9c0d1e2f3a'::uuid,
    '1006',
    'Sarah Wilson',
    'CARD006',
    1,
    0,
    1,
    true,
    'sarah.wilson@company.com',
    '+1234567896',
    'Finance',
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 7: David Martinez
(
    'e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b'::uuid,
    '1007',
    'David Martinez',
    'CARD007',
    1,
    0,
    1,
    true,
    'david.martinez@company.com',
    '+1234567897',
    'IT Support',
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    NOW(),
    'admin@zkteco.com'
),
-- User 8: Lisa Anderson
(
    'f1a2b3c4-d5e6-4f7a-8b9c-0d1e2f3a4b5c'::uuid,
    '1008',
    'Lisa Anderson',
    'CARD008',
    1,
    0,
    1,
    false,
    'lisa.anderson@company.com',
    '+1234567898',
    'Sales',
    'd3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a'::uuid,
    NOW(),
    'admin@zkteco.com'
);

-- ==========================================
-- 4. CREATE SAMPLE ATTENDANCE LOGS
-- ==========================================
INSERT INTO "AttendanceLogs" (
    "Id",
    "DeviceId",
    "UserId",
    "PIN",
    "VerifyMode",
    "AttendanceState",
    "AttendanceTime",
    "WorkCode",
    "CreatedAt",
    "CreatedBy"
) VALUES
-- Today's attendance
('a1a1a1a1-b1b1-4c1c-8d1d-1e1e1e1e1e1e'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b'::uuid, '1001', 1, 0, NOW() - INTERVAL '8 hours', NULL, NOW() - INTERVAL '8 hours', 'System'),
('a2a2a2a2-b2b2-4c2c-8d2d-2e2e2e2e2e2e'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c'::uuid, '1002', 1, 0, NOW() - INTERVAL '7 hours 45 minutes', NULL, NOW() - INTERVAL '7 hours 45 minutes', 'System'),
('a3a3a3a3-b3b3-4c3c-8d3d-3e3e3e3e3e3e'::uuid, 'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid, 'a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d'::uuid, '1003', 1, 0, NOW() - INTERVAL '7 hours 30 minutes', NULL, NOW() - INTERVAL '7 hours 30 minutes', 'System'),
('a4a4a4a4-b4b4-4c4c-8d4d-4e4e4e4e4e4e'::uuid, 'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid, 'b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e'::uuid, '1004', 1, 0, NOW() - INTERVAL '8 hours 15 minutes', NULL, NOW() - INTERVAL '8 hours 15 minutes', 'System'),
('a5a5a5a5-b5b5-4c5c-8d5d-5e5e5e5e5e5e'::uuid, 'd3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a'::uuid, 'c8d9e0f1-a2b3-4c4d-5e6f-7a8b9c0d1e2f'::uuid, '1005', 1, 0, NOW() - INTERVAL '8 hours 5 minutes', NULL, NOW() - INTERVAL '8 hours 5 minutes', 'System'),
('a6a6a6a6-b6b6-4c6c-8d6d-6e6e6e6e6e6e'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'd9e0f1a2-b3c4-4d5e-6f7a-8b9c0d1e2f3a'::uuid, '1006', 1, 0, NOW() - INTERVAL '7 hours 50 minutes', NULL, NOW() - INTERVAL '7 hours 50 minutes', 'System'),
('a7a7a7a7-b7b7-4c7c-8d7d-7e7e7e7e7e7e'::uuid, 'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid, 'e0f1a2b3-c4d5-4e6f-7a8b-9c0d1e2f3a4b'::uuid, '1007', 1, 0, NOW() - INTERVAL '8 hours 10 minutes', NULL, NOW() - INTERVAL '8 hours 10 minutes', 'System'),

-- Checkout records
('a8a8a8a8-b8b8-4c8c-8d8d-8e8e8e8e8e8e'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b'::uuid, '1001', 1, 1, NOW() - INTERVAL '30 minutes', NULL, NOW() - INTERVAL '30 minutes', 'System'),
('a9a9a9a9-b9b9-4c9c-8d9d-9e9e9e9e9e9e'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c'::uuid, '1002', 1, 1, NOW() - INTERVAL '45 minutes', NULL, NOW() - INTERVAL '45 minutes', 'System'),

-- Yesterday's attendance
('b1b1b1b1-c1c1-4d1d-8e1e-1f1f1f1f1f1f'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'e4f5a6b7-c8d9-4e0f-1a2b-3c4d5e6f7a8b'::uuid, '1001', 1, 0, NOW() - INTERVAL '1 day 8 hours', NULL, NOW() - INTERVAL '1 day 8 hours', 'System'),
('b2b2b2b2-c2c2-4d2d-8e2e-2f2f2f2f2f2f'::uuid, 'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid, 'f5a6b7c8-d9e0-4f1a-2b3c-4d5e6f7a8b9c'::uuid, '1002', 1, 0, NOW() - INTERVAL '1 day 7 hours 55 minutes', NULL, NOW() - INTERVAL '1 day 7 hours 55 minutes', 'System'),
('b3b3b3b3-c3c3-4d3d-8e3e-3f3f3f3f3f3f'::uuid, 'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid, 'a6b7c8d9-e0f1-4a2b-3c4d-5e6f7a8b9c0d'::uuid, '1003', 1, 0, NOW() - INTERVAL '1 day 7 hours 40 minutes', NULL, NOW() - INTERVAL '1 day 7 hours 40 minutes', 'System'),
('b4b4b4b4-c4c4-4d4d-8e4e-4f4f4f4f4f4f'::uuid, 'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid, 'b7c8d9e0-f1a2-4b3c-4d5e-6f7a8b9c0d1e'::uuid, '1004', 1, 0, NOW() - INTERVAL '1 day 8 hours 20 minutes', NULL, NOW() - INTERVAL '1 day 8 hours 20 minutes', 'System');

-- ==========================================
-- 5. CREATE SYSTEM CONFIGURATIONS
-- ==========================================
INSERT INTO "SystemConfigurations" (
    "Id",
    "ConfigKey",
    "ConfigValue",
    "Description",
    "CreatedAt",
    "CreatedBy"
) VALUES
(
    '11111111-2222-3333-4444-555555555551'::uuid,
    'SyncInterval',
    '300',
    'Attendance sync interval in seconds (5 minutes)',
    NOW(),
    'System'
),
(
    '11111111-2222-3333-4444-555555555552'::uuid,
    'MaxRetryAttempts',
    '3',
    'Maximum retry attempts for failed device connections',
    NOW(),
    'System'
),
(
    '11111111-2222-3333-4444-555555555553'::uuid,
    'SessionTimeout',
    '30',
    'User session timeout in minutes',
    NOW(),
    'System'
),
(
    '11111111-2222-3333-4444-555555555554'::uuid,
    'EnablePushNotifications',
    'true',
    'Enable push notifications for attendance events',
    NOW(),
    'System'
),
(
    '11111111-2222-3333-4444-555555555555'::uuid,
    'DefaultTimezone',
    'UTC',
    'Default system timezone',
    NOW(),
    'System'
),
(
    '11111111-2222-3333-4444-555555555556'::uuid,
    'MaxDevicesPerUser',
    '10',
    'Maximum number of devices per user account',
    NOW(),
    'System'
);

-- ==========================================
-- 6. CREATE DEVICE SETTINGS
-- ==========================================
INSERT INTO "DeviceSettings" (
    "Id",
    "DeviceId",
    "SettingKey",
    "SettingValue",
    "Description",
    "CreatedAt"
) VALUES
-- Main Entrance Device Settings
(
    '22222222-3333-4444-5555-666666666661'::uuid,
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    'SleepTime',
    '5',
    'Device sleep time in minutes',
    NOW()
),
(
    '22222222-3333-4444-5555-666666666662'::uuid,
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    'Volume',
    '50',
    'Device volume level (0-100)',
    NOW()
),
-- Office Floor Device Settings
(
    '22222222-3333-4444-5555-666666666663'::uuid,
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    'SleepTime',
    '10',
    'Device sleep time in minutes',
    NOW()
),
(
    '22222222-3333-4444-5555-666666666664'::uuid,
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    'Volume',
    '60',
    'Device volume level (0-100)',
    NOW()
);

-- ==========================================
-- 7. CREATE SYNC LOGS
-- ==========================================
INSERT INTO "SyncLogs" (
    "Id",
    "DeviceId",
    "SyncType",
    "SyncStatus",
    "RecordsProcessed",
    "ErrorMessage",
    "StartTime",
    "EndTime"
) VALUES
(
    '33333333-4444-5555-6666-777777777771'::uuid,
    'b1c2d3e4-f5a6-4b7c-8d9e-0f1a2b3c4d5e'::uuid,
    'Attendance',
    'Success',
    45,
    NULL,
    NOW() - INTERVAL '1 hour',
    NOW() - INTERVAL '59 minutes'
),
(
    '33333333-4444-5555-6666-777777777772'::uuid,
    'c2d3e4f5-a6b7-4c8d-9e0f-1a2b3c4d5e6f'::uuid,
    'Attendance',
    'Success',
    32,
    NULL,
    NOW() - INTERVAL '2 hours',
    NOW() - INTERVAL '1 hour 58 minutes'
),
(
    '33333333-4444-5555-6666-777777777773'::uuid,
    'd3e4f5a6-b7c8-4d9e-0f1a-2b3c4d5e6f7a'::uuid,
    'Attendance',
    'Failed',
    0,
    'Connection timeout - device offline',
    NOW() - INTERVAL '3 hours',
    NOW() - INTERVAL '2 hours 59 minutes'
);

-- ==========================================
-- 8. VERIFICATION QUERIES
-- ==========================================
-- Uncomment to verify data insertion

-- SELECT COUNT(*) as "Total Users" FROM "AspNetUsers";
-- SELECT COUNT(*) as "Total Devices" FROM "Devices";
-- SELECT COUNT(*) as "Total Device Users" FROM "UserDevices";
-- SELECT COUNT(*) as "Total Attendance Records" FROM "AttendanceLogs";
-- SELECT COUNT(*) as "Total Configurations" FROM "SystemConfigurations";
-- SELECT COUNT(*) as "Total Device Settings" FROM "DeviceSettings";
-- SELECT COUNT(*) as "Total Sync Logs" FROM "SyncLogs";

-- ==========================================
-- 9. USEFUL QUERIES
-- ==========================================

-- Get today's attendance summary
-- SELECT 
--     d."DeviceName",
--     u."FullName",
--     a."AttendanceTime",
--     CASE a."AttendanceState"
--         WHEN 0 THEN 'Check In'
--         WHEN 1 THEN 'Check Out'
--         ELSE 'Unknown'
--     END as "Status"
-- FROM "AttendanceLogs" a
-- JOIN "Devices" d ON a."DeviceId" = d."Id"
-- LEFT JOIN "UserDevices" u ON a."UserId" = u."Id"
-- WHERE DATE(a."AttendanceTime") = CURRENT_DATE
-- ORDER BY a."AttendanceTime" DESC;

-- Get device status summary
-- SELECT 
--     "DeviceName",
--     "IpAddress",
--     "DeviceStatus",
--     "LastOnline",
--     CASE 
--         WHEN "LastOnline" >= NOW() - INTERVAL '5 minutes' THEN 'Active'
--         WHEN "LastOnline" >= NOW() - INTERVAL '1 hour' THEN 'Idle'
--         ELSE 'Inactive'
--     END as "CurrentStatus"
-- FROM "Devices"
-- ORDER BY "DeviceStatus", "LastOnline" DESC;

-- ==========================================
-- END OF SCRIPT
-- ==========================================

COMMIT;
