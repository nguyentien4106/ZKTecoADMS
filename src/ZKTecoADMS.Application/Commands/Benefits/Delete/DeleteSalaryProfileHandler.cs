using ZKTecoADMS.Application.Commands.Benefits.Delete;

namespace ZKTecoADMS.Application.Commands.Benefits.Delete;

public class DeleteSalaryProfileHandler(IRepository<Benefit> repository) 
    : ICommandHandler<DeleteSalaryProfileCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteSalaryProfileCommand request, CancellationToken cancellationToken)
    {
        var salaryProfile = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        if (salaryProfile == null)
        {
            return AppResponse<bool>.Error("Benefit profile not found");
        }

        var deleted = await repository.DeleteByIdAsync(request.Id, cancellationToken);
        return deleted 
            ? AppResponse<bool>.Success(true) 
            : AppResponse<bool>.Error("Failed to delete salary profile");
    }
}
