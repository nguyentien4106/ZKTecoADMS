namespace ZKTecoADMS.Application.Commands.Devices.ActiveDeviceCommand;

public class ToggleActiveHandler(IRepository<Device> deviceRepository) : ICommandHandler<ToggleActiveCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ToggleActiveCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken: cancellationToken);
        device.IsActive = !device.IsActive;
        
        var result = await deviceRepository.UpdateAsync(device, cancellationToken);
        
        return result ? AppResponse<bool>.Success(true) : AppResponse<bool>.Error("Something went wrong");
    }
}