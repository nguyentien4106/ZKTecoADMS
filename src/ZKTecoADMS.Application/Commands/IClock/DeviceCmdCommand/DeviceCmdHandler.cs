using ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public class DeviceCmdHandler(
    IDeviceCmdService deviceCmdService,
    IDeviceCommandStrategyFactory strategyFactory
    ) : ICommandHandler<DeviceCmdCommand, string>
{
    public async Task<string> Handle(DeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var response = request.Body.ParseClockResponse();
        
        await deviceCmdService.UpdateCommandAfterExecutedAsync(response);
        var (commandType, objectRefId) = await deviceCmdService.GetCommandTypesAndIdAsync(response.CommandId);

        var strategy = strategyFactory.GetStrategy(commandType);
        if (strategy != null)
        {
            await strategy.ExecuteAsync(objectRefId, response, cancellationToken);
        }
        
        return ClockResponses.Ok;
    }
}