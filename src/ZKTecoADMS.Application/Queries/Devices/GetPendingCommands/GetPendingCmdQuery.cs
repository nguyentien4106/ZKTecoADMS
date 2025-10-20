using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Queries.Devices.GetPendingCommands;

public record GetPendingCmdQuery(Guid DeviceId) : IQuery<AppResponse<IEnumerable<DeviceCmdResponse>>>;