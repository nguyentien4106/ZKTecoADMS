using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;

public class ShiftTemplateService(
    IRepository<ShiftTemplate> repository,
    ILogger<ShiftTemplateService> logger) : IShiftTemplateService
{
    public async Task<ShiftTemplate?> GetTemplateByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetSingleAsync(
            st => st.Id == id,
            includeProperties: new[] { nameof(ShiftTemplate.Manager) },
            cancellationToken: cancellationToken);
    }

    public async Task<List<ShiftTemplate>> GetTemplatesByManagerIdAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var templates = await repository.GetAllAsync(
            filter: st => st.ManagerId == managerId,
            orderBy: query => query.OrderByDescending(st => st.CreatedAt),
            includeProperties: new[] { nameof(ShiftTemplate.Manager) },
            cancellationToken: cancellationToken);

        return templates.ToList();
    }

    public async Task<List<ShiftTemplate>> GetActiveTemplatesAsync(CancellationToken cancellationToken = default)
    {
        var templates = await repository.GetAllAsync(
            filter: st => st.IsActive,
            orderBy: query => query.OrderBy(st => st.Name),
            includeProperties: new[] { nameof(ShiftTemplate.Manager) },
            cancellationToken: cancellationToken);

        return templates.ToList();
    }

    public async Task<ShiftTemplate> CreateTemplateAsync(ShiftTemplate template, CancellationToken cancellationToken = default)
    {
        ValidateTemplate(template);

        var createdTemplate = await repository.AddAsync(template, cancellationToken);
        
        logger.LogInformation(
            "Created shift template: {TemplateName} by Manager {ManagerId}",
            template.Name,
            template.ManagerId);

        return createdTemplate;
    }

    public async Task<ShiftTemplate> UpdateTemplateAsync(ShiftTemplate template, CancellationToken cancellationToken = default)
    {
        ValidateTemplate(template);

        var existingTemplate = await GetTemplateByIdAsync(template.Id, cancellationToken);
        if (existingTemplate == null)
        {
            throw new InvalidOperationException($"Shift template with ID {template.Id} not found");
        }

        existingTemplate.Name = template.Name;
        existingTemplate.StartTime = template.StartTime;
        existingTemplate.EndTime = template.EndTime;
        existingTemplate.IsActive = template.IsActive;

        await repository.UpdateAsync(existingTemplate, cancellationToken);
        
        logger.LogInformation(
            "Updated shift template: {TemplateName} (ID: {TemplateId})",
            template.Name,
            template.Id);

        return existingTemplate;
    }

    public async Task<bool> DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var template = await GetTemplateByIdAsync(id, cancellationToken);
        if (template == null)
        {
            logger.LogWarning("Attempted to delete non-existent shift template: {TemplateId}", id);
            return false;
        }

        await repository.DeleteAsync(template, cancellationToken);
        
        logger.LogInformation(
            "Deleted shift template: {TemplateName} (ID: {TemplateId})",
            template.Name,
            id);

        return true;
    }

    public async Task<bool> ActivateTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var template = await GetTemplateByIdAsync(id, cancellationToken);
        if (template == null)
        {
            return false;
        }

        template.IsActive = true;
        await repository.UpdateAsync(template, cancellationToken);
        
        logger.LogInformation("Activated shift template: {TemplateId}", id);
        return true;
    }

    public async Task<bool> DeactivateTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var template = await GetTemplateByIdAsync(id, cancellationToken);
        if (template == null)
        {
            return false;
        }

        template.IsActive = false;
        await repository.UpdateAsync(template, cancellationToken);
        
        logger.LogInformation("Deactivated shift template: {TemplateId}", id);
        return true;
    }

    private void ValidateTemplate(ShiftTemplate template)
    {
        if (string.IsNullOrWhiteSpace(template.Name))
        {
            throw new ArgumentException("Template name is required", nameof(template.Name));
        }

        if (template.StartTime >= template.EndTime)
        {
            throw new ArgumentException("Start time must be before end time");
        }
    }
}
