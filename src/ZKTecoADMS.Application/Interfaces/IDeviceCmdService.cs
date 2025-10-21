using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IDeviceCmdService
{
    Task<IEnumerable<DeviceCommand>> GetCreatedCommandsAsync(Guid deviceId);
    
    Task UpdateCommandStatusAsync(Guid commandId, CommandStatus status);
    
    Task UpdateCommandAfterExecutedAsync(ClockCommandResponse commandResponse);
    
    Task<(DeviceCommandTypes, Guid)> GetCommandTypesAndIdAsync(long commandId);
}