using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

public class PostBiometricStrategy : IPostStrategy
{
    public Task ProcessDataAsync(Device device, string body)
    {
        throw new NotImplementedException();
    }
}