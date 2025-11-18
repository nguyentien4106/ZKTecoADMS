using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.ShiftTemplates.GetShiftTemplates;

public record GetShiftTemplatesQuery(Guid ManagerId) : IQuery<AppResponse<List<ShiftTemplateDto>>>;
