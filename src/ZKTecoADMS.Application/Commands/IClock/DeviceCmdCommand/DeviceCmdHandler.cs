using System.Text;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public class DeviceCmdHandler(IDeviceService deviceService) : ICommandHandler<DeviceCmdCommand, string>
{
    public async Task<string> Handle(DeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var sn = request.SN;
        
        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        var body = await reader.ReadToEndAsync(cancellationToken);
        var response = body.ParseClockResponse();
    
        await deviceService.UpdateCommandStatusAsync(response.ID, CommandStatus.Sent, response.CMD, response.Message);
        
        return ClockResponses.Ok;
    }
}