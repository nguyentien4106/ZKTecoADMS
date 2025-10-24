using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostStrategyContext(IServiceProvider serviceProvider, string table)
{
    private readonly IPostStrategy _strategy = table switch
    {
        "ATTLOG" => new PostAttendancesStrategy(serviceProvider),
        "OPERLOG" => new OperLogStrategy(serviceProvider),
        _ => new PostBiometricStrategy(serviceProvider)
    };

    public async Task<string> ExecuteAsync(Device device, string body)
    {
        return await _strategy.ProcessDataAsync(device, body);
    }
}