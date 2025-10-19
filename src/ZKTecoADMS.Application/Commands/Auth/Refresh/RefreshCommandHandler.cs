using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.DTOs.Auth;
using ZKTecoADMS.Application.Interfaces.Auth;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Auth.Refresh;

public class RefreshCommandHandler(
    IRefreshTokenValidatorService tokenValidatorService,
    IAuthenticateService authenticateService,
    IRepository<UserRefreshToken> refreshTokenRepository
    ) : ICommandHandler<RefreshCommand, AppResponse<AuthenticateResponse>>
{
    public async Task<AppResponse<AuthenticateResponse>> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        if (!tokenValidatorService.Validate(command.RefreshToken))
        {
            return AppResponse<AuthenticateResponse>.Error("Invalid refresh token.");
        }

        var userRefreshToken = await refreshTokenRepository.GetSingleAsync(
            urt => urt.RefreshToken == command.RefreshToken,
            ["ApplicationUser"],
            cancellationToken
        );

        return await authenticateService.Authenticate(userRefreshToken.ApplicationUser, cancellationToken);
    }
}
