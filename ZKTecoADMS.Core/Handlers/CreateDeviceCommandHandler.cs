using MediatR;
using ZKTeco.Domain.Entities;
using ZKTecoADMS.Core.Commands;
using ZKTecoADMS.Infrastructure.Repositories;

namespace ZKTecoADMS.Core.Handlers;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Device>
{
    private readonly IDeviceRepository _deviceRepository;

    public CreateDeviceCommandHandler(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;
    }

    public async Task<Device> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var device = new Device
        {
            SerialNumber = request.SerialNumber,
            DeviceName = request.DeviceName,
            Model = request.Model,
            IpAddress = request.IpAddress,
            Port = request.Port,
            Location = request.Location,
            IsActive = false
        };

        await _deviceRepository.AddAsync(device);
        return device;
    }
}