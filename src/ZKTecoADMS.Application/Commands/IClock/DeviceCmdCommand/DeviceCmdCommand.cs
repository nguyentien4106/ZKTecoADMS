namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public record DeviceCmdCommand(string SN, string Body) : ICommand<string>;