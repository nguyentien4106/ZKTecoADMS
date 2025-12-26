using ZKTecoADMS.Application.DTOs.DeviceUsers;

namespace ZKTecoADMS.Application.Queries.DeviceUsers.GetDeviceUserDevices;

public class GetDeviceUserDevicesHandler(
    IRepository<DeviceUser> deviceUserRepository
    ) : IQueryHandler<GetDeviceUserDevicesQuery, AppResponse<IEnumerable<DeviceUserDto>>>
{
    public async Task<AppResponse<IEnumerable<DeviceUserDto>>> Handle(GetDeviceUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var deviceUsers = await deviceUserRepository.GetAllAsync(
            i => request.DeviceIds.Contains(i.DeviceId) && i.IsActive,
            includeProperties: [nameof(DeviceUser.Device), nameof(DeviceUser.Employee)],
            orderBy: query => query.OrderByDescending(i => i.DeviceId).ThenBy(i => i.Pin),
            cancellationToken: cancellationToken);

        return AppResponse<IEnumerable<DeviceUserDto>>.Success(deviceUsers.Adapt<IEnumerable<DeviceUserDto>>());
    }
}