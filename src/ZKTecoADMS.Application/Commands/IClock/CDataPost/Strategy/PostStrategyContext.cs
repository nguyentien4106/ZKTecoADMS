using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostStrategyContext(string table)
{
    private readonly IPostStrategy _strategy = table switch
    {
        "ATTLOG" => new PostAttendancesStrategy(),
        "OPERLOG" => new PostUsersStrategy(),
        _ => new PostBiometricStrategy()
    };

    public async Task ExecuteAsync(Device device, string body)
    {
        await _strategy.ProcessDataAsync(device, body);
    }
}