using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Infrastructure;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Services;


public class DeviceService(
    IDeviceRepository deviceRepository,
    ILogger<DeviceService> logger,
    ZKTecoDbContext context)
    : IDeviceService
{
    public async Task<Device> GetOrCreateDeviceAsync(string serialNumber)
    {
        var device = await deviceRepository.GetBySerialNumberAsync(serialNumber);
        
        if (device == null)
        {
            logger.LogInformation("Creating new device with SN: {SerialNumber}", serialNumber);
            device = new Device
            {
                SerialNumber = serialNumber,
                DeviceName = $"Device-{serialNumber}",
                IsActive = true,
                DeviceStatus = "Online"
            };
            await deviceRepository.AddAsync(device);
        }

        return device;
    }

    public async Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber)
    {
        return await deviceRepository.GetBySerialNumberAsync(serialNumber);
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid id)
    {
        return await deviceRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await deviceRepository.GetAllAsync();
    }

    public async Task<Device> RegisterDeviceAsync(Device device)
    {
        var existing = await deviceRepository.GetBySerialNumberAsync(device.SerialNumber);
        if (existing != null)
        {
            throw new InvalidOperationException($"Device with serial number {device.SerialNumber} already exists");
        }

        return await deviceRepository.AddAsync(device);
    }

    public async Task UpdateDeviceHeartbeatAsync(string serialNumber)
    {
        var device = await deviceRepository.GetBySerialNumberAsync(serialNumber);
        if (device != null)
        {
            await deviceRepository.UpdateLastOnlineAsync(device.Id, DateTime.UtcNow);
        }
    }

    public async Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId)
    {
        return await context.DeviceCommands
            .Where(c => c.DeviceId == deviceId && c.Status == CommandStatus.Created)
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceCommand>> GetCommandsAsync(Guid deviceId)
    {
        return await context.DeviceCommands
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceCommand>> GetAllDeviceCommandsAsync(Guid deviceId)
    {
        return await context.DeviceCommands.Where(i => i.DeviceId == deviceId).ToListAsync();
    }

    public async Task<DeviceCommand> CreateCommandAsync(DeviceCommand command)
    {
        await context.DeviceCommands.AddAsync(command);
        await context.SaveChangesAsync();
        return command;
    }

    public async Task MarkCommandAsSentAsync(Guid commandId)
    {
        var command = await context.DeviceCommands.FindAsync(commandId);
        if (command != null)
        {
            command.Status = CommandStatus.Sent;
            command.SentAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
    }

    public async Task UpdateCommandStatusAsync(long commandId, CommandStatus status, string? responseData, string? errorMessage)
    {
        var command = await context.DeviceCommands.SingleOrDefaultAsync(i => i.CommandId == commandId);
        if (command != null)
        {
            command.Status = status;
            command.ResponseData = responseData;
            command.CompletedAt = DateTime.UtcNow;
            command.ErrorMessage = errorMessage;
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteDeviceAsync(Guid deviceId)
    {
        var device = await deviceRepository.GetByIdAsync(deviceId);
        await deviceRepository.DeleteAsync(device);
    }

    public async Task<AppResponse<bool>> IsValidUserAsync(User user)
    {
        var existing = await context.UserDevices.Include(i => i.Device).FirstOrDefaultAsync(i => i.DeviceId == user.DeviceId && i.PIN == user.PIN);
        
        return existing == null ? AppResponse<bool>.Success() : AppResponse<bool>.Error($"User PIN ({user.PIN}) is existed in device {existing.Device.DeviceName}).");
    }
}