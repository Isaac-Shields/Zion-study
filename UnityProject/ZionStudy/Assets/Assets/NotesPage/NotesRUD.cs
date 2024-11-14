using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesRUD : MonoBehaviour
{
    public Button backBtn;
    public Button updateBtn;
    public Button deleteBtn;
    public TMP_InputField notesTitle;
    public TMP_InputField notesBody;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    public TextMeshProUGUI messageText;
    private int noteId;
    private string originalNote;
    private string originalTitle;

    public void Start()
    {
        backBtn.onClick.AddListener(goBack);
        updateBtn.onClick.AddListener(handleUpdate);
        deleteBtn.onClick.AddListener(deleteNote);
    }
    
    public void fillNote(string title, string body, int nid)
    {
        master.closeNotesCanvas();
        master.startNoteRUDCanvas();
        master.closeNavBarCanvas();
        notesTitle.text = title;
        notesBody.lineType = TMP_InputField.LineType.MultiLineNewline;
        notesBody.text = body;

        noteId = nid;
        originalNote = body;
        originalTitle = title;
    }

    private void goBack()
    {
        master.closeAddNoteCanvas();
        master.startNotesCanvas();
        master.startNavBarCanvas();
        messageText.text = "";
    }

    private void handleUpdate()
    {
        if(originalNote != notesBody.text || originalTitle != notesTitle.text)
        {
            if(dbHelper.updateNotes(notesTitle.text, notesBody.text, noteId))
            {
                messageText.text = "Note updated successfully.";
                messageText.color = Color.green;
            }
            else
            {
                messageText.text = "Error updating note.";
                messageText.color = Color.red;
            }
        }
        else
        {
            messageText.text = "Nothing to save.";
            messageText.color = Color.yellow;
        }
    }

    private void deleteNote()
    {
        if(dbHelper.deleteNote(noteId))
        {
            messageText.text = "Note deleted successfully.";
            messageText.color = Color.green;
            goBack();
        }
        else
        {
            messageText.text = "Error deleted note.";
            messageText.color = Color.red;
        }
    }




}
