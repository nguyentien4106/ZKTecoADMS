using System.Text;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.IClock.GetRequest;

public class GetRequestHandler(
    IDeviceService deviceService,
    IDeviceCmdService deviceCmdService
    ) : ICommandHandler<GetRequestQuery, string>
{
    public async Task<string> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        var sn = request.SN;
        
        var device = await deviceService.GetDeviceBySerialNumberAsync(sn);
        if (device == null)
        {
            return ClockResponses.Ok;
        }

        var commands = await deviceCmdService.GetCreatedCommandsAsync(device.Id);

        var deviceCommands = commands.ToList();

        if (deviceCommands.Count == 0) return ClockResponses.Ok;
        
        var response = new StringBuilder();
        foreach (var command in deviceCommands.OrderByDescending(c => c.Priority))
        {
            var cmd = $"C:{command.CommandId}:{command.Command}";
            response.AppendLine(cmd);
                    
            await deviceCmdService.UpdateCommandStatusAsync(command.Id, CommandStatus.Sent);
        }

        return response.ToString();

    }
}