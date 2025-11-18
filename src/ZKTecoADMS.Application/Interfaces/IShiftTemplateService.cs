using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Interfaces;

public interface IShiftTemplateService
{
    Task<ShiftTemplate?> GetTemplateByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ShiftTemplate>> GetTemplatesByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<List<ShiftTemplate>> GetActiveTemplatesAsync(CancellationToken cancellationToken = default);
    Task<ShiftTemplate> CreateTemplateAsync(ShiftTemplate template, CancellationToken cancellationToken = default);
    Task<ShiftTemplate> UpdateTemplateAsync(ShiftTemplate template, CancellationToken cancellationToken = default);
    Task<bool> DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ActivateTemplateAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeactivateTemplateAsync(Guid id, CancellationToken cancellationToken = default);
}
