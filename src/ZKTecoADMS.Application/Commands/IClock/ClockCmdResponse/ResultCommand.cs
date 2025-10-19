using ZKTecoADMS.Application.CQRS;

namespace ZKTecoADMS.Application.Commands.IClock.ClockCmdResponse;

public record ResultCommand(string SN, Stream Body) : ICommand<string>;