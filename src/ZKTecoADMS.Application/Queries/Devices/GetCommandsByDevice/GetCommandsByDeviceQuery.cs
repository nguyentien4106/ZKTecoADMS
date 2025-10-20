using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Queries.Devices.GetCommandsByDevice;

public record GetCommandsByDeviceQuery(Guid DeviceId) : IQuery<AppResponse<IEnumerable<DeviceCmdResponse>>>;
