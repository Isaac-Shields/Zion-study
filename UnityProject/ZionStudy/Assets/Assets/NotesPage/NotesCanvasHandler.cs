using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesCanvasHandler : MonoBehaviour
{
    public Button addNewNoteBtn;
    public MasterScript master;
    void Start()
    {
        addNewNoteBtn.onClick.AddListener(addNote);
    }

    private void addNote()
    {
        master.startAddNotesCanvas();
        master.closeNavBarCanvas();
    }



}
