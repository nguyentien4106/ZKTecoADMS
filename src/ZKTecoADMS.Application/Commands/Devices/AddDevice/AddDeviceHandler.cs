using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Devices.AddDevice;

public class AddDeviceHandler(
    IRepository<Device> repository,
    IRepository<DeviceCommand> deviceCommandRepository,
    IDeviceService deviceService
    ) : ICommandHandler<AddDeviceCommand, AppResponse<DeviceDto>>
{
    public async Task<AppResponse<DeviceDto>> Handle(AddDeviceCommand request, CancellationToken cancellationToken)
    {
        var existing = await deviceService.IsExistDeviceAsync(request.SerialNumber);
        if (existing)
        {
            return AppResponse<DeviceDto>.Error($"Device Serial Number: {request.SerialNumber} already exists");
        }
        
        var deviceEntity = request.Adapt<Device>();
        var device = await repository.AddAsync(deviceEntity, cancellationToken);
        
        var syncUsersCommand = new DeviceCommand
        {
            DeviceId = device.Id,
            CommandType = DeviceCommandTypes.SyncEmployees,
            Priority = 10,
            Command = ClockCommandBuilder.BuildGetAllUsersCommand()
        };

        await deviceCommandRepository.AddAsync(syncUsersCommand, cancellationToken);

        return AppResponse<DeviceDto>.Success(device.Adapt<DeviceDto>());
    }
}
