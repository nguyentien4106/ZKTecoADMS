using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Infrastructure;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Services;


public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ILogger<DeviceService> _logger;
    private readonly ZKTecoDbContext _context;

    public DeviceService(
        IDeviceRepository deviceRepository,
        ILogger<DeviceService> logger,
        ZKTecoDbContext context)
    {
        _deviceRepository = deviceRepository;
        _logger = logger;
        _context = context;
    }

    public async Task<Device> GetOrCreateDeviceAsync(string serialNumber)
    {
        var device = await _deviceRepository.GetBySerialNumberAsync(serialNumber);
        
        if (device == null)
        {
            _logger.LogInformation("Creating new device with SN: {SerialNumber}", serialNumber);
            device = new Device
            {
                SerialNumber = serialNumber,
                DeviceName = $"Device-{serialNumber}",
                IsActive = true,
                DeviceStatus = "Online"
            };
            await _deviceRepository.AddAsync(device);
        }

        return device;
    }

    public async Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber)
    {
        return await _deviceRepository.GetBySerialNumberAsync(serialNumber);
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid id)
    {
        return await _deviceRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        return await _deviceRepository.GetAllAsync();
    }

    public async Task<Device> RegisterDeviceAsync(Device device)
    {
        var existing = await _deviceRepository.GetBySerialNumberAsync(device.SerialNumber);
        if (existing != null)
        {
            throw new InvalidOperationException($"Device with serial number {device.SerialNumber} already exists");
        }

        return await _deviceRepository.AddAsync(device);
    }

    public async Task UpdateDeviceHeartbeatAsync(string serialNumber)
    {
        var device = await _deviceRepository.GetBySerialNumberAsync(serialNumber);
        if (device != null)
        {
            await _deviceRepository.UpdateLastOnlineAsync(device.Id, DateTime.UtcNow);
        }
    }

    public async Task<IEnumerable<DeviceCommand>> GetPendingCommandsAsync(Guid deviceId)
    {
        return await _context.DeviceCommands
            .Where(c => c.DeviceId == deviceId && c.Status == CommandStatus.Created)
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<DeviceCommand>> GetAllDeviceCommandsAsync(Guid deviceId)
    {
        return await _context.DeviceCommands.Where(i => i.DeviceId == deviceId).ToListAsync();
    }

    public async Task<DeviceCommand> CreateCommandAsync(DeviceCommand command)
    {
        await _context.DeviceCommands.AddAsync(command);
        await _context.SaveChangesAsync();
        return command;
    }

    public async Task MarkCommandAsSentAsync(Guid commandId)
    {
        var command = await _context.DeviceCommands.FindAsync(commandId);
        if (command != null)
        {
            command.Status = CommandStatus.Sent;
            command.SentAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateCommandStatusAsync(long commandId, CommandStatus status, string? responseData, string? errorMessage)
    {
        var command = await _context.DeviceCommands.SingleOrDefaultAsync(i => i.CommandId == commandId);
        if (command != null)
        {
            command.Status = status;
            command.ResponseData = responseData;
            command.CompletedAt = DateTime.UtcNow;
            command.ErrorMessage = errorMessage;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteDeviceAsync(Guid deviceId)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId);
        await _deviceRepository.DeleteAsync(device);
    }

    public async Task<AppResponse<bool>> IsValidUserAsync(User user)
    {
        var existing = await _context.UserDevices.Include(i => i.Device).FirstOrDefaultAsync(i => i.DeviceId == user.DeviceId && i.PIN == user.PIN);
        
        return existing == null ? AppResponse<bool>.Success() : AppResponse<bool>.Error($"User PIN ({user.PIN}) is existed in device {existing.Device.DeviceName}).");
    }
}