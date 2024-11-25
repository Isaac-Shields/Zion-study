using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardsetObj
{
    private string cardsetTitle;
    private int cardsetId = -1;
    private int cardUserId = -1;

    public cardsetObj()
    {

    }

    public void setId(int id)
    {
        cardsetId = id;
    }

    public int getSetId()
    {
        return cardsetId;
    }

    public void setCardsetTitle(string title)
    {
        cardsetTitle = title;
    }

    public string getCardsetTitle()
    {
        return cardsetTitle;
    }

    public void clearData()
    {
        cardsetTitle = "";
        cardsetId = -1;
    }

    public int getCardUserId()
    {
        return cardUserId;
    }

    public void setCardUserId(int cuid)
    {
        cardUserId = cuid;
    }

}
