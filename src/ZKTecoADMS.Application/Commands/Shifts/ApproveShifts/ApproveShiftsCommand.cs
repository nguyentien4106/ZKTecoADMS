namespace ZKTecoADMS.Application.Commands.Shifts.ApproveShifts;

public record ApproveShiftsCommand(
    List<Guid> Ids
    ) : ICommand<AppResponse<List<Guid>>>;