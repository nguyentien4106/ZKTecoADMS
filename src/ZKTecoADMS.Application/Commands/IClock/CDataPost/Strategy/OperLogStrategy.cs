using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class OperLogStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    public Task ProcessDataAsync(Device device, string body)
    {
        
        return Task.CompletedTask;
    }
}