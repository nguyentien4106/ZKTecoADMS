namespace ZKTecoADMS.Application.Commands.Benefits.Delete;

public record DeleteBenefitCommand(Guid Id) : ICommand<AppResponse<bool>>;
