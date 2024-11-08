public class UserSessionData
{
    private int userId;
    private string userName;
    private string password;

    public int getUserId()
    {
        return userId;
    }

    public void setUserId(int ui)
    {
        userId = ui;
    }

    public string getUserName()
    {
        return userName;
    }

    public void setUsername(string un)
    {
        userName = un;
    }

    public string getUserPassword()
    {
        return password;
    }

    public void setUserPassword(string up)
    {
        password = up;
    }
}