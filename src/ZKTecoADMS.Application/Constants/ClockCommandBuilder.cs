using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Constants;

public static class ClockCommandBuilder
{
    public static string BuildAddUserCommand(User user)
    {
        return $"DATA UPDATE USERINFO PIN={user.PIN}\tName={user.FullName}\tPri={user.Privilege}\tPasswd={user.Password}\tCard={user.CardNumber}\tGrp={user.GroupId}\tTZ=0000\tVerifyMode={user.VerifyMode}";
    }
}