using ZKTecoADMS.Application.DTOs.Devices;

namespace ZKTecoADMS.Application.Commands.Devices.CreateDeviceCmd;

public class CreateDeviceCmdHandler(IRepository<Device> deviceRepository, IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateDeviceCmdCommand, AppResponse<DeviceCmdDto>>
{
    public async Task<AppResponse<DeviceCmdDto>> Handle(CreateDeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByIdAsync(request.DeviceId, cancellationToken: cancellationToken);

        var command = new DeviceCommand
        {
            DeviceId = device.Id,
            Command = request.Command,
            Priority = request.Priority
        };
        
        var created = await deviceCmdRepository.AddAsync(command, cancellationToken);

        return AppResponse<DeviceCmdDto>.Success(created.Adapt<DeviceCmdDto>());
    }
}