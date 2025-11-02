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
        var existing = await deviceService.GetDeviceBySerialNumberAsync(request.SerialNumber);
        if (existing != null)
        {
            return AppResponse<DeviceDto>.Error($"Device Serial Number: {request.SerialNumber} already exists");
        }
        
        var device = request.Adapt<Device>();

        var response = await repository.AddAsync(device, cancellationToken);
        var initialUsers = new DeviceCommand
        {
            DeviceId = response.Id,
            CommandType = DeviceCommandTypes.InitialUsers,
            Priority = 10,
            Command = ClockCommandBuilder.BuildGetAllUsersCommand()
        };


        await deviceCommandRepository.AddAsync(initialUsers, cancellationToken);

        return AppResponse<DeviceDto>.Success(response.Adapt<DeviceDto>());
    }
}
