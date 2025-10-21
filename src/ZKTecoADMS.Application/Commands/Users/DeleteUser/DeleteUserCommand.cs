namespace ZKTecoADMS.Application.Commands.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId) : ICommand<AppResponse<Guid>>;