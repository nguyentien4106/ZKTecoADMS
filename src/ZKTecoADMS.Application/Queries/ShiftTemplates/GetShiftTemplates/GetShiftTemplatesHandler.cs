using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.ShiftTemplates.GetShiftTemplates;

public class GetShiftTemplatesHandler(IRepository<ShiftTemplate> repository) 
    : IQueryHandler<GetShiftTemplatesQuery, AppResponse<List<ShiftTemplateDto>>>
{
    public async Task<AppResponse<List<ShiftTemplateDto>>> Handle(GetShiftTemplatesQuery request, CancellationToken cancellationToken)
    {
        var templates = await repository.GetAllAsync(
            filter: t => t.ManagerId == request.ManagerId,
            orderBy: query => query.OrderByDescending(t => t.CreatedAt),
            includeProperties: new[] { nameof(ShiftTemplate.Manager) },
            cancellationToken: cancellationToken);
        
        var templateDtos = templates.Select(t => new ShiftTemplateDto
        {
            Id = t.Id,
            ManagerId = t.ManagerId,
            ManagerName = t.Manager?.UserName ?? string.Empty,
            Name = t.Name,
            StartTime = t.StartTime,
            EndTime = t.EndTime,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        }).ToList();

        return AppResponse<List<ShiftTemplateDto>>.Success(templateDtos);
    }
}
