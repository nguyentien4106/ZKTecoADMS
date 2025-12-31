using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.CreateShiftTemplate;

public class CreateShiftTemplateHandler(IRepository<ShiftTemplate> repository) 
    : ICommandHandler<CreateShiftTemplateCommand, AppResponse<ShiftTemplateDto>>
{
    public async Task<AppResponse<ShiftTemplateDto>> Handle(CreateShiftTemplateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate time range
            if (request.StartTime >= request.EndTime)
            {
                return AppResponse<ShiftTemplateDto>.Error("Start time must be before end time");
            }

            var shiftTemplate = new ShiftTemplate
            {
                ManagerId = request.ManagerId,
                Name = request.Name,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                BreakTimeMinutes = request.BreakTimeMinutes,
                IsActive = true
            };

            var createdTemplate = await repository.AddAsync(shiftTemplate, cancellationToken);
            
            // Load the manager relationship
            var templateWithManager = await repository.GetSingleAsync(
                t => t.Id == createdTemplate.Id,
                includeProperties: new[] { nameof(ShiftTemplate.Manager) },
                cancellationToken: cancellationToken);
            
            var templateDto = new ShiftTemplateDto
            {
                Id = templateWithManager!.Id,
                ManagerId = templateWithManager.ManagerId,
                ManagerName = templateWithManager.Manager?.UserName ?? string.Empty,
                Name = templateWithManager.Name,
                StartTime = templateWithManager.StartTime,
                EndTime = templateWithManager.EndTime,
                IsActive = templateWithManager.IsActive,
                CreatedAt = templateWithManager.CreatedAt,
                UpdatedAt = templateWithManager.UpdatedAt
            };

            return AppResponse<ShiftTemplateDto>.Success(templateDto);
        }
        catch (ArgumentException ex)
        {
            return AppResponse<ShiftTemplateDto>.Error(ex.Message);
        }
    }
}
