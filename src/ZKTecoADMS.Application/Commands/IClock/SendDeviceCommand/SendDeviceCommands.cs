using ZKTecoADMS.Application.CQRS;

namespace ZKTecoADMS.Application.Commands.SendDeviceCommand;

public record SendDeviceCommands(string SN) : ICommand<string>;