using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.PIN, src => src.PIN)
            .Map(dest => dest, src => src);
    }
}