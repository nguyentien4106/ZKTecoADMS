using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Queries.Users.GetUserDevices;

public class GetUserDevicesHandler(IRepository<User> userRepository) : IQueryHandler<GetUserDevicesQuery, AppResponse<IEnumerable<UserResponse>>>
{
    public async Task<AppResponse<IEnumerable<UserResponse>>> Handle(GetUserDevicesQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllAsync(
            i => request.DeviceIds.Contains(i.DeviceId),
            includeProperties: ["Device"],
            cancellationToken: cancellationToken);

        return AppResponse<IEnumerable<UserResponse>>.Success(users.Adapt<IEnumerable<UserResponse>>());
    }
}