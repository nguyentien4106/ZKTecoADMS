namespace ZKTecoADMS.Application.Constants;

public static class ClockCommandBuilder
{
    public static string BuildAddOrUpdateUserCommand(User user)
    {
        return $"DATA UPDATE USERINFO PIN={user.PIN}\tName={user.FullName}\tPri={user.Privilege}\tPasswd={user.Password}\tCard={user.CardNumber}\tGrp={user.GroupId}\tTZ=0000\tVerifyMode={user.VerifyMode}";
    }

    public static string BuildDeleteUserCommand(string pin)
    {
        return $"DATA DELETE USERINFO PIN={pin}";
    }
}