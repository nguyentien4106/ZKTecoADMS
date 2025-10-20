using Mapster;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Devices.GetDevicesByUser;
public class GetDevicesByUserHandler(IRepository<Device> deviceRepository) : IQueryHandler<GetDevicesByUserQuery, AppResponse<IEnumerable<DeviceResponse>>>
{
    public async Task<AppResponse<IEnumerable<DeviceResponse>>> Handle(GetDevicesByUserQuery request, CancellationToken cancellationToken)
    {
        var devices = await deviceRepository
            .GetAllAsync(d => d.ApplicationUserId == request.UserId, cancellationToken: cancellationToken);

        return AppResponse<IEnumerable<DeviceResponse>>.Success(devices.Adapt<IEnumerable<DeviceResponse>>());
    }
}