using ZKTecoADMS.Application.Commands.Benefits.Delete;

namespace ZKTecoADMS.Application.Commands.Benefits.Delete;

public class DeleteBenefitHandler(IRepository<Benefit> repository) 
    : ICommandHandler<DeleteBenefitCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteBenefitCommand request, CancellationToken cancellationToken)
    {
        var salaryProfile = await repository.GetByIdAsync(request.Id, includeProperties: [nameof(Benefit.EmployeeBenefits)],cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<bool>.Error("Benefit profile not found");
        }

        if (salaryProfile.EmployeeBenefits.Count > 0)
        {
            return  AppResponse<bool>.Error("Cannot delete benefit profile assigned to employees");
        }

        var deleted = await repository.DeleteByIdAsync(request.Id, cancellationToken);
        return deleted 
            ? AppResponse<bool>.Success(true) 
            : AppResponse<bool>.Error("Failed to delete salary profile");
    }
}
