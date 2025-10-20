using Mapster;
using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Queries.Devices.GetCommandsByDevice;

public class GetCommandsByDeviceHandler(IRepository<DeviceCommand> deviceCmdRepository) : IQueryHandler<GetCommandsByDeviceQuery, AppResponse<IEnumerable<DeviceCmdResponse>>>
{

    public async Task<AppResponse<IEnumerable<DeviceCmdResponse>>> Handle(GetCommandsByDeviceQuery request, CancellationToken cancellationToken)
    {
        var commands = await deviceCmdRepository.GetAllAsync(dc => dc.DeviceId == request.DeviceId, cancellationToken: cancellationToken);
        return AppResponse<IEnumerable<DeviceCmdResponse>>.Success(commands.Adapt<IEnumerable<DeviceCmdResponse>>());
    }
}
