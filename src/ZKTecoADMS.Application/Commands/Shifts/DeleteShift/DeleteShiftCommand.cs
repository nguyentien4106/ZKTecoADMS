namespace ZKTecoADMS.Application.Commands.Shifts.DeleteShift;

public record DeleteShiftCommand(Guid Id) : ICommand<AppResponse<bool>>;
