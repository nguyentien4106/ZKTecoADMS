using Mapster;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Devices.GetAllDevices;

public class GetAllDevicesHandler(IRepositoryPagedQuery<Device> deviceRepository) : IQueryHandler<GetAllDevicesQuery, AppResponse<PagedResult<DeviceDto>>>
{

    public async Task<AppResponse<PagedResult<DeviceDto>>> Handle(GetAllDevicesQuery request, CancellationToken cancellationToken)
    {
        PagedResult<Device> results;
        
        // Admin gets all devices, Manager/others get only their devices
        if (request.IsAdminRequest)
        {
            results = await deviceRepository.GetPagedResultAsync(
                request.Request, 
                cancellationToken: cancellationToken
            );
        }
        else
        {
            // Filter by userId for non-admin users
            results = await deviceRepository.GetPagedResultAsync(
                request.Request,
                filter: d => d.ApplicationUserId == request.UserId,
                cancellationToken: cancellationToken
            );
        }
        
        return AppResponse<PagedResult<DeviceDto>>.Success(results.Adapt<PagedResult<DeviceDto>>());
    }
}
