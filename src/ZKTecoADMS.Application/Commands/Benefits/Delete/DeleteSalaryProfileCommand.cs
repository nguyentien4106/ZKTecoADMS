namespace ZKTecoADMS.Application.Commands.Benefits.Delete;

public record DeleteSalaryProfileCommand(Guid Id) : ICommand<AppResponse<bool>>;
