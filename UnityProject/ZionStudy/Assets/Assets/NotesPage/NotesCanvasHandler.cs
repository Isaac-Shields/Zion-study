using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesCanvasHandler : MonoBehaviour
{
    public Button addNewNoteBtn;
    public Button notesGoBackBtn;
    public MasterScript master;
    void Start()
    {
        addNewNoteBtn.onClick.AddListener(addNote);
        notesGoBackBtn.onClick.AddListener(goBackBtn);
    }

    private void addNote()
    {
        master.startAddNotesCanvas();
        master.closeNavBarCanvas();
    }

    private void goBackBtn()
    {
        master.startNotesCanvas();
        master.startNavBarCanvas();
    }



}
