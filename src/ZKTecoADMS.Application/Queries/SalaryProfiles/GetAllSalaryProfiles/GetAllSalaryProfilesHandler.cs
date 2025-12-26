using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles.GetAllSalaryProfiles;

public class GetAllSalaryProfilesHandler(ISalaryProfileRepository repository) 
    : IQueryHandler<GetAllSalaryProfilesQuery, AppResponse<List<SalaryProfileDto>>>
{
    public async Task<AppResponse<List<SalaryProfileDto>>> Handle(GetAllSalaryProfilesQuery request, CancellationToken cancellationToken)
    {
        var profiles = request.ActiveOnly == true
            ? await repository.GetActiveProfilesAsync(cancellationToken)
            : (await repository.GetAllAsync(cancellationToken: cancellationToken)).ToList();

        return AppResponse<List<SalaryProfileDto>>.Success(profiles.Adapt<List<SalaryProfileDto>>());
    }
}
