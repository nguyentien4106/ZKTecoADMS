using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Queries.Devices.GetDeviceById;

public class GetDeviceByIdHandler(IRepository<Device> deviceRepository) 
    : IQueryHandler<GetDeviceByIdQuery, AppResponse<DeviceResponse>>
{
    public async Task<AppResponse<DeviceResponse>> Handle(GetDeviceByIdQuery request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        return AppResponse<DeviceResponse>.Success(device.Adapt<DeviceResponse>());
    }
}
