using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;
using ZKTecoADMS.Infrastructure;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Services;


public class DeviceService(
    IRepository<Device> deviceRepository,
    ILogger<DeviceService> logger,
    ZKTecoDbContext context)
    : IDeviceService
{

    public async Task<Device?> GetDeviceBySerialNumberAsync(string serialNumber)
    {
        return await deviceRepository.GetSingleAsync(d => d.SerialNumber == serialNumber
        );
        // return await deviceRepository.GetBySerialNumberAsync(serialNumber);
    }

    public async Task UpdateDeviceHeartbeatAsync(string serialNumber)
    {
        var device = await GetDeviceBySerialNumberAsync(serialNumber);
        if (device != null)
        {
            device.LastOnline = DateTime.Now;
            device.DeviceStatus = "Online";
            device.UpdatedAt = DateTime.Now;
            await deviceRepository.UpdateAsync(device);
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
            command.SentAt = DateTime.Now;
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
            command.CompletedAt = DateTime.Now;
            command.ErrorMessage = errorMessage;
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteDeviceAsync(Guid deviceId)
    {
        var device = await deviceRepository.GetByIdAsync(deviceId);
        await deviceRepository.DeleteAsync(device);
    }

    public async Task<AppResponse<bool>> IsValidEmployeeAsync(Employee employee)
    {
        var existing = await context.Employees.Include(i => i.Device).FirstOrDefaultAsync(i => i.DeviceId == employee.DeviceId && i.Pin == employee.Pin);
        
        return existing == null ? AppResponse<bool>.Success() : AppResponse<bool>.Error($"Employee PIN ({employee.Pin}) is existed in device {existing.Device.DeviceName}).");
    }

    public async Task<IEnumerable<Device>> GetAllDevicesByEmployeeAsync(Guid employeeId)
    {
        return await context.Devices.Where(d => d.ApplicationUserId == employeeId).ToListAsync();
    }

    public Task<IEnumerable<Device>> GetAllDevicesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsExistDeviceAsync(string serialNumber)
    {
        return await GetDeviceBySerialNumberAsync(serialNumber) != null;
    }
}