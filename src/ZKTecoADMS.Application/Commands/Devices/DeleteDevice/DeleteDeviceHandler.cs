namespace ZKTecoADMS.Application.Commands.Devices.DeleteDevice;

public class DeleteDeviceHandler(IRepository<Device> deviceRepository) : ICommandHandler<DeleteDeviceCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        var result = await deviceRepository.DeleteByIdAsync(request.Id, cancellationToken);
        
        return result ? AppResponse<bool>.Success(result) : AppResponse<bool>.Error("Something went wrong");
    }
}