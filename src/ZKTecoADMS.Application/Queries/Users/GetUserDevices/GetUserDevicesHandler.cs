using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Queries.Users.GetUserDevices;

public class GetUserDevicesHandler(IRepository<User> userRepository) : IQueryHandler<GetUserDevicesQuery, AppResponse<IEnumerable<UserDto>>>
{
    public async Task<AppResponse<IEnumerable<UserDto>>> Handle(GetUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(
            i => request.DeviceIds.Contains(i.DeviceId),
            includeProperties: ["Device", "ApplicationUser"],
            orderBy: query => query.OrderByDescending(i => i.DeviceId).ThenBy(i => i.Pin),
            cancellationToken: cancellationToken);

        return AppResponse<IEnumerable<UserDto>>.Success(users.Adapt<IEnumerable<UserDto>>());
    }
}