using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Queries.Users.GetUserDevices;

public record GetUserDevicesQuery(IEnumerable<Guid> DeviceIds) : IQuery<AppResponse<IEnumerable<UserDto>>>;