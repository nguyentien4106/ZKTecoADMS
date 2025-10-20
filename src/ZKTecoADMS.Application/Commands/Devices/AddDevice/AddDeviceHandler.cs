using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public class AddDeviceHandler(IRepository<Device> repository, IDeviceService deviceService) : ICommandHandler<AddDeviceCommand, AppResponse<DeviceResponse>>
{
    public async Task<AppResponse<DeviceResponse>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var existing = await deviceService.GetDeviceBySerialNumberAsync(request.SerialNumber);
        if (existing != null)
        {
            return AppResponse<DeviceResponse>.Error($"Device Serial Number: {request.SerialNumber} already exists");
        }
        
        var device = request.Adapt<Device>();

        var response = await repository.AddAsync(device, cancellationToken);
        
        return AppResponse<DeviceResponse>.Success(response.Adapt<DeviceResponse>());
    }
}
