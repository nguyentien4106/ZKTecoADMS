using ZKTecoADMS.Application.DTOs.Commons;

namespace ZKTecoADMS.Application.Queries.DeviceUsers.GetDeviceUsersByManager;

public record GetDeviceUsersByManagerQuery(Guid ManagerId) : IQuery<AppResponse<IEnumerable<AccountDto>>>;
