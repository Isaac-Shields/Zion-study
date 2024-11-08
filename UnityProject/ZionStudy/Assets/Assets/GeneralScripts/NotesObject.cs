public class notesObj
{
    private string title;
    private string body;

    private int noteId;

    public notesObj()
    {

    }
    public notesObj(string t, string b)
    {

    }

    public void setNoteTitle(string t)
    {
        title = t;
    }

    public void setNoteBody(string b)
    {
        body = b;
    }

    public string getTitle()
    {
        return title;
    }

    public string getBody()
    {
        return body;
    }

    public int getNoteId()
    {
        return noteId;
    }

    public void setNoteId(int ni)
    {
        noteId = ni;
    }
}