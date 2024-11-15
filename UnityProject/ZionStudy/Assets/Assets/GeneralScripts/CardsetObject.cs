using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cardsetObj
{
    private string cardsetTitle;
    private int cardsetId;

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


}
