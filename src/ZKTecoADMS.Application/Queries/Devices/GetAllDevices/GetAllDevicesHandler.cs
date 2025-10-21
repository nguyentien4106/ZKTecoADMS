using Mapster;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Devices.GetAllDevices;

public class GetAllDevicesHandler(IRepositoryPagedQuery<Device> deviceRepository) : IQueryHandler<GetAllDevicesQuery, AppResponse<PagedResult<DeviceDto>>>
{

    public async Task<AppResponse<PagedResult<DeviceDto>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        var results = await deviceRepository.GetPagedResultAsync(request.Request, cancellationToken: cancellationToken);
        
        return AppResponse<PagedResult<DeviceDto>>.Success(results.Adapt<PagedResult<DeviceDto>>());
    }
}
