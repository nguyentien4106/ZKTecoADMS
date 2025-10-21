using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public class AddDeviceHandler(IRepository<Device> repository, IDeviceService deviceService) : ICommandHandler<AddDeviceCommand, AppResponse<DeviceDto>>
{
    public async Task<AppResponse<DeviceDto>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var existing = await deviceService.GetDeviceBySerialNumberAsync(request.SerialNumber);
        if (existing != null)
        {
            return AppResponse<DeviceDto>.Error($"Device Serial Number: {request.SerialNumber} already exists");
        }
        
        var device = request.Adapt<Device>();

        var response = await repository.AddAsync(device, cancellationToken);
        
        return AppResponse<DeviceDto>.Success(response.Adapt<DeviceDto>());
    }
}
