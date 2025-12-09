namespace ZKTecoADMS.Application.Commands.SalaryProfiles.DeleteSalaryProfile;

public record DeleteSalaryProfileCommand(Guid Id) : ICommand<AppResponse<bool>>;
