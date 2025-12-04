using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IDeviceCmdService
{
    Task<IEnumerable<DeviceCommand>> GetCreatedCommandsAsync(Guid deviceId);
    
    Task<bool> UpdateCommandStatusAsync(Guid commandId, CommandStatus status);
    
    Task<bool> UpdateCommandAfterExecutedAsync(ClockCommandResponse commandResponse);
    
    Task<(DeviceCommandTypes, Guid)> GetCommandTypesAndIdAsync(long commandId);
}