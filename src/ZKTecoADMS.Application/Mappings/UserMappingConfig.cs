using ZKTecoADMS.Application.Commands.Users.CreateUser;
using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Mappings;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.DeviceName, src => src.Device.DeviceName)
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest, src => src);

        // Explicit mapping for CreateUserRequest to CreateUserCommand
        config.NewConfig<CreateUserRequest, CreateUserCommand>()
            .Map(dest => dest.Pin, src => src.Pin)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.CardNumber, src => src.CardNumber)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.Privilege, src => src.Privilege)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.Department, src => src.Department)
            .Map(dest => dest.DeviceIds, src => src.DeviceIds);
    }
}