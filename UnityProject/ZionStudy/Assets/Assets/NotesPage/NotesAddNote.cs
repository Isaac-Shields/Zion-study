using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesAddNote : MonoBehaviour
{
    public TMP_InputField notesTitle;
    public TMP_InputField notesBody;
    public Button backBtn;
    public Button saveBtn;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    public TextMeshProUGUI errorMsg;

    private void Start()
    {
        backBtn.onClick.AddListener(goBack);
        saveBtn.onClick.AddListener(saveNote);
    }

    private void goBack()
    {
        master.closeAddNoteCanvas();
    }

    public void clearCanvas()
    {
        notesTitle.text = "";
        notesBody.text = "";
        errorMsg.text = "";
    }

    private void saveNote()
    {
        if(notesTitle.text.Length > 0 && notesBody.text.Length > 0)
        {
            int uid = master.curSessionData.getUserId();

            if(dbHelper.addNoteToDatabase(notesTitle.text, notesBody.text, uid))
            {
                errorMsg.text = "Note saved.";
                errorMsg.color = Color.green;
            }
            else
            {
                errorMsg.text = "Error saving note.";
                errorMsg.color = Color.red;
            }
        }
    }
}
