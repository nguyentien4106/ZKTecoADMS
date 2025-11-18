using Microsoft.AspNetCore.Identity;
using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.ShiftTemplates.GetShiftTemplates;

public class GetShiftTemplatesHandler(
    IRepository<ShiftTemplate> repository,
    UserManager<ApplicationUser> userManager
    ) : IQueryHandler<GetShiftTemplatesQuery, AppResponse<List<ShiftTemplateDto>>>
{
    public async Task<AppResponse<List<ShiftTemplateDto>>> Handle(GetShiftTemplatesQuery request, CancellationToken cancellationToken)
    {
        Guid managerId = Guid.Empty;
        if(request.IsManager){
            managerId = request.ManagerId;
        }
        else
        {
            var employeeUser = await userManager.FindByIdAsync(request.ManagerId.ToString());
            if (employeeUser == null)
            {
                return AppResponse<List<ShiftTemplateDto>>.Error($"User with ID '{request.ManagerId}' not found.");
            }
            if (employeeUser.ManagerId == null)
            {
                return AppResponse<List<ShiftTemplateDto>>.Error("Employee does not have a manager assigned.");
            }
            managerId = employeeUser.ManagerId.Value;
        }     
           
        var templates = await repository.GetAllAsync(
            filter: t => t.ManagerId == managerId,
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
