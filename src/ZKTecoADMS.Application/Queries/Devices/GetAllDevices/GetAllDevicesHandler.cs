using Mapster;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Devices.GetAllDevices;

public class GetAllDevicesHandler(IRepositoryPagedQuery<Device> deviceRepository) : IQueryHandler<GetAllDevicesQuery, AppResponse<PagedResult<DeviceResponse>>>
{

    public async Task<AppResponse<PagedResult<DeviceResponse>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        var results = await deviceRepository.GetPagedResultAsync(request.Request, cancellationToken: cancellationToken);
        
        return AppResponse<PagedResult<DeviceResponse>>.Success(results.Adapt<PagedResult<DeviceResponse>>());
    }
}
