using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;

public class DeviceCmdService(IRepository<DeviceCommand> deviceCmdRepository) : IDeviceCmdService
{
    public async Task<IEnumerable<DeviceCommand>> GetCreatedCommandsAsync(Guid deviceId)
    {
        return await deviceCmdRepository.GetAllAsync(cmd => cmd.DeviceId == deviceId && cmd.Status == CommandStatus.Created);
    }

    public async Task<bool> UpdateCommandStatusAsync(Guid commandId, CommandStatus status)
    {
        var command = await deviceCmdRepository.GetByIdAsync(commandId);
        if (command == null)
        {
            return false;
        }
        
        command.Status = status;
        if (status == CommandStatus.Sent)
        {
            command.SentAt = DateTime.Now;
        }
        
        return await deviceCmdRepository.UpdateAsync(command);
    }

    public async Task<bool> UpdateCommandAfterExecutedAsync(ClockCommandResponse response)
    {
        var command = await deviceCmdRepository.GetSingleAsync(c => c.CommandId == response.CommandId);
        if (command == null)
        {
            return false;
        }
        
        command.Status = response.IsSuccess ? CommandStatus.Success : CommandStatus.Failed;
        command.ResponseData = response.CMD;
        command.ErrorMessage = response.Message;
        command.Return = response.Return;
        command.CompletedAt = DateTime.Now;
        
        return await deviceCmdRepository.UpdateAsync(command);
    }

    public async Task<(DeviceCommandTypes, Guid)> GetCommandTypesAndIdAsync(long commandId)
    {
        var command = await deviceCmdRepository.GetSingleAsync(c => c.CommandId == commandId);
        if (command == null)
        {
            throw new KeyNotFoundException($"Device command with ID {commandId} not found.");
        }
        
        return (command.CommandType, command.ObjectReferenceId);
    }
}