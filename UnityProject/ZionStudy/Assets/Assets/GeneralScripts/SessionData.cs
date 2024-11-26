public class UserSessionData
{
    private int userId;
    private string userName;
    private string password;
    private int isAdmin;

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

    public void setAdminLevel(int level)
    {
        isAdmin = level;
    }

    public int getAdminLevel()
    {
        return isAdmin;
    }

    public void clearData()
    {
        userId = -1;
        userName = "";
        password = "";
        isAdmin = 0;
    }
}