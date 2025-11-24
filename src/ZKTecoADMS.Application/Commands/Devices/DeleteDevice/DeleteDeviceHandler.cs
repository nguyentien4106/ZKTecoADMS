namespace ZKTecoADMS.Application.Commands.Devices.DeleteDevice;

public class DeleteDeviceHandler(IRepository<Device> deviceRepository) : ICommandHandler<DeleteDeviceCommand, AppResponse<Guid>>
{
    public async Task<AppResponse<Guid>> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await deviceRepository.DeleteByIdAsync(request.Id, cancellationToken);
        
        return result ? AppResponse<Guid>.Success(request.Id) : AppResponse<Guid>.Error("Something went wrong");
    }
}