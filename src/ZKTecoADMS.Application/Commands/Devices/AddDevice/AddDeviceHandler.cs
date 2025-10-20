using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public class AddDeviceHandler(IRepository<Device> repository) : ICommandHandler<AddDeviceCommand, AppResponse<DeviceResponse>>
{
    public async Task<AppResponse<DeviceResponse>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = request.Adapt<Device>();

        var response = await repository.AddAsync(device, cancellationToken);
        
        return response == null ? 
            AppResponse<DeviceResponse>.Error("Failed to add new device. Please try again! Or contact admin site.")
            : AppResponse<DeviceResponse>.Success(response.Adapt<DeviceResponse>());
    }
}
