using Mapster;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest, src => src);
    }
}