using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.SalaryProfiles.DeleteSalaryProfile;

public class DeleteSalaryProfileHandler(ISalaryProfileRepository repository) 
    : ICommandHandler<DeleteSalaryProfileCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        var salaryProfile = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<bool>.Error("Salary profile not found");
        }

        var deleted = await repository.DeleteByIdAsync(request.Id, cancellationToken);
        return deleted 
            ? AppResponse<bool>.Success(true) 
            : AppResponse<bool>.Error("Failed to delete salary profile");
    }
}
