using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Devices;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Application.Queries.Devices.GetAllDevices;

public record GetAllDevicesQuery(PaginationRequest Request) : IQuery<AppResponse<PagedResult<DeviceDto>>>;
