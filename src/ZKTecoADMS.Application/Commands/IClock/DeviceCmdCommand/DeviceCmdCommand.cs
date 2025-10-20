namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public record DeviceCmdCommand(string SN, Stream Body) : ICommand<string>;