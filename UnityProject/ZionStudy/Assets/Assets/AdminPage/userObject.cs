using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userObject
{
    private int userId;
    private string userName;
    private string password;
    private int userLevel;

    public void setUid(int uid)
    {
        userId = uid;
    }

    public int getUid()
    {
        return userId;
    }

    public void setUserName(string uName)
    {
        userName = uName;
    }

    public string getUserName()
    {
        return userName;
    }

    public void setPassword(string p)
    {
        password = p;
    }

    public string getUserPassword()
    {
        return password;
    }

    public void setUserLevel(int level)
    {
        userLevel = level;
    }

    public int getUserLevel()
    {
        return userLevel;
    }


}
