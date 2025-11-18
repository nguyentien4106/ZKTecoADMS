using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.UpdateShiftTemplate;

public class UpdateShiftTemplateHandler(IRepository<ShiftTemplate> repository) 
    : ICommandHandler<UpdateShiftTemplateCommand, AppResponse<ShiftTemplateDto>>
{
    public async Task<AppResponse<ShiftTemplateDto>> Handle(UpdateShiftTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate time range
            if (request.StartTime >= request.EndTime)
            {
                return AppResponse<ShiftTemplateDto>.Error("Start time must be before end time");
            }

            var template = await repository.GetSingleAsync(
                t => t.Id == request.Id,
                includeProperties: new[] { nameof(ShiftTemplate.Manager) },
                cancellationToken: cancellationToken);
            
            if (template == null)
            {
                return AppResponse<ShiftTemplateDto>.Error("Shift template not found");
            }

            template.Name = request.Name;
            template.StartTime = request.StartTime;
            template.EndTime = request.EndTime;
            template.IsActive = request.IsActive;

            await repository.UpdateAsync(template, cancellationToken);
            
            var templateDto = new ShiftTemplateDto
            {
                Id = template.Id,
                ManagerId = template.ManagerId,
                ManagerName = template.Manager?.UserName ?? string.Empty,
                Name = template.Name,
                StartTime = template.StartTime,
                EndTime = template.EndTime,
                IsActive = template.IsActive,
                CreatedAt = template.CreatedAt,
                UpdatedAt = template.UpdatedAt
            };

            return AppResponse<ShiftTemplateDto>.Success(templateDto);
        }
        catch (ArgumentException ex)
        {
            return AppResponse<ShiftTemplateDto>.Error(ex.Message);
        }
    }
}
