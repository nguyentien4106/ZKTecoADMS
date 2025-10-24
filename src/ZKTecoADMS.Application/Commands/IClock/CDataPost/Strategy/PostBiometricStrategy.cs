using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostBiometricStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    public Task<string> ProcessDataAsync(Device device, string body)
    {
        throw new NotImplementedException();
    }
}