using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.DeviceUsers;

namespace ZKTecoADMS.Application.Queries.DeviceUsers.GetDeviceUserDevices;

public class GetDeviceUserDevicesHandler(
    IRepository<DeviceUser> deviceUserRepository
    ) : IQueryHandler<GetDeviceUserDevicesQuery, AppResponse<IEnumerable<DeviceUserDto>>>
{
    public async Task<AppResponse<IEnumerable<DeviceUserDto>>> Handle(GetDeviceUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var deviceUsers = await deviceUserRepository.GetAllWithIncludeAsync(
            i => request.DeviceIds.Contains(i.DeviceId) && i.IsActive,
            includes: query => query.Include(i => i.Device).Include(i => i.Employee!),
            orderBy: query => query.OrderByDescending(i => i.Device.DeviceName).ThenBy(i => i.Pin),
            cancellationToken: cancellationToken
        );

        return AppResponse<IEnumerable<DeviceUserDto>>.Success(deviceUsers.Adapt<List<DeviceUserDto>>());
    }
}