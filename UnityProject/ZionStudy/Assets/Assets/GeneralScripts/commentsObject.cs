using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class commentObj
{
    private string title;
    private string body;
    private int commentId;
    private int setId;

    public string getTitle()
    {
        return title;
    }
    public string getBody()
    {
        return body;
    }
    public int getCommentId()
    {
        return commentId;
    }
    public int getSetId()
    {
        return setId;
    }

    public void setTitle(string t)
    {
        title = t;
    }
    public void setBody(string b)
    {
        body = b;
    }
    public void setCommentId(int id)
    {
        commentId = id;
    }
    public void setSetId(int sid)
    {
        setId = sid;
    }
}