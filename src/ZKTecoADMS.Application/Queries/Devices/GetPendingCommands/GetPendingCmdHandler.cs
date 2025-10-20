using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Devices.GetPendingCommands;

public class GetPendingCmdHandler(IDeviceService deviceService) : IQueryHandler<GetPendingCmdQuery, AppResponse<IEnumerable<DeviceCmdResponse>>>
{
    public async Task<AppResponse<IEnumerable<DeviceCmdResponse>>> Handle(GetPendingCmdQuery request, CancellationToken cancellationToken)
    {
        var pendingCmds = await deviceService.GetPendingCommandsAsync(request.DeviceId);

        return AppResponse<IEnumerable<DeviceCmdResponse>>.Success(pendingCmds.Adapt<IEnumerable<DeviceCmdResponse>>());
    }
}