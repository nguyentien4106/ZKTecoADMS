using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.ActiveDeviceCommand;

public class ToggleActiveHandler(IRepository<Device> deviceRepository) : ICommandHandler<ToggleActiveCommand, AppResponse<DeviceDto>>
{
    public async Task<AppResponse<DeviceDto>> Handle(ToggleActiveCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken: cancellationToken);
        if (device == null)
        {
            return  AppResponse<DeviceDto>.Error("Device not found");
        }
        
        device.IsActive = !device.IsActive;
        var result = await deviceRepository.UpdateAsync(device, cancellationToken);

        return result ? AppResponse<DeviceDto>.Success(device.Adapt<DeviceDto>()) : AppResponse<DeviceDto>.Error("Something went wrong");
    }
}