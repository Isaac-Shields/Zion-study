using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesRUD : MonoBehaviour
{
    public Button backBtn;
    public Button editBtn;
    public Button deleteBtn;
    public TMP_InputField notesTitle;
    public TMP_InputField notesBody;
    public MasterScript master;

    public void Start()
    {
        backBtn.onClick.AddListener(goBack);
    }
    public void fillNote(string title, string body)
    {
        master.closeNotesCanvas();
        master.startNoteRUDCanvas();
        master.closeNavBarCanvas();
        notesTitle.text = title;
        notesBody.text = body;
    }

    private void goBack()
    {
        master.closeAddNoteCanvas();
        master.startNotesCanvas();
        master.startNavBarCanvas();
    }


}
